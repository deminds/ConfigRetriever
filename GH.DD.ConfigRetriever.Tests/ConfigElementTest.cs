using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace GH.DD.ConfigRetriever.Tests
{
    public class ConfigElementTest
    {
        private string _name = "SomeName";
        private string _nameConfigProperty = "SomeNameConfigProperty";
        private Type _elementType = typeof(string);

        [Test]
        public void FloatUp_CanFloat_False()
        {
            var configElement = new ConfigElement(
                _name, 
                _nameConfigProperty, 
                new List<string>(), 
                _elementType,
                false);
            
            var expected = new List<ConfigElement>();
            
            var result = configElement.FloatUp().ToList();

            result.Should().BeEquivalentTo(expected);
        }
        [Test]
        public void FloatUp_CanFloat_True_PathLenthZero()
        {
            var configElement = new ConfigElement(
                _name, 
                _nameConfigProperty, 
                new List<string>(), 
                _elementType,
                true);

            var expected = new List<ConfigElement>();
            
            var result = new List<ConfigElement>();
            foreach (var elem in configElement.FloatUp())
            {
                result.Add(elem);
            }
            
            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void FloatUp_CanFloat_True_PathLenthOne()
        {
            var configElement = new ConfigElement(
                _name, 
                _nameConfigProperty, 
                new List<string>() {"FirstLevelPath"}, 
                _elementType,
                true);
            
            var expected = new List<ConfigElement>()
            {
                new ConfigElement(
                    _name, 
                    _nameConfigProperty, 
                    new List<string>(), 
                    typeof(string), 
                    true),
            };
            
            var result = new List<ConfigElement>();
            foreach (var elem in configElement.FloatUp())
            {
                result.Add(elem);
            }
            
            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void FloatUp_CanFloat_True_PathLenthThree()
        {
            var configElement = new ConfigElement(
                _name, 
                _nameConfigProperty, 
                new List<string>() {"FirstLevelPath", "SecondLevelPath", "ThirdLevelPath"}, 
                _elementType,
                true);
            
            var expected = new List<ConfigElement>()
            {
                new ConfigElement(
                    _name, 
                    _nameConfigProperty, 
                    new List<string>() {"FirstLevelPath", "SecondLevelPath"}, 
                    typeof(string), 
                    true),
                new ConfigElement(
                    _name, 
                    _nameConfigProperty, 
                    new List<string>() {"FirstLevelPath"}, 
                    typeof(string), 
                    true),
                new ConfigElement(
                    _name, 
                    _nameConfigProperty, 
                    new List<string>(), 
                    typeof(string), 
                    true),
            };
            
            var result = new List<ConfigElement>();
            foreach (var elem in configElement.FloatUp())
            {
                result.Add(elem);
            }
            
            result.Should().BeEquivalentTo(expected);
        }
    }
}