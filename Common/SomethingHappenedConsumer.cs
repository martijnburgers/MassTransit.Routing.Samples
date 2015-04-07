using System;
using MassTransit;

namespace MassTransitDemo.Common
{
    public class SomethingHappenedConsumer : Consumes<YourMessage>.All
    {
        public void Consume(YourMessage message)
        {
            Console.WriteLine("message received: [id = {0}, text = {1}, tid = {2}]", message.Id, message.Text, System.Threading.Thread.CurrentThread.ManagedThreadId);            
        }
    }    
}
