package justGivingServlet;

import java.io.IOException;
import java.io.InputStream;
import java.io.PrintWriter;
import java.util.ArrayList;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import javax.json.*;

import org.apache.thrift.transport.*;
import org.apache.thrift.protocol.*;
import justGivingThrift.*;

public class JustGivingServlet extends HttpServlet {
	private static final long serialVersionUID = 1L;
       
    public JustGivingServlet() {
        super();
    }

	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		ConfigService.Client client = null;
		
		// Get the config service client and get the current configuration
		try {
			client = getClient();
			ServiceConfig config = client.GetConfiguration();
			
			// Send the response back containing the current config
			sendConfigResponse(config, response);
		} catch (Exception e) {
			// Something went wrong getting the current config, throw a servlet exception
			throw new ServletException(e.toString());
		} finally {
			client.getInputProtocol().getTransport().close();
			client.getOutputProtocol().getTransport().close();
		}
	}

	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		// Get the input stream from the request, and convert it to a json object.
		InputStream inputStream = request.getInputStream();
		
		JsonReader reader = Json.createReader(inputStream);
		JsonObject config = reader.readObject();
		
		// Validate the input
		ArrayList<String> errors = validateConfigInput(config);
		if (errors.size() == 0) {
			// Get the config data from the json object and put it in to the thrift definition
			String pageId = config.getString("pageId");
			String pollingPeriod = config.getString("pollingPeriod");
			int pollingPeriodInt = Integer.parseInt(pollingPeriod);
			String rainmeterExe = config.getString("rainmeterExe");
			
			ServiceConfig newConfig = new ServiceConfig();
			newConfig.setPageId(pageId);
			newConfig.setPollingPeriod(pollingPeriodInt);
			newConfig.setRainmeterExe(rainmeterExe);
			
			// Try to send the new config to the config service server
			ConfigService.Client client = null;
			try {
				client = getClient();
				client.SetConfiguration(newConfig);
				
				// If it's successful, then send back the new config
				newConfig = client.GetConfiguration();
				sendConfigResponse(newConfig, response);
			}
			catch (Exception e){
				throw new ServletException(e.toString());
			} 
			finally {
				client.getInputProtocol().getTransport().close();
				client.getOutputProtocol().getTransport().close();
			}
		}
		else {
			// There were errors, so send these back to the UI to be displayed
			JsonBuilderFactory factory = Json.createBuilderFactory(null);
			JsonObjectBuilder builder = factory.createObjectBuilder();
			JsonArrayBuilder errorsBuilder = factory.createArrayBuilder();
			
			for (String error : errors) {
				errorsBuilder.add(error);
			}
			
			builder.add("errors", errorsBuilder.build());
			
			sendResponse(builder.build(), response);
		}
	}
	
	private void sendConfigResponse(ServiceConfig config, HttpServletResponse response) throws IOException {
		// Write the current config out to JSON in the way the config UI expects
		JsonBuilderFactory factory = Json.createBuilderFactory(null);
		JsonObjectBuilder configBuilder = factory.createObjectBuilder();
					
		configBuilder.add("pageId", config.pageId);
		configBuilder.add("pollingPeriod", String.valueOf(config.pollingPeriod));
		configBuilder.add("rainmeterExe", config.rainmeterExe);
					
		JsonObject builtConfig = configBuilder.build();
					
		// Send the json back in the response
		sendResponse(builtConfig, response);
	}
	
	private void sendResponse(JsonObject object, HttpServletResponse response) throws IOException {
		response.setContentType("application/json");
		PrintWriter output = response.getWriter();
		output.print(object.toString());
		output.flush();
	}
	
	private ArrayList<String> validateConfigInput(JsonObject newConfig) {
		ArrayList<String> errors = new ArrayList<>();
		
		// Check the polling period is actually an integer
		String pollingPeriod = newConfig.getString("pollingPeriod");
		try {
			int pollingPeriodInt = Integer.parseInt(pollingPeriod);
		} 
		catch (NumberFormatException e) {
			errors.add(pollingPeriod + " is not a valid polling period. It must be a positive whole number.");
		}
		
		return errors;
	}
	
	private ConfigService.Client getClient() throws TTransportException {
		TTransport transport = new TSocket("localhost", 9090);
		transport.open();
		TProtocol protocol = new TBinaryProtocol(transport);
		
		ConfigService.Client client = new ConfigService.Client(protocol);
		
		return client;
	}
}
