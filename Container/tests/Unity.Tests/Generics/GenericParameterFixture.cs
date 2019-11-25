﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.Exceptions;
using Unity.Injection;
using Unity.Tests.v5.TestSupport;

namespace Unity.Tests.v5.Generics
{
    /// <summary>
    /// Tests that use the GenericParameter class to ensure that
    /// generic object injection works.
    /// </summary>
    [TestClass]
    public class GenericParameterFixture
    {
        [TestMethod]
        public void CanCallNonGenericConstructorOnOpenGenericType()
        {
            IUnityContainer container = new UnityContainer()
                .RegisterType(typeof(ClassWithOneGenericParameter<>),
                        new InjectionConstructor("Fiddle", new InjectionParameter<object>("someValue")));

            ClassWithOneGenericParameter<User> result = container.Resolve<ClassWithOneGenericParameter<User>>();

            Assert.IsNull(result.InjectedValue);
        }

        [TestMethod]
        public void CanCallConstructorTakingGenericParameter()
        {
            IUnityContainer container = new UnityContainer()
                .RegisterType(typeof(ClassWithOneGenericParameter<>),
                    new InjectionConstructor(new GenericParameter("T")));

            Account a = new Account();
            container.RegisterInstance<Account>(a);

            ClassWithOneGenericParameter<Account> result = container.Resolve<ClassWithOneGenericParameter<Account>>();
            Assert.AreSame(a, result.InjectedValue);
        }

        [TestMethod]
        public void CanConfiguredNamedResolutionOfGenericParameter()
        {
            IUnityContainer container = new UnityContainer()
                .RegisterType(typeof(ClassWithOneGenericParameter<>),
                    new InjectionConstructor(new GenericParameter("T", "named")));

            Account a = new Account();
            container.RegisterInstance<Account>(a);
            Account named = new Account();
            container.RegisterInstance<Account>("named", named);

            ClassWithOneGenericParameter<Account> result = container.Resolve<ClassWithOneGenericParameter<Account>>();
            Assert.AreSame(named, result.InjectedValue);
        }

        // Our various test objects
        public class ClassWithOneGenericParameter<T>
        {
            public T InjectedValue;

            public ClassWithOneGenericParameter(string s, object o)
            {
            }

            public ClassWithOneGenericParameter(T injectedValue)
            {
                InjectedValue = injectedValue;
            }
        }

        public class GenericTypeWithMultipleGenericTypeParameters<T, U>
        {
            private T theT;
            private U theU;
            public string Value;

            [InjectionConstructor]
            public GenericTypeWithMultipleGenericTypeParameters()
            {
            }

            public GenericTypeWithMultipleGenericTypeParameters(T theT)
            {
                this.theT = theT;
            }

            public GenericTypeWithMultipleGenericTypeParameters(U theU)
            {
                this.theU = theU;
            }

            public void Set(T theT)
            {
                this.theT = theT;
            }

            public void Set(U theU)
            {
                this.theU = theU;
            }

            public void SetAlt(T theT)
            {
                this.theT = theT;
            }

            public void SetAlt(string value)
            {
                this.Value = value;
            }
        }
    }
}
