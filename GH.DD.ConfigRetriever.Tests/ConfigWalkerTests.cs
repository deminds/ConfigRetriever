using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GH.DD.ConfigRetriever.Attributes;
using NUnit.Framework;

namespace GH.DD.ConfigRetriever.Tests
{
    public class ConfigWalkerTests
    {
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
            Assert.Throws<TypeAccessException>(() =>
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
        public void Walk_InterfaceProp()
        {
            var walker = new ConfigWalker<TestClass_InterfaceProp>();
            
            Assert.Throws<TypeAccessException>(() =>
            {
                var result = walker.Walk().ToList();
            });
        }
        
        [Test]
        public void Walk_GenericProp()
        {
            var walker = new ConfigWalker<TestClass_GenericProp>();
            
            Assert.Throws<TypeAccessException>(() =>
            {
                var result = walker.Walk().ToList();
            });
        }
        
        [Test]
        public void Walk_GenericClass()
        {
            Assert.Throws(typeof(TypeAccessException), () =>
            {
                var configWalker = new ConfigWalker<TestClass_Generic<string>>();
            });
        }
        
        [Test]
        public void Walk_PrivateAndStaticProps()
        {
            var walker = new ConfigWalker<TestClass_PrivateAndStaticProps>();
            
            var result = walker.Walk().ToList();
            
            var expected = new List<ConfigElement>();

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Walk_WithoutAttr()
        {
            var walker = new ConfigWalker<TestClass_WithoutAttr>();
            
            var result = walker.Walk().ToList();
            
            var expected = new List<ConfigElement>
            {
                new ConfigElement(
                    paths: new List<List<string>>
                    {
                        new List<string>
                        {
                            "TestClass_WithoutAttr",
                            "PropString",
                        },
                    },
                    pathInConfigObject: new List<string>
                    {
                        "TestClass_WithoutAttr",
                        "PropString",
                    },
                    elementType: typeof(string)
                 ),
            };

            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Walk_ClassWithPathAttr()
        {
            var walker = new ConfigWalker<TestClass_ClassWithPathAttr>();
            
            var result = walker.Walk().ToList();
            
            var expected = new List<ConfigElement>
            {
                new ConfigElement(
                    paths: new List<List<string>>
                    {
                        new List<string>
                        {
                            "FakePathLevel0",
                            "FakePathLevel1",
                            "TestClass_ClassWithPathAttr",
                            "PropBool",
                        },
                    },
                    pathInConfigObject: new List<string>
                    {
                        "TestClass_ClassWithPathAttr",
                        "PropBool",
                    },
                    elementType: typeof(bool)
                 ),
            };

            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Walk_ClassWithNameAndPathAttr()
        {
            var walker = new ConfigWalker<TestClass_ClassWithNameAndPathAttr>();
            
            var result = walker.Walk().ToList();
            
            var expected = new List<ConfigElement>
            {
                new ConfigElement(
                    paths: new List<List<string>>
                    {
                        new List<string>
                        {
                            "FakePathLevel0",
                            "FakePathLevel1",
                            "TestClassFakeName",
                            "PropInt",
                        },
                    },
                    pathInConfigObject: new List<string>
                    {
                        "TestClass_ClassWithNameAndPathAttr",
                        "PropInt",
                    },
                    elementType: typeof(int)
                 ),
            };

            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Walk_PropIgnoreAttr()
        {
            var walker = new ConfigWalker<TestClass_PropIgnoreAttr>();
            
            var result = walker.Walk().ToList();
            
            var expected = new List<ConfigElement>();

            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Walk_PropFailbackPathAttr()
        {
            var walker = new ConfigWalker<TestClass_PropFailbackPathAttr>();
            
            var result = walker.Walk().ToList();
            
            var expected = new List<ConfigElement>
            {
                new ConfigElement(
                    paths: new List<List<string>>
                    {
                        new List<string>
                        {
                            "FakeFailbackPathLevel0",
                            "FakeFailbackPathLevel1",
                            "PropLong",
                        },
                        new List<string>
                        {
                            "TestClass_PropFailbackPathAttr",
                            "PropLong",
                        },
                    },
                    pathInConfigObject: new List<string>
                    {
                        "TestClass_PropFailbackPathAttr",
                        "PropLong",
                    },
                    elementType: typeof(long)
                 ),
            };

            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Walk_PropPathAndFailbackPathAttr()
        {
            var walker = new ConfigWalker<TestClass_PropPathAndFailbackPathAttr>();
            
            var result = walker.Walk().ToList();
            
            var expected = new List<ConfigElement>
            {
                new ConfigElement(
                    paths: new List<List<string>>
                    {
                        new List<string>
                        {
                            "FakeFailbackPathLevel0",
                            "FakeFailbackPathLevel1",
                            "PropLong",
                        },
                        new List<string>
                        {
                            "FakePathLevel0",
                            "FakePathLevel1",
                            "PropLong",
                        },
                    },
                    pathInConfigObject: new List<string>
                    {
                        "TestClass_PropPathAndFailbackPathAttr",
                        "PropLong",
                    },
                    elementType: typeof(long)
                 ),
            };

            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Walk_PropNameAndPathAndFailbackPathAttr()
        {
            var walker = new ConfigWalker<TestClass_PropNameAndPathAndFailbackPathAttr>();
            
            var result = walker.Walk().ToList();
            
            var expected = new List<ConfigElement>
            {
                new ConfigElement(
                    paths: new List<List<string>>
                    {
                        new List<string>
                        {
                            "FakeFailbackPathLevel0",
                            "FakeFailbackPathLevel1",
                            "PropFakeName",
                        },
                        new List<string>
                        {
                            "FakePathLevel0",
                            "FakePathLevel1",
                            "PropFakeName",
                        },
                    },
                    pathInConfigObject: new List<string>
                    {
                        "TestClass_PropNameAndPathAndFailbackPathAttr",
                        "PropDouble",
                    },
                    elementType: typeof(double)
                 ),
            };

            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Walk_ClassWithNameAndPathAttr_PropNameAndPathAndFailbackPathAttr()
        {
            var walker = new ConfigWalker<TestClass_ClassWithNameAndPathAttr_PropNameAndPathAndFailbackPathAttr>();
            
            var result = walker.Walk().ToList();
            
            var expected = new List<ConfigElement>
            {
                new ConfigElement(
                    paths: new List<List<string>>
                    {
                        new List<string>
                        {
                            "FakeFailbackPathLevel0",
                            "FakeFailbackPathLevel1",
                            "PropFakeName",
                        },
                        new List<string>
                        {
                            "FakePathLevel0",
                            "FakePathLevel1",
                            "PropFakeName",
                        },
                    },
                    pathInConfigObject: new List<string>
                    {
                        "TestClass_ClassWithNameAndPathAttr_PropNameAndPathAndFailbackPathAttr",
                        "PropDouble",
                    },
                    elementType: typeof(double)
                 ),
            };

            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Walk_ClassWithNameAndPathAttr_PropNameAttr()
        {
            var walker = new ConfigWalker<TestClass_ClassWithNameAndPathAttr_PropNameAttr>();
            
            var result = walker.Walk().ToList();
            
            var expected = new List<ConfigElement>
            {
                new ConfigElement(
                    paths: new List<List<string>>
                    {
                        new List<string>
                        {
                            "FakePathLevel0",
                            "FakePathLevel1",
                            "TestClassFakeName",
                            "PropFakeName",
                        },
                    },
                    pathInConfigObject: new List<string>
                    {
                        "TestClass_ClassWithNameAndPathAttr_PropNameAttr",
                        "PropDouble",
                    },
                    elementType: typeof(double)
                 ),
            };

            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Walk_ClassWithNameAndPathAttr_PropNameAndFailbackPathAttr()
        {
            var walker = new ConfigWalker<TestClass_ClassWithNameAndPathAttr_PropNameAndFailbackPathAttr>();
            
            var result = walker.Walk().ToList();
            
            var expected = new List<ConfigElement>
            {
                new ConfigElement(
                    paths: new List<List<string>>
                    {
                        new List<string>
                        {
                            "FakeFailbackPathLevel0",
                            "FakeFailbackPathLevel1",
                            "PropFakeName",
                        },
                        new List<string>
                        {
                            "FakePathLevel0",
                            "FakePathLevel1",
                            "TestClassFakeName",
                            "PropFakeName",
                        },
                    },
                    pathInConfigObject: new List<string>
                    {
                        "TestClass_ClassWithNameAndPathAttr_PropNameAndFailbackPathAttr",
                        "PropDouble",
                    },
                    elementType: typeof(double)
                 ),
            };

            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Walk_NestedFailbackPathAndNameAttr_PropFailbackPathAndNameAttr()
        {
            var walker = new ConfigWalker<TestClass_NestedFailbackPathAndNameAttr_PropFailbackPathAndNameAttr>();
            
            var result = walker.Walk().ToList();
            
            var expected = new List<ConfigElement>
            {
                new ConfigElement(
                    paths: new List<List<string>>
                    {
                        new List<string>
                        {
                            "FakeFailbackPathLevel0",
                            "FakeFailbackPathLevel1",
                            "PropFakeName",
                        },
                        new List<string>
                        {
                            "FakeFailbackPathLevel0_1",
                            "FakeFailbackPathLevel1_1",
                            "PropFakeName",
                            "PropFakeName",
                        },
                        new List<string>
                        {
                            "TestClass_NestedFailbackPathAndNameAttr_PropFailbackPathAndNameAttr",
                            "PropFakeName",
                            "PropFakeName",
                        },
                    },
                    pathInConfigObject: new List<string>
                    {
                        "TestClass_NestedFailbackPathAndNameAttr_PropFailbackPathAndNameAttr",
                        "PropNestedClass",
                        "PropDouble",
                    },
                    elementType: typeof(double)
                 ),
            };

            result.Should().BeEquivalentTo(expected);
        }

        #region TestData
        
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
        
        private interface ITestClass_Interface : IConfigObject
        {
            string PropString { set; get; }
        }
    
        private class TestClass_InterfaceProp : IConfigObject
        {
            public ITestClass_Interface PropITestClassInterface { set; get; }
        }
        
        private class TestClass_GenericProp : IConfigObject
        {
                public TestClass_Generic<string> PropTestClassGeneric { set; get; }
        }
        
        private class TestClass_Generic<T> : IConfigObject
        {
            public string PropString { set; get; }
        }

        private class TestClass_PrivateAndStaticProps : IConfigObject
        {
            private string PropStringPrivate { set; get; }
            public string PropStringPrivateSet { private set; get; }
            public string PropStringPrivateGet { set; private get; }
            public static string PropStringStatic { set; get; }
        }
        
        private class TestClass_WithoutAttr : IConfigObject
        {
            public string PropString { set; get; }
        }

        [ConfigRetrieverPath("FakePathLevel0", "FakePathLevel1")]
        private class TestClass_ClassWithPathAttr : IConfigObject
        {
            public bool PropBool { set; get; }
        }
        
        [ConfigRetrieverPath("FakePathLevel0", "FakePathLevel1")]
        [ConfigRetrieverElementName("TestClassFakeName")]
        private class TestClass_ClassWithNameAndPathAttr : IConfigObject
        {
            public int PropInt { set; get; }
        }
        
        private class TestClass_PropIgnoreAttr : IConfigObject
        {
            [ConfigRetrieverIgnore]
            public int PropInt { set; get; }
        }
        
        private class TestClass_PropFailbackPathAttr : IConfigObject
        {
            [ConfigRetrieverFailbackPath("FakeFailbackPathLevel0", "FakeFailbackPathLevel1")]
            public long PropLong { set; get; }
        }

        private class TestClass_PropPathAndFailbackPathAttr : IConfigObject
        {
            [ConfigRetrieverPath("FakePathLevel0", "FakePathLevel1")]
            [ConfigRetrieverFailbackPath("FakeFailbackPathLevel0", "FakeFailbackPathLevel1")]
            public long PropLong { set; get; }
        }
        
        private class TestClass_PropNameAndPathAndFailbackPathAttr : IConfigObject
        {
            [ConfigRetrieverPath("FakePathLevel0", "FakePathLevel1")]
            [ConfigRetrieverFailbackPath("FakeFailbackPathLevel0", "FakeFailbackPathLevel1")]
            [ConfigRetrieverElementName("PropFakeName")]
            public double PropDouble { set; get; }
        }
        
        [ConfigRetrieverPath("FakePathLevel0", "FakePathLevel1")]
        [ConfigRetrieverElementName("TestClassFakeName")]
        private class TestClass_ClassWithNameAndPathAttr_PropNameAndPathAndFailbackPathAttr : IConfigObject
        {
            [ConfigRetrieverPath("FakePathLevel0", "FakePathLevel1")]
            [ConfigRetrieverFailbackPath("FakeFailbackPathLevel0", "FakeFailbackPathLevel1")]
            [ConfigRetrieverElementName("PropFakeName")]
            public double PropDouble { set; get; }
        }
        
        [ConfigRetrieverPath("FakePathLevel0", "FakePathLevel1")]
        [ConfigRetrieverElementName("TestClassFakeName")]
        private class TestClass_ClassWithNameAndPathAttr_PropNameAttr : IConfigObject
        {
            [ConfigRetrieverElementName("PropFakeName")]
            public double PropDouble { set; get; }
        }
        
        [ConfigRetrieverPath("FakePathLevel0", "FakePathLevel1")]
        [ConfigRetrieverElementName("TestClassFakeName")]
        private class TestClass_ClassWithNameAndPathAttr_PropNameAndFailbackPathAttr : IConfigObject
        {
            [ConfigRetrieverFailbackPath("FakeFailbackPathLevel0", "FakeFailbackPathLevel1")]
            [ConfigRetrieverElementName("PropFakeName")]
            public double PropDouble { set; get; }
        }

        private class TestClass_NestedFailbackPathAndNameAttr_PropFailbackPathAndNameAttr : IConfigObject
        {
            [ConfigRetrieverFailbackPath("FakeFailbackPathLevel0_1", "FakeFailbackPathLevel1_1")]
            [ConfigRetrieverElementName("PropFakeName")]
            public TestClass_ClassWithNameAndPathAttr_PropNameAndFailbackPathAttr PropNestedClass { set; get; }
        }
        
        #endregion TestData
    }
}