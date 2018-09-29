using System.Threading.Tasks;
using FluentAssertions;
using GH.DD.ConfigRetriever.Attributes;
using Moq;
using NUnit.Framework;

namespace GH.DD.ConfigRetriever.Tests
{
    public class ConfigRetrieverTests
    {
        [Test]
        public void Fill_AvailableInRetriever()
        {
            string value;
            
            var retrieverMock = new Mock<IRetriever>();
            value = "some value";
            
            // Class1
            retrieverMock.Setup(retriever => retriever.Retrieve(
                new []{"FakePathLevel0", "FakePathLevel1", "TestClass1", "PropString"}))
                .Returns(Task.FromResult(value));
            
            // Class1_1
            retrieverMock.Setup(retriever => retriever.Retrieve(
                new []{"FakePathLevel0", "FakePathLevel1", "TestClass1", "TestClass1_1Name", "PropString"}))
                .Returns(Task.FromResult(value));
            
            // Class2
            retrieverMock.Setup(retriever => retriever.Retrieve(
                new []{"FakePathLevel0", "FakePathLevel1", "TestClass2", "PropString"}))
                .Returns(Task.FromResult(value));
            
            retrieverMock.Setup(retriever => retriever.Retrieve(
                new []{"FakePathLevel0", "TestClass2", "PropString"}))
                .Returns(Task.FromResult(value));
            
            // Class3
            retrieverMock.Setup(retriever => retriever.Retrieve(
                new []{"FakePathLevel0", "FakePathLevel1", "TestClass2", "TestClass3", "PropString"}))
                .Returns(Task.FromResult(value));
            
            retrieverMock.Setup(retriever => retriever.Retrieve(
                new []{"FakePathLevel0", "TestClass2", "TestClass3", "PropString"}))
                .Returns(Task.FromResult(value));
            
            retrieverMock.Setup(retriever => retriever.Retrieve(
                new []{"FakePathLevel0", "TestClass3", "PropString"}))
                .Returns(Task.FromResult(value));
            
            // =========

            var expected = new TestClass1()
            {
                PropString = value,
                PropTestClass1_1 = new TestClass1_1()
                {
                    PropString = value
                },
                TestClass2 = new TestClass2()
                {
                    PropString = value,
                    PropTestClass3 = new TestClass3()
                    {
                        PropString = value
                    }
                }
            };

            var configRetriever = new ConfigRetriever<TestClass1>(retrieverMock.Object);
            
            var resultTask = configRetriever.Fill();
            var result = resultTask.Result;
            
            result.Should().BeEquivalentTo(expected);
        }

        #region TestData
        
        [ConfigRetrieverPath("FakePathLevel0", "FakePathLevel1")]
        private class TestClass1 : IConfigObject
        {
            public string PropString { set; get; }
            
            [ConfigRetrieverElementName("TestClass1_1Name")]
            public TestClass1_1 PropTestClass1_1 { set; get; }
            
            [ConfigRetrieverPath("FakePathLevel0", "FakePathLevel1")]
            [ConfigRetrieverFailbackPath("FakePathLevel0")]
            public TestClass2 TestClass2 { set; get; }
        }

        // attribute must ignore
        [ConfigRetrieverPath("FakePathLevel0", "FakePathLevel1")]
        private class TestClass1_1 : IConfigObject
        {
            public string PropString { set; get; }
        }

        // attribute must ignore
        [ConfigRetrieverElementName("FakeName")]
        private class TestClass2 : IConfigObject
        {
            public string PropString { set; get; }
            
            [ConfigRetrieverFailbackPath("FakePathLevel0")]
            [ConfigRetrieverElementName("TestClass3")]
            public TestClass3 PropTestClass3 { set; get; }
        }

        private class TestClass3 : IConfigObject
        {
            public string PropString { set; get; }
        }
        
        #endregion TestData
    }
}