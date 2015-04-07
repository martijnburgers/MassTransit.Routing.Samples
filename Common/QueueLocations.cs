using System.Diagnostics.CodeAnalysis;

namespace MassTransitDemo.Common
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class QueueLocations
    {
        public const string MULTICAST_PUBLISHER_QUEUE = "msmq://localhost/Multicast.Publisher";
        public const string MULTICAST_SUBSCRIBER_QUEUE = "msmq://localhost/Multicast.Subscriber";
        public const string STATIC_ROUTING_PUBLISHER_QUEUE = "msmq://localhost/StaticRouting.Publisher";
        public const string STATIC_ROUTING_SUBSCRIBER_QUEUE = "msmq://localhost/StaticRouting.Subscriber";
        public const string RUNTIME_SERVICES_SUBSCRIPTION_QUEUE = "msmq://localhost/mt_subscriptions";
        public const string RUNTIME_SERVICES_PUBLISHER_QUEUE = "msmq://localhost/RuntimeServices.Publisher";
        public const string RUNTIME_SERVICES_SUBSCRIBER_QUEUE = "msmq://localhost/RuntimeServices.Subscriber";
        public const string ENDPOINT_PUBLISHER_QUEUE = "msmq://localhost/Endpoint.Publisher";
        public const string ENDPOINT_SUBSCRIBER_QUEUE = "msmq://localhost/Endpoint.Subscriber";
    }
}
