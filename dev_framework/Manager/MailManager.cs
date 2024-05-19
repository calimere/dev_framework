using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace dev_framework.Manager
{
    public class MailManager
    {
        private string _env;
        private string _host;
        private int _port;
        private string _destinataire;
        private string _from;
        private string _smtpPassword;
        private string _smtpUser;
        private string _fromDisplay;
        private readonly SerilogManager _serilogManager;

        public MailManager(string env, string host, string from, string smtpPassword, string smtpUser, string destinataire, int port, string fromDisplayName, SerilogManager serilogManager)
        {
            _env = env;
            _host = host;
            _from = from;
            _smtpPassword = smtpPassword;
            _smtpUser = smtpUser;
            _destinataire = destinataire;
            _port = port;
            _fromDisplay = fromDisplayName;

            _serilogManager = serilogManager;

        }

        /// <summary>
        /// Méthode privée d'envoi d'email
        /// </summary>
        /// <param name="mail"></param>
        private bool SendMailInterne(MailMessage mail, bool ssl = true)
        {
            try
            {
                mail.From = new MailAddress(_from, _fromDisplay);
                var smtp = new SmtpClient { Host = _host, Port = _port, UseDefaultCredentials = false, Credentials = new NetworkCredential(_smtpUser, _smtpPassword), EnableSsl = ssl };
                if (string.IsNullOrEmpty(_env) || _env != "PROD")
                {
                    mail.To.Clear();
                    mail.To.Add(new MailAddress(_destinataire));
                }
                smtp.Send(mail);
            }
            catch (Exception ex) { throw new Exception("Le mail n'a pas pu partir", ex); }
            return true;
        }

        /// <summary>
        /// Méthode d'envoi d'email
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        /// <exception cref="MailNotSendMitException"></exception>
        public bool SendMail(MailMessage mail)
        {
            var startTime = _serilogManager.Debut(SerilogManager.GetCurrentMethod(), mail);
            SendMailInterne(mail);
            _serilogManager.Fin(SerilogManager.GetCurrentMethod(), mail, startTime);
            return true;
        }
        [Obsolete("Utiliser la méthode SendMail(MailMessage mail)", true)]
        public bool SendMail(List<string> tos, string subject, string body, bool ssl = true, string from = "")
        {
            return true;
        }
        public string GetMailContent(bool isLocal, string path)
        {
            StreamReader sr = new StreamReader(path);
            var content = string.Empty;
            return sr.ReadToEnd();
        }
        public string GetMailContent<T>(bool isLocal, string path, T args)
        {
            var content = GetMailContent(isLocal, path);
            content = content.Replace(args);
            return content;
        }
    }
}
