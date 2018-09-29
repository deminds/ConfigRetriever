using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace GH.DD.ConfigRetriever.Tests
{
    public class ConfigElementTest
    {
        private List<List<string>> _path0 = new List<List<string>>();
        
        private List<List<string>> _path1 = new List<List<string>>()
        {
            new List<string>()
        };
        
        private List<List<string>> _path2 = new List<List<string>>()
        {
            new List<string>()
            {
                "FirstPathLevel",
                "SecondPathLevel"
            },
            new List<string>()
        };
        
        private List<string> _path_in_config = new List<string>()
        {
            "FirstPathLevel",
        };
        
        private Type _elementType = typeof(string);

        [Test]
        public void ToString_Paths_Count2()
        {
            var configElement = new ConfigElement(_path2, _path_in_config, _elementType);
            
            var expected = "ElementType: String, PathInConfigObject: \"/FirstPathLevel\", Paths: [\"/FirstPathLevel/SecondPathLevel\", \"/\"]";
            
            var result = configElement.ToString();

            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void ToString_Paths_Count1()
        {
            var configElement = new ConfigElement(_path1, _path_in_config, _elementType);
            
            var expected = "ElementType: String, PathInConfigObject: \"/FirstPathLevel\", Paths: [\"/\"]";
            
            var result = configElement.ToString();

            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Paths_Count0()
        {
            Assert.Throws(typeof(ArgumentException), 
                () => new ConfigElement(_path0, _path_in_config, _elementType));
        }
        
        [Test]
        public void Paths_Null()
        {
            Assert.Throws(typeof(ArgumentNullException), 
                () => new ConfigElement(null, _path_in_config, _elementType));
        }
        
        [Test]
        public void PathInConfig_Count0()
        {
            Assert.Throws(typeof(ArgumentException), 
                () => new ConfigElement(_path1, new List<string>(), _elementType));
        }
        
        [Test]
        public void PathInConfigObject_Null()
        {
            Assert.Throws(typeof(ArgumentNullException), 
                () => new ConfigElement(_path1, null, _elementType));
        }
        
        [Test]
        public void ElementType_UnsupportedElementType()
        {
            Assert.Throws(typeof(ArgumentException), 
                () => new ConfigElement(_path1, _path_in_config, typeof(float)));
        }

        [Test]
        public void GetNextPath_PathCount3()
        {
            
            var paths = new List<List<string>>()
            {
                new List<string>()
                {
                    "FirstPathLevel",
                    "SecondPathLevel"
                },
                new List<string>(),
                new List<string>()
                {
                    "FirstPathLevel"
                }
            };
            
            var configElement = new ConfigElement(paths, _path_in_config, _elementType);
            
            var expected = new List<List<string>>()
            {
                new List<string>()
                {
                    "FirstPathLevel"
                },
                new List<string>(),
                new List<string>()
                {
                    "FirstPathLevel",
                    "SecondPathLevel"
                },
            };
            
            var result = new List<List<string>>();
            foreach (var path in configElement.GetNextPath())
            {
                result.Add(path);
            }

            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void GetNextPath_PathCount1()
        {
            
            var paths = new List<List<string>>()
            {
                new List<string>()
                {
                    "FirstPathLevel"
                }
            };
            
            var configElement = new ConfigElement(paths, _path_in_config, _elementType);
            
            var expected = new List<List<string>>()
            {
                new List<string>()
                {
                    "FirstPathLevel"
                },
            };
            
            var result = new List<List<string>>();
            foreach (var path in configElement.GetNextPath())
            {
                result.Add(path);
            }

            result.Should().BeEquivalentTo(expected);
        }
    }
}