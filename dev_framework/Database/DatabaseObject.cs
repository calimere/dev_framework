using System;

namespace dev_framework.Database
{
    public abstract class DatabaseObject
    {
        public DateTime created { get; set; }

        public DatabaseObject()
        {
            created = DateTime.Now;
        }
    }

    public class SupervisedDatabaseObject : BusinessObject
    {
        public string created_by { get; set; } // si guid empty => création système
        public DateTime modified { get; set; }
        public string modified_by { get; set; }

        public SupervisedDatabaseObject()
        {
            modified = DateTime.Now;
            created_by = Guid.Empty.ToString();
        }
    }

    public class OnlyInsertDatabaseObject : DatabaseObject
    {
        public string created_by { get; set; } // si guid empty => création système
        public OnlyInsertDatabaseObject()
        {
            created_by = Guid.Empty.ToString();
        }
    }

    public abstract class BusinessObject : DatabaseObject
    {
        public bool is_deleted { get; set; }
        public BusinessObject()
        {
            is_deleted = false;
        }
    }
}
