using System.ComponentModel;

namespace ui.utils.Manager.Enum
{
    public enum MessageStatut
    {
        [Description("alert-danger")]
        Error,
        [Description("alert-success")]
        Success,
        [Description("alert-info")]
        Info,
        [Description("alert-warning")]
        Warning
    }
}
