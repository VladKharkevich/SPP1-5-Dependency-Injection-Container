using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Dependency_Injection_Container
{
    public class DependencyProvider
    {
        DependenciesConfiguration fDependenciesConfiguration;

        public DependencyProvider(DependenciesConfiguration dependenciesConfiguration)
        {
            fDependenciesConfiguration = dependenciesConfiguration;
        }

        public T Resolve<T>()
        {
            foreach (var record in fDependenciesConfiguration.storage)
            {
                if (typeof(T) == record.dependency)
                {
                    return (T)GenerateObject(record.implementation);
                }
            }
            return default(T);
        }

        private object ResolveTypeInParameter(Type type)
        {
            foreach (var record in fDependenciesConfiguration.storage)
            {
                if (type == record.dependency)
                {
                    return GenerateObject(record.implementation);
                }
            }
            return default(object);
        }

        private object GenerateObject(Type implementation)
        {
            int count = 0;
            int pos = 0;
            for (int i = 0; i < implementation.GetConstructors().Length; i++)
            {
                if (implementation.GetConstructors()[i].GetParameters().Length > count)
                {
                    pos = i;
                    count = implementation.GetConstructors()[i].GetParameters().Length;
                }
            }
            var constructor = implementation.GetConstructors()[pos];
            ParameterInfo[] parameterInfos = constructor.GetParameters();
            List<object> param = new List<object>();
            foreach (ParameterInfo pInfo in parameterInfos)
            {
                Type tempType = pInfo.ParameterType;
                param.Add(GenerateObject(tempType));
            }
            return constructor.Invoke(param.ToArray());
        }
    }
}
