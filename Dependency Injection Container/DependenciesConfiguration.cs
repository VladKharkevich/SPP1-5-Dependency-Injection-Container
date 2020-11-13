using System;
using System.Collections.Generic;
using System.Text;

namespace Dependency_Injection_Container
{
    public class DependenciesConfiguration
    {
        public List<StorageRecord> storage { get; }

        public DependenciesConfiguration()
        {
            storage = new List<StorageRecord>();
        }
        public void Register<T1, T2>(bool isSingleton=false)
            where T1 : class
            where T2 : class
        {
            bool isAdd = true;
            foreach (StorageRecord record in storage)
            {
                if (record.dependency == typeof(T1) && record.implementation == typeof(T2))
                    isAdd = false;
            }
            if (isAdd)
                storage.Add(new StorageRecord(typeof(T1), typeof(T2), isSingleton));
        }

        public void Register(Type T1, Type T2, bool isSingleton = false)
        {
            bool isAdd = true;
            foreach (StorageRecord record in storage)
            {
                if (record.dependency == T1 && record.implementation == T2)
                    isAdd = false;
            }
            if (isAdd)
                storage.Add(new StorageRecord(T1, T2, isSingleton));
        }
    }
}
