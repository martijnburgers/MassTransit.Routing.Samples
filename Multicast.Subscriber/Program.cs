using System;
using log4net.Config;
using MassTransit;
using MassTransit.Log4NetIntegration;
using MassTransitDemo.Common;

namespace Multicast.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(typeof(Program).Namespace);

            Console.WriteLine("Initializing service bus");

            XmlConfigurator.Configure();

            Bus.Initialize(sbc =>
            {                
                sbc.UseMsmq(
                    mq =>
                    {
                        mq.UseMulticastSubscriptionClient();
                        mq.VerifyMsmqConfiguration();
                    });
                sbc.ReceiveFrom(QueueLocations.MULTICAST_SUBSCRIBER_QUEUE);
                
                sbc.UseLog4Net();

                sbc.Subscribe(subs =>
                {
                    subs.Consumer<SomethingHappenedConsumer>();
                });
            });
         
            Console.ReadLine();
            
            Console.WriteLine("Shutting down service bus");

            Bus.Shutdown();
        }
    }
}
