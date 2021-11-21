using System;
using System.Collections.Generic;

namespace Core.DependenciesInjectorCore
{
    public class DependenciesConfiguration
    {
        internal Dictionary<Type, List<Dependency>> Dependencies { get; }

        public DependenciesConfiguration()
        {
            Dependencies = new Dictionary<Type, List<Dependency>>();
        }

        public void Register<TInterface, TImplementation>(Scope scope) where TInterface : class
            where TImplementation : class, TInterface
        {
            var inter = typeof(TInterface);
            var impl = typeof(TImplementation);
            if (impl.IsAbstract)
            {
                throw new ArgumentException($"Abstract {impl.Name} classes are not supported");
            }

            if (Dependencies.ContainsKey(inter))
            {
                Dependencies[inter].Add(new Dependency(impl, scope, this));
            }
            else
            {
                Dependencies.Add(inter, new List<Dependency> { new(impl, scope, this) });
            }
        }
    }
}