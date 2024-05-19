namespace dev_framework.Message.Model
{
    public class DatabaseMessage : Message
    {
        public int Count { get; set; }
        public EnumDataBaseMessage EnumDataBaseMessage { get; }
        public DatabaseMessage(EnumDataBaseMessage enumDataBaseMessage) { EnumDataBaseMessage = enumDataBaseMessage; }
    }

    public class IOMessage : Message
    {
        public EIOMessage EIoMessage { get; }
        public IOMessage(EIOMessage eIoMessage) { EIoMessage = eIoMessage; }
    }

    public enum EnumDataBaseMessage
    {
        Success,
        Error,
        NoChanges,
        Warning
    }

    public enum EIOMessage
    {
        Success,
        Error,
        Warning
    }
}
