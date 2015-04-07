using System;
using log4net.Config;
using MassTransit;
using MassTransit.BusServiceConfigurators;
using MassTransit.Log4NetIntegration;
using MassTransitDemo.Common;

namespace StaticRouting.Publisher
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine(typeof (Program).Namespace);            

            Console.WriteLine("Initializing service bus");

            XmlConfigurator.Configure();

            Bus.Initialize(
                sbc =>
                {
                    sbc.UseMsmq(x => x.VerifyMsmqConfiguration());
                    
                    sbc.ReceiveFrom(QueueLocations.STATIC_ROUTING_PUBLISHER_QUEUE);
                    
                    var busConfig = new CustomBusServiceConfigurator(new StaticRoutingConfig());

                    sbc.AddBusConfigurator(busConfig);

                    sbc.UseLog4Net();
                });

            bool keepRunning = true;

            while (keepRunning)
            {
                keepRunning = TranslateUserInput.IntoPublishingMessages();
            }
        }    
    }
}