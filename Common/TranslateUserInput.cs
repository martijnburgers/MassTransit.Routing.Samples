using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;

namespace MassTransitDemo.Common
{

    public static class TranslateUserInput
    {
        public static bool IntoSendingMessages(Uri subscriberQueue)
        {
            bool keepRunning = true;

            Console.WriteLine(@"Please enter your command:");
            Console.WriteLine(@"
a) Send 1 message
b) Send 10 messages
c) Send 100 messages
d) Send 1000 messages
e) Send 10000 messages
-------------------------------------------------------------------
q) Quit
");
            char key = char.ToLower(Console.ReadKey(true).KeyChar);

            switch (key)
            {
                case 'a':
                    Send(1, subscriberQueue);
                    break;
                case 'b':
                    Send(10, subscriberQueue);
                    break;
                case 'c':
                    Send(100, subscriberQueue);
                    break;
                case 'd':
                    Send(1000, subscriberQueue);
                    break;
                case 'e':
                    Send(10000, subscriberQueue);
                    break;
                case 'q':
                    Console.WriteLine("Quitting");

                    Console.WriteLine("Shutting down service bus");

                    Bus.Shutdown();

                    keepRunning = false;
                    break;
                default:
                    Console.WriteLine("Unknown command. Try again." + Environment.NewLine);
                    break;
            }

            return keepRunning;
        }

        public static void Send(int numberOfMessages, Uri destinationQueue)
        {
            Console.WriteLine("Sending {0} messages", numberOfMessages);

            Func<IEnumerable<YourMessage>, ParallelLoopResult> transmitFunc = messages =>
            {
                IEndpoint destinationEndpoint = Bus.Instance.GetEndpoint(destinationQueue);

                return Parallel.ForEach(
                    messages,
                    new ParallelOptions { MaxDegreeOfParallelism = 10 },
                    destinationEndpoint.Send);
            };

            Transmit(numberOfMessages, transmitFunc);
        }

        public static bool IntoPublishingMessages()
        {
            bool keepRunning = true;

            Console.WriteLine(@"Please enter your command:");
            Console.WriteLine(@"
a) Publish 1 message
b) Publish 10 messages
c) Publish 100 messages
d) Publish 1000 messages
e) Publish 10000 messages
-------------------------------------------------------------------
q) Quit
");
            char key = char.ToLower(Console.ReadKey(true).KeyChar);

            switch (key)
            {
                case 'a':
                    Publish(1);
                    break;
                case 'b':
                    Publish(10);
                    break;
                case 'c':
                    Publish(100);
                    break;
                case 'd':
                    Publish(1000);
                    break;
                case 'e':
                    Publish(10000);
                    break;
                case 'q':
                    Console.WriteLine("Quitting");

                    Console.WriteLine("Shutting down service bus");

                    Bus.Shutdown();

                    keepRunning = false;
                    break;
                default:
                    Console.WriteLine("Unknown command. Try again." + Environment.NewLine);
                    break;
            }

            return keepRunning;
        }

        public static void Publish(int numberOfMessages)
        {
            Console.WriteLine("Publishing {0} messages", numberOfMessages);

            Func<IEnumerable<YourMessage>, ParallelLoopResult> transmitFunc =
                messages =>
                    Parallel.ForEach(messages, new ParallelOptions {MaxDegreeOfParallelism = 10}, Bus.Instance.Publish);

            Transmit(numberOfMessages, transmitFunc);
        }

        public static void Transmit(
            int numberOfMessages,
            Func<IEnumerable<YourMessage>, ParallelLoopResult> transmitFunc)
        {
            Console.WriteLine("Publishing {0} messages", numberOfMessages);

            //we throttle the maximum number messages created in memory at once. This throtteling has nothing
            //to do with the parallelism, which is set to 10. Not very usefull in this usecase because the messages
            //are very small. Took this code from another project where we used this code to test messages with large
            // payloads (databus properties).

            int maximumNumberOfmessagesAtOnce = 100;
            int maximumCeiledLoopCount = 1;
            double preciseLoopCount = 1;

            if (numberOfMessages > 100)
            {
                preciseLoopCount = (double) numberOfMessages/100;
                maximumCeiledLoopCount = (int) Math.Ceiling(preciseLoopCount);
            }
            else
            {
                maximumNumberOfmessagesAtOnce = numberOfMessages;
            }

            for (int currentLoopCount = 0; currentLoopCount < maximumCeiledLoopCount; currentLoopCount++)
            {
                int copyMaxiumNumbersOfmessages = maximumNumberOfmessagesAtOnce;
                int copyCurrentLoopCount = currentLoopCount;

                if (copyCurrentLoopCount != 0 && copyCurrentLoopCount + 1 == maximumCeiledLoopCount)
                {
                    copyMaxiumNumbersOfmessages =
                        (int) ((preciseLoopCount - Math.Floor(preciseLoopCount))*copyMaxiumNumbersOfmessages);
                }

                IEnumerable<YourMessage> messages =
                    Enumerable.Range(0, copyMaxiumNumbersOfmessages)
                        .Select(
                            rangeIndex =>
                                new YourMessage
                                {
                                    Id = copyCurrentLoopCount*maximumNumberOfmessagesAtOnce + rangeIndex,
                                    Text = "Yoo"
                                });

                transmitFunc(messages);
            }
        }
    }
}