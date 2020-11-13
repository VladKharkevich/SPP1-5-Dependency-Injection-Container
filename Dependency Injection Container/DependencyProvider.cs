using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Dependency_Injection_Container
{
    public class DependencyProvider
    {
        DependenciesConfiguration fDependenciesConfiguration;
        Dictionary<StorageRecord, object> singletonCache;

        public DependencyProvider(DependenciesConfiguration dependenciesConfiguration)
        {

            fDependenciesConfiguration = Validate(dependenciesConfiguration);
            singletonCache = new Dictionary<StorageRecord, object>();
        }

        public T Resolve<T>()
        {
            foreach (var record in fDependenciesConfiguration.storage)
            {
                if (typeof(T) == record.dependency)
                {
                    if (record.isSingleton)
                        return (T)GenerateSingletonObject(record);
                    else
                        return (T)GenerateObject(record.implementation);
                }
                else if (record.dependency.IsGenericTypeDefinition)
                {
                    if (typeof(T).GetGenericTypeDefinition() == record.dependency)
                    {
                        if (record.isSingleton)
                            return (T)GenerateSingletonObject(record);
                        else
                            return (T)GenerateObject(record.implementation.MakeGenericType(typeof(T).GetGenericArguments()[0]));
                    }
                }
            }
            return default(T);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            List<T> container = new List<T>();
            foreach (var record in fDependenciesConfiguration.storage)
            {
                if (typeof(T) == record.dependency)
                {
                    if (record.isSingleton)
                        container.Add((T)GenerateSingletonObject(record));
                    else
                        container.Add((T)GenerateObject(record.implementation));
                }
                else if (record.dependency.IsGenericTypeDefinition)
                {
                    if (typeof(T).GetGenericTypeDefinition() == record.dependency)
                    {
                        if (record.isSingleton)
                            container.Add((T)GenerateSingletonObject(record));
                        else
                            container.Add((T)GenerateObject(typeof(T)));
                    }
                }
            }
            return container;
        }

        private DependenciesConfiguration Validate(DependenciesConfiguration dependenciesConfiguration)
        {
            var newDependenciesConfiguration = new DependenciesConfiguration();
            foreach (var dependency in dependenciesConfiguration.storage)
            {
                if (dependency.dependency == dependency.implementation || 
                    dependency.implementation.GetInterfaces().Contains(dependency.dependency) ||
                    dependency.implementation.IsSubclassOf(dependency.dependency)||
                    dependency.implementation.IsGenericTypeDefinition)
                {
                    newDependenciesConfiguration.Register(dependency.dependency, dependency.implementation, dependency.isSingleton);
                }
                
            }
            return newDependenciesConfiguration;
        }

        private object GenerateSingletonObject(StorageRecord record)
        {
            foreach (var singRecord in singletonCache)
            {
                if (singRecord.Key.implementation == record.implementation && singRecord.Key.dependency == record.dependency)
                {
                    return singRecord.Value;
                }
            }
            object result = null;
            lock (singletonCache)
            {
                foreach (var singRecord in singletonCache)
                {
                    if (singRecord.Key.implementation == record.implementation && singRecord.Key.dependency == record.dependency)
                    {
                        result = singRecord.Value;
                    }
                }
                if (result != null)
                {
                    result = GenerateObject(record.implementation);
                    singletonCache.Add(record, result);
                }
            }
            return result;
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
                param.Add(ResolveTypeInParameter(tempType));
            }
            return constructor.Invoke(param.ToArray());
        }
    }
}
 