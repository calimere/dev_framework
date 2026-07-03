using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ui.utils.Manager.Model
{
    public abstract class Message
    {
        public Exception Exception { get; set; }
        public object ReturnValue { get; set; }

        public T GetReturnValue<T>() where T : class
        {
            if (ReturnValue != null)
                return (T)ReturnValue;
            return null;
        }
    }

    public class IOMessage : Message
    {
        public EIOMessage EIoMessage { get; }
        public IOMessage(EIOMessage eIoMessage) { EIoMessage = eIoMessage; }
    }

    public enum EIOMessage
    {
        Success,
        Error,
        Warning
    }
}
