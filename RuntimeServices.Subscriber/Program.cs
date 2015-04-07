using System;
using log4net.Config;
using MassTransit;
using MassTransit.Log4NetIntegration;
using MassTransitDemo.Common;

namespace RuntimeServices.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(typeof(Program).Namespace);     

            Console.WriteLine("Initializing service bus");

            XmlConfigurator.Configure();

            Bus.Initialize(
                sbc =>
                {
                    sbc.UseControlBus();

                    sbc.UseMsmq(
                        mq =>
                        {                            
                            mq.VerifyMsmqConfiguration();
                            mq.UseSubscriptionService(QueueLocations.RUNTIME_SERVICES_SUBSCRIPTION_QUEUE);
                        });

                    sbc.ReceiveFrom(QueueLocations.RUNTIME_SERVICES_SUBSCRIBER_QUEUE);
                    
                    sbc.Subscribe(subs => { subs.Consumer<SomethingHappenedConsumer>(); });

                    sbc.UseLog4Net();
                });

            Console.ReadLine();

            Console.WriteLine("Shutting down service bus");

            Bus.Shutdown();
        }
    }
}
