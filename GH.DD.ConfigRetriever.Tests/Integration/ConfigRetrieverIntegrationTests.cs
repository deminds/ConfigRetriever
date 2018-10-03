using FluentAssertions;
using GH.DD.ConfigRetriever.Attributes;
using GH.DD.ConfigRetriever.Loggers;
using GH.DD.ConfigRetriever.Retrievers;
using NUnit.Framework;

namespace GH.DD.ConfigRetriever.Tests.Integration
{
    [Category("integration")]
    public class ConfigRetrieverIntegrationTests
    {
        private const string ConsulHttpSchema = "http";
        private const string ConsulHost = "consul";
        private const int ConsulPort = 8500;
        private const string ConsulAclToken = "";
        
        [Test]
        public void ConfigRetriever_ManualWithUrlInParts()
        {
            var retriever = new ConsulRetriever(ConsulHttpSchema, ConsulHost, ConsulPort, ConsulAclToken);
            var configRetriever = new ConfigRetriever<TestClass1>(retriever);

            var resultTask = configRetriever.Fill();
            var result = resultTask.Result;
                
            var expected = new TestClass1()
            {
                PropTestClass1_1 = new TestClass1_1()
                {
                    PropInt = 10,
                    PropDouble = 10.10
                },
                PropTestClass2 = new TestClass2()
                {
                    PropString = "some string",
                    PropTestClass3 = new TestClass3()
                    {
                        PropBool = true,
                        PropLong = 1000,
                        PropString = "some string"
                    }
                }
            };
            
            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void ConfigRetriever_ManualWithUrl()
        {
            var url = $"{ConsulHttpSchema}://{ConsulHost}:{ConsulPort}";
            var retriever = new ConsulRetriever(url, ConsulAclToken);
            var logger = new StdoutLogger();
            var configRetriever = new ConfigRetriever<TestClass1>(retriever, logger);

            var resultTask = configRetriever.Fill();
            var result = resultTask.Result;
                
            var expected = new TestClass1()
            {
                PropTestClass1_1 = new TestClass1_1()
                {
                    PropInt = 10,
                    PropDouble = 10.10
                },
                PropTestClass2 = new TestClass2()
                {
                    PropString = "some string",
                    PropTestClass3 = new TestClass3()
                    {
                        PropBool = true,
                        PropLong = 1000,
                        PropString = "some string"
                    }
                }
            };
            
            result.Should().BeEquivalentTo(expected);
        }

        #region TestData

        [ConfigRetrieverPath("FakePath0", "FakePath1")]
        private class TestClass1 : IConfigObject
        {
            [ConfigRetrieverIgnore]
            public TestClass1_1 PropTestClass1_1_Ignore { set; get; }
            
            [ConfigRetrieverElementName("PropTestClass1_1_FakeName")]
            public TestClass1_1 PropTestClass1_1 { set; get; }
            
            [ConfigRetrieverPath("FakePath0", "FakePath1")]
            [ConfigRetrieverFailbackPath("FakePath0")]
            public TestClass2 PropTestClass2 { set; get; }
        }
        
        private class TestClass1_1 : IConfigObject
        {
            public int PropInt { set; get; }
            
            [ConfigRetrieverElementName("PropDoubleFakeName")]
            public double PropDouble { set; get; }
            
            [ConfigRetrieverIgnore]
            public int PropDouble_Ignore { set; get; }
        }

        private class TestClass2 : IConfigObject
        {
            public string PropString { set; get; }
            
            [ConfigRetrieverFailbackPath("FakePath0")]
            public TestClass3 PropTestClass3 { set; get; }
        }
        
        private class TestClass3 : IConfigObject
        {
            public bool PropBool { set; get; }
            
            [ConfigRetrieverElementName("PropLongFakeName")]
            [ConfigRetrieverFailbackPath()]
            public long PropLong { set; get; }
            
            public string PropString { set; get; }
        }
        
        #endregion TestData
    }
}