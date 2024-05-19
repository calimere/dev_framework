using System.ComponentModel;

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
