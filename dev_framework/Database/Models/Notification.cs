using dev_framework.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace dev_framework.Database.Models
{
    public abstract class Notification : BusinessObject
    {
        [Key]
        public int ntf_id { get; set; }
        public string ntf_title { get; set; }
        public string ntf_content { get; set; }
        public bool ntf_is_read { get; set; }

        [ForeignKey("User")]
        public string usr_id { get; set; }
        public AUser User { get; set; }

        public ENotificationStatut ntf_statut { get; set; }
        public ENotificationPriority ntf_notification_priority { get; set; }

        public string ntf_data { get; set; }

        public Notification()
        {
            ntf_is_read = false;
        }
    }

    public enum ENotificationStatut
    {
        [Description("Information")]
        Info,
        [Description("Attention")]
        Warning,
        [Description("Erreur")]
        Error
    }
    public enum ENotificationPriority
    {
        [Description("Basse")]
        Low,
        [Description("Normale")]
        Medium,
        [Description("Haute")]
        High
    }
}
