using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev_framework
{

    public abstract class Singleton<T> : IDisposable where T : Singleton<T>, new()
    {
        private static T _instance;
        private static object _objLock = new object();

        public static T Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (_objLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new T();
                        }
                    }
                }
                return _instance;
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}