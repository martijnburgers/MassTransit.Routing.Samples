using System;
using MassTransit.Services.Routing.Configuration;

namespace MassTransitDemo.Common
{
    public class StaticRoutingConfig : RoutingConfigurator
    {
        public StaticRoutingConfig()
        {
            var staticRoutingPublisherQueueLocation = new Uri(QueueLocations.STATIC_ROUTING_PUBLISHER_QUEUE);
            var staticRoutingSubscriberQueueLocation = new Uri(QueueLocations.STATIC_ROUTING_SUBSCRIBER_QUEUE);

            Route<YourMessage>().To(staticRoutingSubscriberQueueLocation);            
        }
    }
}