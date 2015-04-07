using System;
using log4net.Config;
using MassTransit;
using MassTransit.Log4NetIntegration;
using MassTransitDemo.Common;

namespace RuntimeServices.Publisher
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
                    sbc.UseControlBus();

                    sbc.UseMsmq(
                        mq   =>
                        {
                            mq.VerifyMsmqConfiguration();
                            mq.UseSubscriptionService(QueueLocations.RUNTIME_SERVICES_SUBSCRIPTION_QUEUE);
                        });

                    sbc.ReceiveFrom(QueueLocations.RUNTIME_SERVICES_PUBLISHER_QUEUE);                    

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
