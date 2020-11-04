using System;
using System.Collections.Generic;
using System.Text;

namespace Dependency_Injection_Container
{
    public class StorageRecord
    {
        public Type dependency { get; }
        public Type implementation { get; }
        public StorageRecord(Type dependency, Type implementation)
        {
            this.dependency = dependency;
            this.implementation = implementation;
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
