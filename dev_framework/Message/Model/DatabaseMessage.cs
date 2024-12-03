namespace dev_framework.Message.Model
{
    public class DatabaseMessage : Message
    {
        public int Count { get; set; }
        public EnumDataBaseMessage EnumDataBaseMessage { get; }
        public DatabaseMessage(EnumDataBaseMessage enumDataBaseMessage) { EnumDataBaseMessage = enumDataBaseMessage; }
        public DatabaseMessage() { }

        /// <summary>
        /// Constructeur pour les messages de la base de données, récupérant l'objet de retour précédent mais en changeant enumDataBaseMessage et en ajoutant une exception
        /// </summary>
        /// <param name="databaseMessage"></param>
        /// <param name="enumDataBaseMessage"></param>
        /// <param name="ex"></param>
        public DatabaseMessage(DatabaseMessage databaseMessage, EnumDataBaseMessage enumDataBaseMessage, Exception ex)
        {
            ReturnValue = databaseMessage.ReturnValue;
            Count = databaseMessage.Count;

            EnumDataBaseMessage = enumDataBaseMessage;
            Exception = ex;
        }
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
