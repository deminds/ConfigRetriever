using System.Collections.Generic;
using System.Linq;
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
            string value;
            
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
                
                // found
                new ConfigElement(
                    name: "BoolProp3",
                    nameConfigProperty: "BoolProp3",
                    path: new List<string>() {"FirstPathLevel", "SecondPathLevel", "TestConfig"},
                    elementType: typeof(bool),
                    canFloatUp: true),
                
                // not found
                new ConfigElement(
                    name: "BoolProp4",
                    nameConfigProperty: "BoolProp4",
                    path: new List<string>() {"TestConfig"},
                    elementType: typeof(bool),
                    canFloatUp: true),
                
                // found
                new ConfigElement(
                    name: "BoolProp5",
                    nameConfigProperty: "BoolProp5",
                    path: new List<string>() {"TestConfig"},
                    elementType: typeof(bool),
                    canFloatUp: true),
            };
            
            var retrieverMock = new Mock<IRetriever>();
            value = null;
            retrieverMock.Setup(retriever => retriever.TryRetrieve(
                It.Is<ConfigElement>(e => e.Name==configElements[0].Name && e.Path.SequenceEqual(configElements[0].Path)), 
                out value)).Returns(false);
            
            value = "true";
            retrieverMock.Setup(retriever => retriever.TryRetrieve(
                It.Is<ConfigElement>(e => e.Name==configElements[1].Name && e.Path.SequenceEqual(configElements[1].Path)), 
                out value)).Returns(true);
            
            value = null;
            retrieverMock.Setup(retriever => retriever.TryRetrieve(
                It.Is<ConfigElement>(e => e.Name==configElements[2].Name && e.Path.SequenceEqual(configElements[2].Path)), 
                out value)).Returns(false);
            
            value = "true";
            retrieverMock.Setup(retriever => retriever.TryRetrieve(
                It.Is<ConfigElement>(e => e.Name==configElements[2].Name && e.Path.SequenceEqual(configElements[2].Path.GetRange(0, configElements[2].Path.Count-1))), 
                out value)).Returns(true);
            
            value = null;
            retrieverMock.Setup(retriever => retriever.TryRetrieve(
                It.Is<ConfigElement>(e => e.Name==configElements[3].Name && e.Path.SequenceEqual(configElements[3].Path)), 
                out value)).Returns(false);
            retrieverMock.Setup(retriever => retriever.TryRetrieve(
                It.Is<ConfigElement>(e => e.Name==configElements[3].Name && e.Path.SequenceEqual(configElements[3].Path.GetRange(0, configElements[3].Path.Count-1))), 
                out value)).Returns(false);
            retrieverMock.Setup(retriever => retriever.TryRetrieve(
                It.Is<ConfigElement>(e => e.Name==configElements[4].Name && e.Path.SequenceEqual(configElements[4].Path)), 
                out value)).Returns(false);
            
            value = "true";
            retrieverMock.Setup(retriever => retriever.TryRetrieve(
                It.Is<ConfigElement>(e => e.Name==configElements[4].Name && e.Path.SequenceEqual(configElements[4].Path.GetRange(0, configElements[4].Path.Count-1))), 
                out value)).Returns(true);

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
                BoolProp3 = true,
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