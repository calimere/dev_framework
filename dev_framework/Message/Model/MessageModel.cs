using System.Collections.Generic;
using System;
using System.Web;
using dev_framework.Message.Model;

namespace dev_framework.Message.Model
{
    [Serializable]
    public class MessageModel : Message
    {
        private string _message;

        public Exception[] Exceptions { get; set; }

        public string Id { get; set; }
        public string Statut { get; set; }
        public string StatutClass { get; set; }
        public MessageStatut MessageStatut { get; set; }

        public MessageModel()
        {
            Id = "prime-message";
        }

        public MessageModel(string id)
        {
            Id = id;
        }

        public MessageModel(MessageStatut retour)
        {
            MessageStatut = retour;
            Statut = retour.ToString();
            StatutClass = retour.ToDescription();
        }
        public MessageModel(MessageStatut retour, string message)
        {
            MessageStatut = retour;
            Statut = retour.ToString();
            StatutClass = retour.ToDescription();
            var dic = new Dictionary<string, string>();
            dic.Add(Statut, message);
            SetMessage(dic);
        }
        public MessageModel(string message, string statut, string statutClass)
        {
            Statut = statut;
            StatutClass = statutClass;

            var dic = new Dictionary<string, string>();
            dic.Add(Statut, message);
            SetMessage(dic);
        }

        public void SetMessage(Dictionary<string, string> args) { _message = args[Statut]; }
        public string Message { get { return _message; } }
        public string HtmlMessage { get { return HttpUtility.HtmlEncode(_message); } }
        public MessageModel InitMessageModel(DatabaseMessage retour, string libelle)
        {
            MessageModel message = null;
            switch (retour.EnumDataBaseMessage)
            {
                case EnumDataBaseMessage.Success:
                    message = new MessageModel(MessageStatut.Success);
                    message.ReturnValue = retour.ReturnValue;
                    break;
                case EnumDataBaseMessage.Error:
                    message = new MessageModel(MessageStatut.Error);
                    message.Exception = retour.Exception;
                    break;
                case EnumDataBaseMessage.NoChanges:
                    message = new MessageModel(MessageStatut.Info);
                    message.Exception = retour.Exception;
                    message.ReturnValue = retour.ReturnValue;
                    break;
                case EnumDataBaseMessage.Warning:
                    message = new MessageModel(MessageStatut.Warning);
                    message.Exception = retour.Exception;
                    message.ReturnValue = retour.ReturnValue;
                    break;
            }
            message.SetMessage(new Dictionary<string, string> {
                { MessageStatut.Error.ToString(), retour.Exception != null ? retour.Exception.Message : string.Empty },
                { MessageStatut.Success.ToString(), libelle },
                { MessageStatut.Warning.ToString(), retour.Exception != null ? libelle : string.Empty },
                { MessageStatut.Info.ToString(), "Aucun changement n'a été effectué" }
            });
            return message;
        }

    }
}