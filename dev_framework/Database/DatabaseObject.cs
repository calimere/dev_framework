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

    public class SupervisedDatabaseObject : DatabaseObject
    {
        public DateTime modified { get; set; }
        public int modified_by { get; set; }
    }

    public abstract class BusinessObject : DatabaseObject
    {
        public bool is_deleted { get; set; }
        public BusinessObject() { is_deleted = false; }
    }
}
