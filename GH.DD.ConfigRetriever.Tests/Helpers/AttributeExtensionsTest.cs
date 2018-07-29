using System;
using System.Collections.Generic;
using System.Reflection;
using GH.DD.ConfigRetriever.Attributes;
using GH.DD.ConfigRetriever.Helpers;
using NUnit.Framework;

namespace GH.DD.ConfigRetriever.Tests.Helpers
{
    public class AttributeExtensionsTest
    {
        private PropertyInfo _propertyInfoWithAttr;
        private PropertyInfo _propertyInfoWithoutAttr;
        private PropertyInfo _propertyInfoWrongAttr;
        private PropertyInfo _propertyInfoEmptyPathAttr;

        [OneTimeSetUp]
        public void Setup()
        {
            _propertyInfoWithAttr = typeof(TestClassWithAttr)
                .GetProperty(nameof(TestClassWithAttr.Prop));

            _propertyInfoWithoutAttr = typeof(TestClassWithoutAttr)
                .GetProperty(nameof(TestClassWithoutAttr.Prop));

            _propertyInfoWrongAttr = typeof(TestClassWrongAttr)
                .GetProperty(nameof(TestClassWrongAttr.Prop));

            _propertyInfoEmptyPathAttr = typeof(TestClassEmptyPathAttr)
                .GetProperty(nameof(TestClassEmptyPathAttr.Prop));
        }

        #region HasAttribute

        [Test]
        public void HasAttribute_Class_True()
        {
            var result = typeof(TestClassWithAttr).HasAttribute<ConfigRetrieverElementNameAttribute>();
            const bool expected = true;

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void HasAttribute_Class_False()
        {
            var result = typeof(TestClassWithoutAttr).HasAttribute<ConfigRetrieverElementNameAttribute>();
            const bool expected = false;

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void HasAttribute_Prop_True()
        {
            var result = _propertyInfoWithAttr.HasAttribute<ConfigRetrieverElementNameAttribute>();
            const bool expected = true;

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void HasAttribute_Prop_False()
        {
            var result = _propertyInfoWithoutAttr.HasAttribute<ConfigRetrieverElementNameAttribute>();
            const bool expected = false;

            Assert.AreEqual(expected, result);
        }

        #endregion HasAttribute

        #region GetAttributeValue_Class_Name

        [Test]
        public void GetAttributeValue_Class_Name_AttrExist()
        {
            var result = typeof(TestClassWithAttr)
                .GetAttributeValue((ConfigRetrieverElementNameAttribute a) => a.Name, false);

            const string expected = "SomeNameOfTestClass1";

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetAttributeValue_Class_Name_AttrNotExist()
        {
            var result = typeof(TestClassWithoutAttr)
                .GetAttributeValue((ConfigRetrieverElementNameAttribute a) => a.Name, false);

            const string expected = null;

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetAttributeValue_Class_Name_ThrowError()
        {
            Assert.Throws<ArgumentNullException>(() => typeof(TestClassWrongAttr)
                .GetAttributeValue((ConfigRetrieverElementNameAttribute a) => a.Name, false));
        }

        #endregion GetAttributeValue_Class_Name

        #region GetAttributeValue_Class_Path

        [Test]
        public void GetAttributeValue_Class_Path_AttrExist()
        {
            var result = typeof(TestClassWithAttr)
                .GetAttributeValue((ConfigRetrieverPathAttribute a) => a.Path, false);

            var expected = new List<string>() {"FirstLevelPath", "SecondLevelPath"};

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetAttributeValue_Class_Path_AttrNotExist()
        {
            var result = typeof(TestClassWithoutAttr)
                .GetAttributeValue((ConfigRetrieverPathAttribute a) => a.Path, false);

            const string expected = null;

            Assert.AreEqual(expected, result);
        }

        #endregion GetAttributeValue_Class_Path

        #region GetAttributeValue_Prop_Name

        [Test]
        public void GetAttributeValue_Prop_Name_AttrExist()
        {
            var result = _propertyInfoWithAttr
                .GetAttributeValue((ConfigRetrieverElementNameAttribute a) => a.Name, false);

            var expected = "SomeNameOfProp";

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetAttributeValue_Prop_Name_AttrNotExist()
        {
            var result = _propertyInfoWithoutAttr
                .GetAttributeValue((ConfigRetrieverElementNameAttribute a) => a.Name, false);

            const string expected = null;

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetAttributeValue_Prop_Name_ThrowError()
        {
            Assert.Throws<ArgumentNullException>(() => _propertyInfoWrongAttr
                .GetAttributeValue((ConfigRetrieverElementNameAttribute a) => a.Name, false));
        }

        #endregion GetAttributeValue_Prop_Name

        #region GetAttributeValue_Prop_Path

        [Test]
        public void GetAttributeValue_Prop_Path_AttrExist()
        {
            var result = _propertyInfoWithAttr
                .GetAttributeValue((ConfigRetrieverPathAttribute a) => a.Path, false);

            var expected = new List<string>() {"FirstLevelPath", "SecondLevelPath"};

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetAttributeValue_Prop_Path_AttrNotExist()
        {
            var result = _propertyInfoWithoutAttr
                .GetAttributeValue((ConfigRetrieverPathAttribute a) => a.Path, false);

            const string expected = null;

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetAttributeValue_Prop_Path_ThrowError()
        {
            Assert.Throws<ArgumentException>(() => _propertyInfoWrongAttr
                .GetAttributeValue((ConfigRetrieverPathAttribute a) => a.Path, false));
        }

        #endregion GetAttributeValue_Prop_Path

        #region GetAttributeValue_EmptyPath

        [Test]
        public void GetAttributeValue_Class_EmptyPath_AttrExist()
        {
            var result = typeof(TestClassEmptyPathAttr)
                .GetAttributeValue((ConfigRetrieverPathAttribute a) => a.Path, false);

            var expected = new List<string>();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetAttributeValue_Prop_EmptyPath_AttrExist()
        {
            var result = _propertyInfoEmptyPathAttr
                .GetAttributeValue((ConfigRetrieverPathAttribute a) => a.Path, false);

            var expected = new List<string>();

            Assert.AreEqual(expected, result);
        }

        #endregion GetAttributeValue_EmptyPath

        #region TestData

        [ConfigRetrieverElementName("SomeNameOfTestClass1")]
        [ConfigRetrieverPath("FirstLevelPath", "SecondLevelPath")]
        private class TestClassWithAttr
        {
            [ConfigRetrieverCanFloatUp]
            [ConfigRetrieverIgnore]
            [ConfigRetrieverElementName("SomeNameOfProp")]
            [ConfigRetrieverPath("FirstLevelPath", "SecondLevelPath")]
            public string Prop { set; get; }
        }

        private class TestClassWithoutAttr
        {
            public string Prop { set; get; }
        }

        [ConfigRetrieverElementName("")]
        private class TestClassWrongAttr
        {
            [ConfigRetrieverElementName("  ")]
            [ConfigRetrieverPath("FirstLevelPath", "    ")]
            public string Prop { set; get; }
        }

        [ConfigRetrieverPath]
        private class TestClassEmptyPathAttr
        {
            [ConfigRetrieverPath] 
            public string Prop { set; get; }
        }
        
        #endregion TestData
    }
}