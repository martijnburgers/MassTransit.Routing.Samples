using System;
using log4net.Config;
using MassTransit;
using MassTransit.Log4NetIntegration;
using MassTransitDemo.Common;

namespace Endpoint.Publisher
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
                    sbc.UseMsmq(
                        mq =>
                        {
                            mq.VerifyMsmqConfiguration();                            
                        });

                    sbc.ReceiveFrom(QueueLocations.ENDPOINT_PUBLISHER_QUEUE);

                    sbc.UseLog4Net();
                });


            var subscriberQueue = new Uri(QueueLocations.ENDPOINT_SUBSCRIBER_QUEUE);
            bool keepRunning = true;

            while (keepRunning)
            {
                keepRunning = TranslateUserInput.IntoSendingMessages(subscriberQueue);
            }
        }
    }
}
