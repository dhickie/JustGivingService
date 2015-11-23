using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using JustGivingThrift;
using System.IO;

namespace JustGivingService.Controllers
{
    class ConfigController : ConfigService.Iface
    {
        // The singleton instance
        private static ConfigController instance;
        private static object instanceLock = new object();

        // Member variables
        private String m_rainmeterInstallDir;
        private int m_apiPollingPeriod;
        private List<String> m_pageIds;
        private object m_configLock;

        private ConfigController()
        {
            m_configLock = new object();

            try
            {
                lock (m_configLock)
                {
                    m_pageIds = new List<string>();

                    // On initialisation, read the xml config file from the service's executable directory
                    XmlReader reader = XmlReader.Create("ServiceConfig.xml");

                    // Rainmeter install directory
                    reader.ReadToFollowing("RainmeterInstallDir");
                    reader.Read();
                    m_rainmeterInstallDir = reader.Value;

                    // Polling period
                    reader.ReadToFollowing("PollingPeriod");
                    reader.Read();
                    m_apiPollingPeriod = Int32.Parse(reader.Value);

                    // Page IDs
                    while (reader.ReadToFollowing("PageId"))
                    {
                        reader.Read();
                        m_pageIds.Add(reader.Value);
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                // TODO add some logging here
            }
        }

        public static ConfigController Instance
        {
            get
            {
                // Do a double-check lock to make sure two threads don't try to initialise
                // at the same time.
                if (instance == null)
                {
                    lock (instanceLock)
                    {
                        if (instance == null)
                        {
                            instance = new ConfigController();
                        }
                    }
                }
                return instance;
            }
        }

        // Accessors
        public static String GetRainmeterDir()
        {
            return Instance.GetRainmeterDirImpl();
        }

        private String GetRainmeterDirImpl()
        {
            return m_rainmeterInstallDir;
        }

        public static int GetApiPollingPeriod()
        {
            return Instance.GetApiPollingPeriodImpl();
        }

        private int GetApiPollingPeriodImpl()
        {
            return m_apiPollingPeriod;
        }

        public static List<String> GetPageIds()
        {
            return Instance.GetPageIdsImpl();
        }

        private List<String> GetPageIdsImpl()
        {
            return m_pageIds;
        }

        // Thrift accessors
        public ServiceConfig GetConfiguration()
        {
            ServiceConfig config = new ServiceConfig();

            // We don't want something to be accessing the config if it's in the middle of being changed
            lock (m_configLock)
            {
                config.PageId = m_pageIds.First(); // Obviously this will change when the rainmeter skin supports multiple fundraisers
                config.PollingPeriod = m_apiPollingPeriod;
                config.RainmeterExe = m_rainmeterInstallDir;
            }

            return config;
        }

        public void SetConfiguration(ServiceConfig newConfig)
        {
            // Lock the config so we don't try to modify and dispatch the service configuration at the same time
            lock (m_configLock)
            {
                m_pageIds = new List<string>();
                m_pageIds.Add(newConfig.PageId);

                m_apiPollingPeriod = newConfig.PollingPeriod;
                m_rainmeterInstallDir = newConfig.RainmeterExe;
            }

            // Write the new config out to the config file for persistance
            using (XmlWriter writer = XmlWriter.Create("ServiceConfig.xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("JustGivingServiceConfig");

                // Rainmeter install directory
                writer.WriteStartElement("RainmeterInstallDir");
                writer.WriteString(m_rainmeterInstallDir);
                writer.WriteEndElement();

                // Polling period
                writer.WriteStartElement("PollingPeriod");
                writer.WriteString(m_apiPollingPeriod.ToString());
                writer.WriteEndElement();

                // Page ID
                writer.WriteStartElement("PageId");
                writer.WriteString(m_pageIds.First());
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}
