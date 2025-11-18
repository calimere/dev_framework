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
    public abstract class ANotification : BusinessObject 
    {
        [Key]
        public int ntf_id { get; set; }
        public string ntf_title { get; set; }
        public string ntf_content { get; set; }
        public bool ntf_is_read { get; set; }

        public string? ntf_creator_id { get; set; }

        public ENotificationStatut ntf_statut { get; set; }
        public ENotificationPriority ntf_notification_priority { get; set; }

        public string ntf_data { get; set; }

        public ANotification()
        {
            ntf_is_read = false;
        }
    }

    public class NotificationUser<T,W> : BusinessObject where T : ANotification where W : AUser
    {
        [Key]
        public int ntu_id { get; set; }
        public T Notification { get; set; }
        
        [ForeignKey("Notification")]
        public int ntf_id { get; set; }
        
        public W User { get; set; }
        [ForeignKey("User")]
        public string usr_id { get; set; }

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
