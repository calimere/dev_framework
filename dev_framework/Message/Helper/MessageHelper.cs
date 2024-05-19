using dev_framework.Message.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev_framework.Message.helper
{
    public class MessageHelper : Singleton<MessageHelper>
    {
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
        public MessageModel InitMessageModel(string[] success, string[] error, string messageIntroduction, Exception[] e, object returnValue)
        {
            var message = error.Any()
                ? success.Any()
                    ? new MessageModel(MessageStatut.Warning)
                    : new MessageModel(MessageStatut.Error)
                : new MessageModel(MessageStatut.Success);

            message.Exceptions = e;
            message.ReturnValue = returnValue;
            message.SetMessage(new Dictionary<string, string> { { message.MessageStatut.ToString(), GetMessageModel(messageIntroduction, success, error) } });
            return message;
        }
        private string GetMessageModel(string messageIntroduction, string[] success, string[] error)
        {
            var successStr = string.Join("</li><li>", success);
            var errorStr = string.Join("</li><li>", error);

            var successUl = !string.IsNullOrEmpty(successStr)
                ? string.Format("<br/><br/><strong>Les actions suivantes se sont déroulées correctement : </strong><ul><li>{0}</li></ul>", string.Join("</li><li>", successStr))
                : string.Empty;

            var errorUl = !string.IsNullOrEmpty(errorStr)
                ? string.Format("{1}<strong>Les actions suivantes ont rencontré une erreur : </strong><ul><li>{0}</li></ul><br/><br/>", string.Join("</li><li>", errorStr), !string.IsNullOrEmpty(successUl) ? "<br/><br/>" : "")
                : string.Empty;

            return string.Format("{0}{1}{2}", messageIntroduction, successUl, errorUl);
        }
        public T GetReturnObject<T>(DatabaseMessage message)
        {
            return (T)message.ReturnValue;
        }
    }
}
