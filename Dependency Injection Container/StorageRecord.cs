using System;
using System.Collections.Generic;
using System.Text;

namespace Dependency_Injection_Container
{
    public class StorageRecord
    {
        public Type dependency { get; }

        public Type implementation { get; }
        
        public bool isSingleton { get;  }

        public StorageRecord(Type dependency, Type implementation, bool isSingleton)
        {
            this.dependency = dependency;
            this.implementation = implementation;
            this.isSingleton = isSingleton;
        }

        public Type this[int index]
        { 
            get
            {
                if (index == 0)
                {
                    return dependency;
                }
                else if (index == 1)
                {
                    return implementation;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

    }
}
