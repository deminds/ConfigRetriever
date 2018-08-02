using System.Collections.Generic;
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
            var expected = new List<ConfigElement>()
            {
                new ConfigElement(
                    "StringProp",
                    "StringProp",
                    new List<string>() {"TestClassWithoutAttr"},
                    typeof(string),
                    false
                ),
                new ConfigElement(
                    "IntProp",
                    "IntProp",
                    new List<string>() {"TestClassWithoutAttr"},
                    typeof(int),
                    false
                ),
                new ConfigElement(
                    "BoolProp",
                    "BoolProp",
                    new List<string>() {"TestClassWithoutAttr"},
                    typeof(bool),
                    false
                ),
            };

            var walker = new ConfigWalker<TestClassWithoutAttr>();

            var result = new List<ConfigElement>();
            foreach (var elem in walker.Walk())
            {
                result.Add(elem);
            }

            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Walk_WithClassAttr()
        {
            var expected = new List<ConfigElement>()
            {
                new ConfigElement(
                    "StringProp",
                    "StringProp",
                    new List<string>() {"FirstLevelPath", "SecondLevelPath", "SomeNameTestClass"},
                    typeof(string),
                    false
                ),
                new ConfigElement(
                    "IntProp",
                    "IntProp",
                    new List<string>() {"FirstLevelPath", "SecondLevelPath", "SomeNameTestClass"},
                    typeof(int),
                    false
                ),
                new ConfigElement(
                    "BoolProp",
                    "BoolProp",
                    new List<string>() {"FirstLevelPath", "SecondLevelPath", "SomeNameTestClass"},
                    typeof(bool),
                    false
                ),
            };

            var walker = new ConfigWalker<TestClassWithClassAttr>();

            var result = new List<ConfigElement>();
            foreach (var elem in walker.Walk())
            {
                result.Add(elem);
            }

            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Walk_WithClassAndPropAttr()
        {
            var expected = new List<ConfigElement>()
            {
                new ConfigElement(
                    "SomeStringProp",
                    "StringProp",
                    new List<string>() {"FirstLevelPath", "SecondLevelPath", "SomeNameTestClass"},
                    typeof(string),
                    true
                ),
                new ConfigElement(
                    "SomeIntProp",
                    "IntProp",
                    new List<string>(),
                    typeof(int),
                    false
                ),
                new ConfigElement(
                    "BoolProp",
                    "BoolProp",
                    new List<string>() {"SomeFirstLevelPath", "SomeSecondLevelPath"},
                    typeof(bool),
                    false
                ),
            };

            var walker = new ConfigWalker<TestClassWithClassAndPropAttr>();

            var result = new List<ConfigElement>();
            foreach (var elem in walker.Walk())
            {
                result.Add(elem);
            }

            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Walk_WithPropAttr()
        {
            var expected = new List<ConfigElement>()
            {
                new ConfigElement(
                    "SomeStringProp",
                    "StringProp",
                    new List<string>() {"TestClassWithPropAttr"},
                    typeof(string),
                    true
                ),
                new ConfigElement(
                    "SomeIntProp",
                    "IntProp",
                    new List<string>(),
                    typeof(int),
                    false
                ),
                new ConfigElement(
                    "BoolProp",
                    "BoolProp",
                    new List<string>() {"SomeFirstLevelPath", "SomeSecondLevelPath"},
                    typeof(bool),
                    false
                ),
            };

            var walker = new ConfigWalker<TestClassWithPropAttr>();

            var result = new List<ConfigElement>();
            foreach (var elem in walker.Walk())
            {
                result.Add(elem);
            }

            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Walk_WithClassNameAndPropAttr()
        {
            var expected = new List<ConfigElement>()
            {
                new ConfigElement(
                    "SomeStringProp",
                    "StringProp",
                    new List<string>() {"SomeNameTestClass"},
                    typeof(string),
                    true
                ),
                new ConfigElement(
                    "SomeIntProp",
                    "IntProp",
                    new List<string>(),
                    typeof(int),
                    false
                ),
                new ConfigElement(
                    "BoolProp",
                    "BoolProp",
                    new List<string>() {"SomeFirstLevelPath", "SomeSecondLevelPath"},
                    typeof(bool),
                    false
                ),
            };

            var walker = new ConfigWalker<TestClassWithClassNameAndPropAttr>();

            var result = new List<ConfigElement>();
            foreach (var elem in walker.Walk())
            {
                result.Add(elem);
            }

            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Walk_WithClassPathAndPropAttr()
        {
            var expected = new List<ConfigElement>()
            {
                new ConfigElement(
                    "SomeStringProp",
                    "StringProp",
                    new List<string>() {"FirstLevelPath", "SecondLevelPath", "TestClassWithClassPathAndPropAttr"},
                    typeof(string),
                    true
                ),
                new ConfigElement(
                    "SomeIntProp",
                    "IntProp",
                    new List<string>(),
                    typeof(int),
                    false
                ),
                new ConfigElement(
                    "BoolProp",
                    "BoolProp",
                    new List<string>() {"SomeFirstLevelPath", "SomeSecondLevelPath"},
                    typeof(bool),
                    false
                ),
            };

            var walker = new ConfigWalker<TestClassWithClassPathAndPropAttr>();

            var result = new List<ConfigElement>();
            foreach (var elem in walker.Walk())
            {
                result.Add(elem);
            }

            result.Should().BeEquivalentTo(expected);
        }
        
        private class TestClassWithoutAttr
        {
            public string PropPrivateGetSkip { set; private get; }
            public string PropPrivateSetSkip { private set; get; }
            private string PropPrivateSkip { set; get; }
            internal string PropInternalSkip { set; get; }

            public string StringProp { set; get; }
            public int IntProp { set; get; }
            public bool BoolProp { set; get; }
        }
        
        [ConfigRetrieverElementName("SomeNameTestClass")]
        [ConfigRetrieverPath("FirstLevelPath", "SecondLevelPath")]
        private class TestClassWithClassAttr
        {
            public string StringProp { set; get; }
            public int IntProp { set; get; }
            public bool BoolProp { set; get; }
        }
        
        [ConfigRetrieverElementName("SomeNameTestClass")]
        [ConfigRetrieverPath("FirstLevelPath", "SecondLevelPath")]
        private class TestClassWithClassAndPropAttr
        {
            [ConfigRetrieverIgnore]
            public string StringPropSkip { set; get; }
            
            [ConfigRetrieverElementName("SomeStringProp")]
            [ConfigRetrieverCanFloatUp]
            public string StringProp { set; get; }
            
            [ConfigRetrieverPath]
            [ConfigRetrieverElementName("SomeIntProp")]
            public int IntProp { set; get; }
            
            [ConfigRetrieverPath("SomeFirstLevelPath", "SomeSecondLevelPath")]
            public bool BoolProp { set; get; }
        }
        
        private class TestClassWithPropAttr
        {
            [ConfigRetrieverIgnore]
            public string StringPropSkip { set; get; }
            
            [ConfigRetrieverElementName("SomeStringProp")]
            [ConfigRetrieverCanFloatUp]
            public string StringProp { set; get; }
            
            [ConfigRetrieverPath]
            [ConfigRetrieverElementName("SomeIntProp")]
            public int IntProp { set; get; }
            
            [ConfigRetrieverPath("SomeFirstLevelPath", "SomeSecondLevelPath")]
            public bool BoolProp { set; get; }
        }
        
        [ConfigRetrieverElementName("SomeNameTestClass")]
        private class TestClassWithClassNameAndPropAttr
        {
            [ConfigRetrieverIgnore]
            public string StringPropSkip { set; get; }
            
            [ConfigRetrieverElementName("SomeStringProp")]
            [ConfigRetrieverCanFloatUp]
            public string StringProp { set; get; }
            
            [ConfigRetrieverPath]
            [ConfigRetrieverElementName("SomeIntProp")]
            public int IntProp { set; get; }
            
            [ConfigRetrieverPath("SomeFirstLevelPath", "SomeSecondLevelPath")]
            public bool BoolProp { set; get; }
        }
        
        [ConfigRetrieverPath("FirstLevelPath", "SecondLevelPath")]
        private class TestClassWithClassPathAndPropAttr
        {
            [ConfigRetrieverIgnore]
            public string StringPropSkip { set; get; }
            
            [ConfigRetrieverElementName("SomeStringProp")]
            [ConfigRetrieverCanFloatUp]
            public string StringProp { set; get; }
            
            [ConfigRetrieverPath]
            [ConfigRetrieverElementName("SomeIntProp")]
            public int IntProp { set; get; }
            
            [ConfigRetrieverPath("SomeFirstLevelPath", "SomeSecondLevelPath")]
            public bool BoolProp { set; get; }
        }
    }
}