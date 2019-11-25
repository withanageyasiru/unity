using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Unity.Exceptions;
using Unity.Tests.v5.TestSupport;

namespace Unity.Tests.v5.Container
{
    [TestClass]
    public class ContainerBuildUpFixture
    {
        #region BuildUp method with null input or empty string

        [TestMethod]
        public void BuildNullObject1()
        {
            try
            {
                UnityContainer uc = new UnityContainer();
                uc.BuildUp((object) null);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }
        }

        [TestMethod]
        public void BuildNullObject2()
        {
            UnityContainer uc = new UnityContainer();
            object myNullObject = null;

            AssertHelper.ThrowsException<ArgumentNullException>(() => uc.BuildUp(myNullObject, "myNullObject"), "Null object is not allowed");
        }

        [TestMethod]
        public void BuildNullObject3()
        {
            UnityContainer uc = new UnityContainer();
            object myNullObject = null;

            AssertHelper.ThrowsException<ArgumentNullException>(() => uc.BuildUp(null, myNullObject), "Null object is not allowed");
        }

        [TestMethod]
        public void BuildNullObject4()
        {
            IUnityContainer uc = new UnityContainer();
            object myNullObject = null;

            AssertHelper.ThrowsException<ArgumentNullException>(() => uc.BuildUp(null, myNullObject, "myNullObject"), "Null object is not allowed");
        }

        [TestMethod]
        public void BuildNullObject5()
        {
            UnityContainer uc = new UnityContainer();
            SimpleClass myObject = new SimpleClass();
            uc.BuildUp(myObject, (string)null);

            Assert.AreNotEqual(uc.Resolve<SimpleClass>(), myObject);
        }

        [TestMethod]
        public void BuildNullObject6()
        {
            UnityContainer uc = new UnityContainer();
            SimpleClass myObject = new SimpleClass();
            uc.BuildUp(myObject, String.Empty);

            Assert.AreNotEqual(uc.Resolve<SimpleClass>(), myObject);
        }

        [TestMethod]
        public void BuildNullObject7()
        {
            UnityContainer uc = new UnityContainer();
            SimpleClass myObject1 = new SimpleClass();
            SimpleClass myObject2 = new SimpleClass();
            uc.BuildUp(myObject1, String.Empty);
            uc.BuildUp(myObject2, (string)null);

            Assert.AreNotEqual(uc.Resolve<SimpleClass>(), myObject2);
        }

        [TestMethod]
        public void BuildNullObject8()
        {
            UnityContainer uc = new UnityContainer();
            SimpleClass myObject1 = new SimpleClass();
            SimpleClass myObject2 = new SimpleClass();
            uc.BuildUp(myObject1, String.Empty);
            uc.BuildUp(myObject2, "     ");

            Assert.AreNotEqual(uc.Resolve<SimpleClass>(), myObject2);
        }

        [TestMethod]
        public void BuildNullObject9()
        {
            IUnityContainer uc = new UnityContainer();
            SimpleClass myObject1 = new SimpleClass();
            SimpleClass myObject2 = new SimpleClass();
            uc.BuildUp(myObject1, "a");
            uc.BuildUp(myObject2, "   a  ");

            Assert.AreNotEqual(uc.Resolve(typeof(SimpleClass), "a"), myObject2);
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void BuildUpPrimitiveAndDotNetClassTest()
        {
            IUnityContainer uc = new UnityContainer();
            int i = 0;
            uc.BuildUp(i, "a");

          var res = uc.Resolve(typeof(int), "a");
        }

        [TestMethod]
        public void BuildNullObject10()
        {
            IUnityContainer uc = new UnityContainer();
            SimpleClass myObject2 = new SimpleClass();

            uc.BuildUp(myObject2, "  �� ");

            Assert.AreNotEqual(
                uc.Resolve(typeof(SimpleClass), "��"), myObject2);
        }

        [TestMethod]
        public void BuildNullObject11()
        {
            IUnityContainer uc = new UnityContainer();
            SimpleClass myObject1 = new SimpleClass();
            uc.BuildUp(myObject1, " a b c ");

            Assert.AreNotEqual(uc.Resolve(typeof(SimpleClass), "a b c"), myObject1);
        }

        public class SimpleClass
        {
            private object myFirstObj;
            public static int Count = 0;

            [Dependency]
            public object MyFirstObj
            {
                get { return myFirstObj; }
                set { myFirstObj = value; }
            }
        }

        #endregion

        #region BuildUp Method with mismatched type and object

        [TestMethod]
        public void BuildUnmatchedObject1()
        {
            UnityContainer uc = new UnityContainer();

            BuildUnmatchedObject1_TestClass obj1 = new BuildUnmatchedObject1_TestClass();
            uc.BuildUp(typeof(object), obj1);

            Assert.IsNull(obj1.MyFirstObj);
        }

        public class BuildUnmatchedObject1_TestClass
        {
            private object myFirstObj;

            [Dependency]
            public object MyFirstObj
            {
                get { return myFirstObj; }
                set { myFirstObj = value; }
            }
        }

        #endregion

        #region BuildUp method with Base and Child

        [TestMethod]
        public void BuildBaseAndChildObject1()
        {
            UnityContainer uc = new UnityContainer();
            ChildStub1 objChild = new ChildStub1();

            Assert.IsNotNull(objChild);
            Assert.IsNull(objChild.BaseProp);
            Assert.IsNull(objChild.ChildProp);

            uc.BuildUp(typeof(BaseStub1), objChild);

            Assert.IsNotNull(objChild.BaseProp);
            Assert.IsNull(objChild.ChildProp); //the base does not know about child, so it will not build the child property

            uc.BuildUp(typeof(ChildStub1), objChild);

            Assert.IsNotNull(objChild.BaseProp);
            Assert.IsNotNull(objChild.ChildProp); //ChildProp get created

            uc.BuildUp(typeof(BaseStub1), objChild);

            Assert.IsNotNull(objChild.BaseProp);
            Assert.IsNotNull(objChild.ChildProp); //ChildProp is not touched, so it is still NotNull
        }

        [TestMethod]
        public void BuildBaseAndChildObject2()
        {
            UnityContainer uc = new UnityContainer();
            ChildStub1 objChild = new ChildStub1();

            Assert.IsNotNull(objChild);
            Assert.IsNull(objChild.BaseProp);
            Assert.IsNull(objChild.ChildProp);

            uc.BuildUp(typeof(ChildStub1), objChild);

            Assert.IsNotNull(objChild.BaseProp);
            Assert.IsNotNull(objChild.ChildProp); //ChildProp get created
        }

        public interface Interface1
        {
            [Dependency]
            object InterfaceProp
            {
                get;
                set;
            }
        }

        public class BaseStub1 : Interface1
        {
            private object baseProp;
            private object interfaceProp;

            [Dependency]
            public object BaseProp
            {
                get { return this.baseProp; }
                set { this.baseProp = value; }
            }

            public object InterfaceProp
            {
                get { return this.interfaceProp; }
                set { this.interfaceProp = value; }
            }
        }

        public class ChildStub1 : BaseStub1
        {
            private object childProp;

            [Dependency]
            public object ChildProp
            {
                get { return this.childProp; }
                set { this.childProp = value; }
            }
        }

        #endregion

        #region BuildUp method with Abstract Base

        [TestMethod]
        public void BuildAbstractBaseAndChildObject2()
        {
            UnityContainer uc = new UnityContainer();
            ConcreteChild objChild = new ConcreteChild();

            Assert.IsNotNull(objChild);
            Assert.IsNull(objChild.AbsBaseProp);
            Assert.IsNull(objChild.ChildProp);

            uc.BuildUp(typeof(ConcreteChild), objChild);

            Assert.IsNotNull(objChild.AbsBaseProp);
            Assert.IsNotNull(objChild.ChildProp); //ChildProp get created
        }

        public abstract class AbstractBase
        {
            private object baseProp;

            [Dependency]
            public object AbsBaseProp
            {
                get { return baseProp; }
                set { baseProp = value; }
            }

            public abstract void AbstractMethod();
        }

        public class ConcreteChild : AbstractBase
        {
            public override void AbstractMethod()
            {
            }

            [Dependency]
            public object ChildProp { get; set; }
        }

        #endregion

        #region BuildUp method with Contained Object

        [TestMethod]
        public void BuildContainedObject1()
        {
            UnityContainer uc = new UnityContainer();
            MainClass objMain = new MainClass();

            Assert.IsNotNull(objMain);
            Assert.IsNull(objMain.ContainedObj);

            uc.BuildUp(objMain);

            Assert.IsNotNull(objMain.ContainedObj);
            Assert.IsNotNull(objMain.ContainedObj.DependencyProp1);
            Assert.IsNull(objMain.ContainedObj.RegularProp1);
        }

        public class MainClass
        {
            private ContainnedClass containedObj;

            [Dependency]
            public ContainnedClass ContainedObj
            {
                get { return containedObj; }
                set { containedObj = value; }
            }
        }

        public class ContainnedClass
        {
            private object dependencyProp1;
            private object regularProp1;

            [Dependency]
            public object DependencyProp1
            {
                get { return dependencyProp1; }
                set { dependencyProp1 = value; }
            }

            public object RegularProp1
            {
                get { return regularProp1; }
                set { regularProp1 = value; }
            }
        }

        #endregion

        #region BuildUp Calling BuildUp method on itself
        private class ObjectUsingLogger
        {
            [InjectionMethod]
            public void BuildUpTest()
            {
                IUnityContainer container = new UnityContainer();
                container.BuildUp(this);
            }
        }
        #endregion

        #region GetOrDefault method

        [TestMethod]
        public void GetObject1()
        {
            UnityContainer uc = new UnityContainer();
            object obj = uc.Resolve<object>();

            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void GetObject2()
        {
            UnityContainer uc = new UnityContainer();
            object obj = uc.Resolve(typeof(object));

            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void GetObject3()
        {
            UnityContainer uc = new UnityContainer();
            GetTestClass1 obj = uc.Resolve<GetTestClass1>();

            Assert.IsNotNull(obj.BaseProp);
        }

        [TestMethod]
        public void GetObject4()
        {
            UnityContainer uc = new UnityContainer();
            GetTestClass1 obj = (GetTestClass1)uc.Resolve(typeof(GetTestClass1));

            Assert.IsNotNull(obj.BaseProp);
        }

        [TestMethod]
        public void GetObject5()
        {
            UnityContainer uc = new UnityContainer();
            GetTestClass1 obj = uc.Resolve<GetTestClass1>("hello");

            Assert.IsNotNull(obj.BaseProp);
        }

        [TestMethod]
        public void GetObject6()
        {
            UnityContainer uc = new UnityContainer();
            GetTestClass1 objA = uc.Resolve<GetTestClass1>("helloA");

            Assert.IsNotNull(objA.BaseProp);

            GetTestClass1 objB = uc.Resolve<GetTestClass1>("helloB");

            Assert.IsNotNull(objB.BaseProp);
            Assert.AreNotSame(objA, objB);
        }

        public class GetTestClass1
        {
            private object baseProp;

            [Dependency]
            public object BaseProp
            {
                get { return baseProp; }
                set { baseProp = value; }
            }
        }

        #endregion
    }
}