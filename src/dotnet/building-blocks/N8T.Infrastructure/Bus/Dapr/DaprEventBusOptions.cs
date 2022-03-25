namespace N8T.Infrastructure.Bus.Dapr
{
    public class DaprEventBusOptions
    {
        public static string Name = "DaprEventBus";
        public string PubSubName { get; set; } = "pubsub";
    }
}
