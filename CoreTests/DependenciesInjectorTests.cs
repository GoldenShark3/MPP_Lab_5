using System;
using Core.DependenciesInjectorCore;
using NUnit.Framework;

namespace CoreTests
{
    [TestFixture]
    public class DependenciesInjectorTests
    {
        interface A
        {
            int rnd { get; }
        }

        interface B
        {
            int rnd { get; }
            A a { get; }
        }

        interface C<T>
        {
            int rnd { get; }
            T t { get; }
        }

        class CI1 : C<A>
        {
            private int _rnd = new Random().Next();
            public int rnd => _rnd;
            private A _a;
            public A t => _a;
        }
        
        class CI2 : C<A>
        {
            private int _rnd = new Random().Next();
            public int rnd => _rnd;
            private A _a;
            public A t => _a;
        }
        
        class AI : A
        {
            private int _rnd = new Random().Next();
            public int rnd => _rnd;
        }

        class BI : B
        {
            private A _a;
            public A a => _a;
            private int _rnd = new Random().Next();
            public int rnd => _rnd;

            public BI(A a)
            {
                _a = a;
            }
        }

        [Test]
        public void Resolve_DifferentValues_Instance()
        {
            var c = new DependenciesConfiguration();
            c.Register<A, AI>(Scope.Instance);
            var s = new DependencyProvider(c);
            int prev = s.Resolve<A>().rnd;
            Assert.Multiple(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    int curr = s.Resolve<A>().rnd;
                    Assert.AreNotEqual(prev, curr);
                }
            });
        }
        
        [Test]
        public void Resolve_SameValues_Singleton()
        {
            var c = new DependenciesConfiguration();
            c.Register<A, AI>(Scope.Singleton);
            var s = new DependencyProvider(c);
            int prev = s.Resolve<A>().rnd;
            Assert.Multiple(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    int curr = s.Resolve<A>().rnd;
                    Assert.AreEqual(prev, curr);
                }
            });
        }

        [Test]
        public void Resolve_Instance_Instance()
        {
            var c = new DependenciesConfiguration();
            c.Register<A, AI>(Scope.Instance);
            c.Register<B, BI>(Scope.Instance);
            var s = new DependencyProvider(c);
            B prev = s.Resolve<B>();
            Assert.Multiple(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    B curr = s.Resolve<B>();
                    Assert.AreNotEqual(prev.rnd, curr.rnd);
                    Assert.AreNotEqual(prev.a.rnd, curr.a.rnd);
                }
            });
        }
        
        [Test]
        public void Resolve_Instance_Singleton()
        {
            var c = new DependenciesConfiguration();
            c.Register<A, AI>(Scope.Instance);
            c.Register<B, BI>(Scope.Singleton);
            var s = new DependencyProvider(c);
            int preva = s.Resolve<A>().rnd;
            B prev = s.Resolve<B>();
            Assert.Multiple(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    int curra = s.Resolve<A>().rnd;
                    B curr = s.Resolve<B>();
                    Assert.AreNotEqual(preva, curra);
                    Assert.AreEqual(prev.rnd, curr.rnd);
                    Assert.AreEqual(prev.a.rnd, curr.a.rnd);
                }
            });
        }
        
        [Test]
        public void Resolve_Singleton_Instance()
        {
            var c = new DependenciesConfiguration();
            c.Register<A, AI>(Scope.Singleton);
            c.Register<B, BI>(Scope.Instance);
            var s = new DependencyProvider(c);
            int preva = s.Resolve<A>().rnd;
            B prev = s.Resolve<B>();
            Assert.Multiple(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    int curra = s.Resolve<A>().rnd;
                    B curr = s.Resolve<B>();
                    Assert.AreEqual(preva, curra);
                    Assert.AreNotEqual(prev.rnd, curr.rnd);
                    Assert.AreEqual(prev.a.rnd, curr.a.rnd);
                }
            });
        }
        
        [Test]
        public void Resolve_Singleton_Singleton()
        {
            var c = new DependenciesConfiguration();
            c.Register<A, AI>(Scope.Singleton);
            c.Register<B, BI>(Scope.Singleton);
            var s = new DependencyProvider(c);
            int preva = s.Resolve<A>().rnd;
            B prev = s.Resolve<B>();
            Assert.Multiple(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    int curra = s.Resolve<A>().rnd;
                    B curr = s.Resolve<B>();
                    Assert.AreEqual(preva, curra);
                    Assert.AreEqual(prev.rnd, curr.rnd);
                    Assert.AreEqual(prev.a.rnd, curr.a.rnd);
                }
            });
        }

        [Test]
        public void ResolveAll_Count_Correct()
        {
            var c = new DependenciesConfiguration();
            c.Register<C<A>, CI1>(Scope.Instance);
            c.Register<C<A>, CI2>(Scope.Instance);
            var s = new DependencyProvider(c);
            Assert.AreEqual(2, s.ResolveAll<C<A>>().Length);
        }
    }
}