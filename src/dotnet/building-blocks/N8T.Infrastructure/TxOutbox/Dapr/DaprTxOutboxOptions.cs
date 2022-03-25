namespace N8T.Infrastructure.TxOutbox.Dapr
{
    public class DaprTxOutboxOptions
    {
        public static string Name = "DaprTransactionalOutbox";
        public string StateStoreName { get; set; } = "statestore";
        public string OutboxName { get; set; } = "outbox";
    }
}
