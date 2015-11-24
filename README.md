# JustGivingService
The JustGivingService is a collection of Windows and web applications which allows anyone to see the current status of a JustGiving fundraiser by simply glancing at their desktop. Details displayed include current funding level, target funding level, and a list of recent donations. Configuration is done via a simple and easy to use Web UI, and the details displayed on the desktop are updated in real-time using JustGiving’s publically available API.

The installation instructions below, where applicable, relate to installing the service on Windows 7.

## Prerequisites
There are number of prerequisites needed to use the JustGivingService:
-	.Net 4.5
-	Java Runtime Environment 8
-	Apache Tomcat 8
-	Rainmeter

Instructions for installing most of these can be seen below.

## Installation
### Service Installation instructions
To install the JustGivingService, follow these steps:

1.	If not already installed, install .Net 4.5.
2.	Install the Java Runtime Environment. If this is already installed, ensure the JRE_HOME environment variable is set (see below).
3.	Install Apache Tomcat 8. If this is already installed, ensure Tomcat is configured to run as a Windows service, CATALINA_HOME environment variable is set, and Tomcat is running on port 8080 (see below).
4.	Install Rainmeter. If this is already installed, two custom environment variables need to be set, RAINMETER_HOME and RAINMETER_SKINS_HOME (see below).
5.	Restart the computer.
6.	Download the latest version of the JustGivingService from the Releases folder above, and extract the archive to the location you wish to install the service to.
7.	Double click “Install.exe”.

### JRE Installation instructions
To install the Java Runtime Environment, do the following:

1.	Download the latest version of JRE 8 from [here](http://www.oracle.com/technetwork/java/javase/downloads/jre8-downloads-2133155.html).
2.	Run the installer.
3.	Create a new environment variable, JRE_HOME, with a value of the location you installed the JRE to. By default this will be something similar to “C:\Program Files (x86)\Java\jre1.8.0_66”.

### Apache Tomcat 8 installation instructions:
To install Apache Tomcat 8, do the following:

1.	Download the core zip distribution of Tomcat 8 from [here](https://tomcat.apache.org/download-80.cgi).
2.	Extract the zip file to where you want Tomcat to be installed.
3.	Create a new environment variable, CATALINA_HOME, with a value of the path to the Tomcat /bin directory. For example if you extracted the downloaded archive to the root of the C drive, this would look similar to “C:\apache-tomcat-8.0.28”.
4.	Start a command prompt, and enter the following commands in order:
  1.	%CATALINA_HOME%\bin\service.bat install
  2.	%CATALINA_HOME%\bin\tomcat8.exe //US//Tomcat8 --Startup=auto
  3.	%CATALINA_HOME%\bin\tomcat8.exe start

### Rainmeter installation instructions:
To install Rainmeter, do the following:

1.	Download the latest version of Rainmeter from [here](http://www.rainmeter.net/).
2.	Run the installer. Tick the option to start Rainmeter when Windows starts.
3.	Create two new environment variables:
  1.	RAINMETER_SKINS_HOME – This should have a value of the Rainmeter skins directory. By default this will be “C:\Users\{YourUsername}\Documents\Rainmeter\Skins”
  2.	RAINMETER_HOME – This should have a value of the directory you installed Rainmeter to. By default this will look similar to “C:\Program Files\Rainmeter”.
4.	Start Rainmeter.
5.	(Optional) The first time Rainmeter is run, it will load a number of default skins that are not required. These can be disabled by doing the following:
  1.	From the system tray, double click the Rainmeter icon.
  2.	In the resulting window, select each item in turn from the “Active skins” dropdown box, and click “Unload” for each.

### Creating new environment variables
To create a new environment variable, do the following:

1.	Go to Start -> Control Panel.
2.	From the control panel, go to System -> Advanced system settings.
3.	At the bottom of the resulting window, click “Environment Variables...”.
4.	Click “New...” in the “System variables” section and enter a new name and value for the environment variable.
