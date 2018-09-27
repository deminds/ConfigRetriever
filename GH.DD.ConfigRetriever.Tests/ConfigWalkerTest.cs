using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using FluentAssertions;
using GH.DD.ConfigRetriever.Attributes;
using NUnit.Framework;

namespace GH.DD.ConfigRetriever.Tests
{
    public class ConfigWalkerTest
    {
        [Test]
        public void Walk_WithoutAttr()
        {
        }
        
        [Test]
        public void Walk_WithClassAttr()
        {
        }
        
        [Test]
        public void Walk_WithClassAndPropAttr()
        {
        }
        
        [Test]
        public void Walk_WithPropAttr()
        {
        }
        
        [Test]
        public void Walk_WithClassNameAndPropAttr()
        {
        }

        [Test]
        public void Walk_WithClassPathAndPropAttr()
        {
        }

        [Test]
        public void Walk_ClassWithoutInterface()
        {
            Assert.Throws<TypeAccessException>(() =>
            {
                var configWalker = new ConfigWalker<TestClass_WithoutInterface>();
            });
        }
        
        [Test]
        public void Walk_NestedClassWithoutInterface()
        {
            // TargetInvocationException instead TypeAccessException because TypeAccessException is inner exception
            Assert.Throws<TargetInvocationException>(() =>
            {
                var configWalker = new ConfigWalker<TestClass_NestedClassWithoutInterface>();
                var result = configWalker.Walk().ToList();
            });
        }

        [Test]
        public void Walk_ClassWithoutProps()
        {
            var walker = new ConfigWalker<TestClass_WithoutProps>();
            
            var result = walker.Walk().ToList();
            
            var expected = new List<ConfigElement>();

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Walk_SkipWrongPropsTypes()
        {
            var walker = new ConfigWalker<TestClass_WithWrongPropsTypes>();
            
            var result = walker.Walk().ToList();
            
            var expected = new List<ConfigElement>();

            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Walk_GenericClass()
        {
            Assert.Throws(typeof(TypeAccessException), () =>
            {
                var configWalker = new ConfigWalker<TestClass_Generic<string>>();
                foreach (var configElement in configWalker.Walk())
                {
                    Console.WriteLine(configElement);
                }
            });
        }
        
        [Test]
        public void Walk_SkipPrivateProps()
        {
            var walker = new ConfigWalker<TestClassWithPrivateProps>();
            
            var result = walker.Walk().ToList();
            
            var expected = new List<ConfigElement>();

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Walk_ComplexAttrTest()
        {
            var walker = new ConfigWalker<TestClass1>();
            
            var result = walker.Walk().ToList();
            
            var expected = new List<ConfigElement>
            {
                new ConfigElement(
                    new List<List<string>>
                    {
                        new List<string>{ "FakePathLevel0", "FakePathLevel1", "TestClass1", "PropString" },
                    }, 
                    new List<string>{ "TestClass1", "PropString" },
                    typeof(string)),
                
                new ConfigElement(
                    new List<List<string>>
                    {
                        new List<string>{ "FakePathLevel0", "FakePathLevel1", "TestClass1", "PropBool" },
                    }, 
                    new List<string>{ "TestClass1", "PropBool" },
                    typeof(bool)),
                
                new ConfigElement(
                    new List<List<string>>
                    {
                        new List<string>{ "FakePathLevel0", "FakePathLevel1", "TestClass1", "TestClass1_1Name", "PropBool" },
                    }, 
                    new List<string>{ "TestClass1", "PropTestClass1_1", "PropBool" },
                    typeof(bool)),
                
                new ConfigElement(
                    new List<List<string>>
                    {
                        new List<string>{ "FakePathLevel0", "FakePathLevel1", "TestClass1", "TestClass1_1Name", "PropInt" },
                    }, 
                    new List<string>{ "TestClass1", "PropTestClass1_1", "PropInt" },
                    typeof(int)),
                
                new ConfigElement(
                    new List<List<string>>
                    {
                        new List<string>{ "FakePathLevel0", "TestClass2", "PropLong" },
                        new List<string>{ "FakePathLevel0", "FakePathLevel1", "TestClass2", "PropInt" },
                    }, 
                    new List<string>{ "TestClass1", "PropTestClass2", "PropInt" },
                    typeof(int)),
                
                new ConfigElement(
                    new List<List<string>>
                    {
                        new List<string>{ "FakePathLevel0", "TestClass2", "PropLong" },
                        new List<string>{ "FakePathLevel0", "FakePathLevel1", "TestClass2", "PropLong" },
                    }, 
                    new List<string>{ "TestClass1", "PropTestClass2", "PropLong" },
                    typeof(long)),
                
                new ConfigElement(
                    new List<List<string>>
                    {
                        new List<string>{ "FakePathLevel0", "TestClass3", "PropLong" },
                        new List<string>{ "FakePathLevel0", "TestClass2", "TestClass3", "PropLong" },
                        new List<string>{ "FakePathLevel0", "FakePathLevel1", "TestClass2", "TestClass3", "PropLong" },
                    }, 
                    new List<string>{ "TestClass1", "PropTestClass2", "PropTestClass3", "PropLong" },
                    typeof(long)),
                
                new ConfigElement(
                    new List<List<string>>
                    {
                        new List<string>{ "FakePathLevel0", "TestClass3", "PropDouble" },
                        new List<string>{ "FakePathLevel0", "TestClass2", "TestClass3", "PropDouble" },
                        new List<string>{ "FakePathLevel0", "FakePathLevel1", "TestClass2", "TestClass3", "PropDouble" },
                    }, 
                    new List<string>{ "TestClass1", "PropTestClass2", "PropTestClass3", "PropDouble" },
                    typeof(double)),
            };

            result.Should().BeEquivalentTo(expected);
        }
        
        private class TestClass_WithoutInterface
        {
            public string PropString { set; get; }
        }
        
        private class TestClass_WithoutProps : IConfigObject
        {
        }

        private class TestClass_NestedClassWithoutInterface : IConfigObject
        {
            public TestClass_WithoutInterface testProp { set; get; }
        }
        
        private class TestClass_WithWrongPropsTypes : IConfigObject
        {
            public ITestClass PropITestClass { set; get; }
            public TestClass_Generic<string> PropTestClassGeneric { set; get; }
        }
        
        private interface ITestClass : IConfigObject
        {
            string PropString { set; get; }
        }
        
        private class TestClass_Generic<T> : IConfigObject
        {
            public string PropString { set; get; }
        }

        private class TestClassWithPrivateProps : IConfigObject
        {
            private string PropStringPrivate { set; get; }
            public string PropStringPrivateSet { private set; get; }
            public string PropStringPrivateGet { set; private get; }
            public static string PropStringStatic { set; get; }
        }
        
        [ConfigRetrieverPath("FakePathLevel0", "FakePathLevel1")]
        private class TestClass1 : IConfigObject
        {
            public string PropString { set; get; }
            public bool PropBool { set; get; }
            
            [ConfigRetrieverElementName("TestClass1_1Name")]
            public TestClass1_1 PropTestClass1_1 { set; get; }
            
            [ConfigRetrieverPath("FakePathLevel0", "FakePathLevel1")]
            [ConfigRetrieverFailbackPath("FakePathLevel0")]
            public TestClass2 PropTestClass2 { set; get; }
        }

        // attribute must ignore
        [ConfigRetrieverPath("FakePathLevel0", "FakePathLevel1")]
        private class TestClass1_1 : IConfigObject
        {
            public bool PropBool { set; get; }
            public int PropInt { set; get; }
        }

        // attribute must ignore
        [ConfigRetrieverElementName("FakeName")]
        private class TestClass2 : IConfigObject
        {
            public int PropInt { set; get; }
            public long PropLong { set; get; }
            
            [ConfigRetrieverFailbackPath("FakePathLevel0")]
            [ConfigRetrieverElementName("TestClass3")]
            public TestClass3 PropTestClass3 { set; get; }
        }

        private class TestClass3 : IConfigObject
        {
            public long PropLong { set; get; }
            public double PropDouble { set; get; }
        }
    }
}