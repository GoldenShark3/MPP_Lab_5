using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.DependenciesInjectorCore
{
    public class DependencyProvider
    {
        private readonly DependenciesConfiguration configuration;

        public DependencyProvider(DependenciesConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public TInterface Resolve<TInterface>() where TInterface : class
        {
            var inter = typeof(TInterface);
            if (!configuration.Dependencies.ContainsKey(inter))
            {
                throw new InvalidOperationException("No such type is registered");
            }

            return (TInterface)configuration.Dependencies[inter][0].Instantiate();
        }


        public TInterface[] ResolveAll<TInterface>() where TInterface : class
        {
            var objects = new List<object>();
            var inter = typeof(TInterface);
            if (!configuration.Dependencies.ContainsKey(inter))
                throw new InvalidOperationException("No such type is registered");

            return configuration.Dependencies[inter].Select(d => d.Instantiate()).Cast<TInterface>().ToArray();
        }
    }
}