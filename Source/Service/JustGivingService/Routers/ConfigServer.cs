using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JustGivingThrift;
using JustGivingService.Controllers;
using Thrift.Transport;
using Thrift.Server;

namespace JustGivingService.Routers
{
    public static class ConfigServer
    {
        public static void EntryPoint()
        {
            ConfigController configController = ConfigController.Instance;
            ConfigService.Processor processor = new ConfigService.Processor(configController);
            TServerTransport serverTransport = new TServerSocket(9090);
            TServer server = new TSimpleServer(processor, serverTransport);

            server.Serve();
        }
    }
}
