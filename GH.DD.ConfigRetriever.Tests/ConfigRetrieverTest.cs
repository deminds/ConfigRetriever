using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using GH.DD.ConfigRetriever.Attributes;
using Moq;
using NUnit.Framework;

namespace GH.DD.ConfigRetriever.Tests
{
    public class ConfigRetrieverTest
    {
        [Test]
        public void Fill_AvailableInRetriever()
        {
            var value = "true";
            
            var configElements = new List<ConfigElement>()
            {
                // not found
                new ConfigElement(
                    name: "BoolProp1",
                    nameConfigProperty: "BoolProp1",
                    path: new List<string>() {"FirstPathLevel", "SecondPathLevel", "TestConfig"},
                    elementType: typeof(bool),
                    canFloatUp: false),
                
                // found
                new ConfigElement(
                    name: "BoolProp2",
                    nameConfigProperty: "BoolProp2",
                    path: new List<string>() {"FirstPathLevel", "SecondPathLevel", "TestConfig"},
                    elementType: typeof(bool),
                    canFloatUp: false),
                
                // not found
                new ConfigElement(
                    name: "BoolProp3",
                    nameConfigProperty: "BoolProp3",
                    path: new List<string>() {"FirstPathLevel", "SecondPathLevel", "TestConfig"},
                    elementType: typeof(bool),
                    canFloatUp: true),
                
                // found
                new ConfigElement(
                    name: "BoolProp3",
                    nameConfigProperty: "BoolProp3",
                    path: new List<string>() {"FirstPathLevel", "SecondPathLevel"},
                    elementType: typeof(bool),
                    canFloatUp: true),
                
                // not found
                new ConfigElement(
                    name: "BoolProp4",
                    nameConfigProperty: "BoolProp4",
                    path: new List<string>() {"TestConfig"},
                    elementType: typeof(bool),
                    canFloatUp: true),
                
                // not found
                new ConfigElement(
                    name: "BoolProp4",
                    nameConfigProperty: "BoolProp4",
                    path: new List<string>() {},
                    elementType: typeof(bool),
                    canFloatUp: true),
                
                // not found
                new ConfigElement(
                    name: "BoolProp5",
                    nameConfigProperty: "BoolProp5",
                    path: new List<string>() {"TestConfig"},
                    elementType: typeof(bool),
                    canFloatUp: true),
                
                // found
                new ConfigElement(
                    name: "BoolProp5",
                    nameConfigProperty: "BoolProp5",
                    path: new List<string>() {"FirstPathLevel", "SecondPathLevel", "TestConfig"},
                    elementType: typeof(bool),
                    canFloatUp: true),
            };
            
            var retrieverMock = new Mock<IRetriever>();
            retrieverMock.Setup(retriever => retriever.TryRetrieve(configElements[0], out value)).Returns(false);
            retrieverMock.Setup(retriever => retriever.TryRetrieve(configElements[1], out value)).Returns(true);
            retrieverMock.Setup(retriever => retriever.TryRetrieve(configElements[2], out value)).Returns(false);
            retrieverMock.Setup(retriever => retriever.TryRetrieve(configElements[3], out value)).Returns(true);
            retrieverMock.Setup(retriever => retriever.TryRetrieve(configElements[4], out value)).Returns(false);
            retrieverMock.Setup(retriever => retriever.TryRetrieve(configElements[5], out value)).Returns(false);
            retrieverMock.Setup(retriever => retriever.TryRetrieve(configElements[6], out value)).Returns(false);
            retrieverMock.Setup(retriever => retriever.TryRetrieve(configElements[7], out value)).Returns(true);

            var walkerMock = new Mock<IConfigWalker>();
            walkerMock.Setup(w => w.Walk()).Returns(configElements);
            
            var configRetriever = new ConfigRetriever<TestAvailableIdRetrieverConfig>(retrieverMock.Object);
            
            var walkerFieldInfo = configRetriever.GetType().GetField("_walker", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            walkerFieldInfo.SetValue(configRetriever, walkerMock.Object);

            var result = configRetriever.Fill();
            
            var expected = new TestAvailableIdRetrieverConfig()
            {
                BoolProp1 = false,
                BoolProp2 = true,
                BoolProp3 = false,
                BoolProp4 = false,
                BoolProp5 = true
            };
            
            result.Should().BeEquivalentTo(expected);
        }
        
        [ConfigRetrieverPath("FirstPathLevel", "SecondPathLevel")]
        private class TestAvailableIdRetrieverConfig
        {
            // false
            public bool BoolProp1 { set; get; }
            
            // true
            public bool BoolProp2 { set; get; }
            
            // false
            public bool BoolProp3 { set; get; }
            
            // false
            [ConfigRetrieverPath("TestConfig")]
            public bool BoolProp4 { set; get; }
            
            // true
            [ConfigRetrieverPath("TestConfig")]
            public bool BoolProp5 { set; get; }
        }
    }
}