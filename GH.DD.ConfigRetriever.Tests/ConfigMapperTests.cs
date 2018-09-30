using System;
using System.Collections.Generic;
using System.Data;
using FluentAssertions;
using NUnit.Framework;

namespace GH.DD.ConfigRetriever.Tests
{
    public class ConfigMapperTests
    {
        [Test]
        public void Map_Simple_SuccessMap()
        {
            var value = "some value";
            var configMapper = new ConfigMapper<ClassSimple>();
            
            var path = new List<string>
            {
                "ClassName", 
                "PropStringPublic",
            };
            configMapper.Map(path, value);

            var expected = new ClassSimple()
            {
                PropStringPublic = value
            };

            var result = configMapper.GetResultObject();
            
            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Map_Simple_WrongTypeProperty_Exception()
        {
            var fakeValue = 10;
            var configMapper = new ConfigMapper<ClassSimple>();
            
            var path = new List<string>
            {
                "ClassName", 
                "PropStringPublic",
            };

            Assert.Throws<DataException>(() => configMapper.Map(path, fakeValue));
        }
        
        [Test]
        public void Map_Simple_PropertyNotFound_Exception()
        {
            var value = "some value";
            var configMapper = new ConfigMapper<ClassSimple>();
            
            var path = new List<string>
            {
                "ClassName", 
                "SomeFakeProperty",
            };

            Assert.Throws<NullReferenceException>(() => configMapper.Map(path, value));
        }

        [Test]
        public void Map_Simple_PrivateProperty_Exception()
        {
            var value = "some value";
            var configMapper = new ConfigMapper<ClassSimple>();
            
            var path = new List<string>
            {
                "ClassName", 
                "PropStringPrivate",
            };

            Assert.Throws<NullReferenceException>(() => configMapper.Map(path, value));
        }

        [Test]
        public void Map_Nested_SuccessMap()
        {
            var valueString = "some string value";
            var valueInt = 10;
            var valueDouble = 1.4;
            var valueLong = -15;
            var valueBool = true;
            
            var configMapper = new ConfigMapper<ClassDoubleNested>();
            
            configMapper.Map(new List<string>{"ClassDoubleNested", "PropInt"}, valueInt);
            configMapper.Map(new List<string>{"ClassDoubleNested", "PropString"}, valueString);
            configMapper.Map(new List<string>{"ClassDoubleNested", "PropClassNested1", "PropDouble"}, valueDouble);
            configMapper.Map(new List<string>{"ClassDoubleNested", "PropClassNested1", "PropLong"}, valueLong);
            configMapper.Map(new List<string>{"ClassDoubleNested", "PropClassNested1", "PropClass", "PropBool"}, valueBool);
            configMapper.Map(new List<string>{"ClassDoubleNested", "PropClassNested2", "PropDouble"}, valueDouble);
            configMapper.Map(new List<string>{"ClassDoubleNested", "PropClassNested2", "PropLong"}, valueLong);
            configMapper.Map(new List<string>{"ClassDoubleNested", "PropClassNested2", "PropClass", "PropBool"}, valueBool);

            var expected = new ClassDoubleNested()
            {
                PropInt = valueInt,
                PropString = valueString,
                PropClassNested1 = new ClassNested()
                {
                    PropDouble = valueDouble,
                    PropLong = valueLong,
                    PropClass = new Class()
                    {
                        PropBool = valueBool
                    }
                },
                PropClassNested2 = new ClassNested()
                {
                    PropDouble = valueDouble,
                    PropLong = valueLong,
                    PropClass = new Class()
                    {
                        PropBool = valueBool
                    }
                }
            };
            
            var result = configMapper.GetResultObject();
            
            result.Should().BeEquivalentTo(expected);
        }
        
        
        #region TestData

        class ClassSimple
        {
            public string PropStringPublic { set; get; }
            private string PropStringPrivate { set; get; }
        }

        class ClassDoubleNested
        {
            public string PropString { set; get; }
            public int PropInt { set; get; }
            
            public ClassNested PropClassNested1 { set; get; }
            public ClassNested PropClassNested2 { set; get; }
        }

        class ClassNested
        {
            public long PropLong { set; get; }
            public double PropDouble { set; get; }
            
            public Class PropClass { set; get; }
        }

        class Class
        {
            public bool PropBool { set; get; }
        }

        #endregion TestData
    }
}