using System;
using log4net.Config;
using MassTransit;
using MassTransit.Log4NetIntegration;
using MassTransitDemo.Common;

namespace Endpoint.Subscriber
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine(typeof(Program).Namespace);
            
            Console.WriteLine("Initializing service bus");

            XmlConfigurator.Configure();

            Bus.Initialize(
                sbc =>
                {
                    sbc.UseMsmq(mq => mq.VerifyMsmqConfiguration());

                    sbc.ReceiveFrom(QueueLocations.ENDPOINT_SUBSCRIBER_QUEUE);

                    sbc.Subscribe(subs => { subs.Consumer<SomethingHappenedConsumer>(); });

                    sbc.UseLog4Net();
                });

            Console.WriteLine("Subscriber listening - press ENTER to quit");

            Console.ReadLine();

            Console.WriteLine("Shutting down service bus");

            Bus.Shutdown();
        }
    }
}
