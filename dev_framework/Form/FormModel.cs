using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class FormModel
{
    public string FormModelStatus { get; set; }
    public List<string> Errors { get; set; }
    public List<string> Warnings { get; set; }
    public object ReturnValue { get; set; }
    public ResultStatut ResultStatut { get; set; }

    public FormModel()
    {
        ResultStatut = ResultStatut.success;
        FormModelStatus = ResultStatut.success.ToString();
        Errors = new List<string>();
        Warnings = new List<string>();
    }
}

public enum ResultStatut
{
    success = 1,
    warning = 2,
    error = 3
}