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
        public void Register<T1, T2>()
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
                    storage.Add(new StorageRecord(typeof(T1), typeof(T2)));
        }
    }
}
