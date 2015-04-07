using System;
using log4net.Config;
using MassTransit;
using MassTransit.Advanced;
using MassTransit.BusServiceConfigurators;
using MassTransit.Log4NetIntegration;
using MassTransitDemo.Common;

namespace StaticRouting.Subscriber
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
                    sbc.SetConcurrentReceiverLimit(1);
                    sbc.SetConcurrentConsumerLimit(1);

                    sbc.UseMsmq(x => x.VerifyMsmqConfiguration());

                    sbc.ReceiveFrom(QueueLocations.STATIC_ROUTING_SUBSCRIBER_QUEUE);

                    var busConfig = new CustomBusServiceConfigurator(new StaticRoutingConfig());

                    sbc.AddBusConfigurator(busConfig);

                    sbc.Subscribe(subs => { subs.Consumer<SomethingHappenedConsumer>(); });

                    sbc.UseLog4Net();
                });

            Console.ReadLine();

            Console.WriteLine("Shutting down service bus");

            Bus.Shutdown();
        }
    }
}