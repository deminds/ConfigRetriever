using System.IO;
using NUnit.Framework;

namespace GH.DD.ConfigRetriever.Tests
{
    public class ConvertProviderTests
    {
        [Test]
        public void Convert_ToString_Success()
        {
            var convertProvider = new ConvertProvider();
            var converter = convertProvider.GetConverter(typeof(string));

            var rawData = "some data";
            var result = converter.Convert(rawData);

            var expected = "some data";
            
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void Convert_ToInt_Success()
        {
            var convertProvider = new ConvertProvider();
            var converter = convertProvider.GetConverter(typeof(int));

            var rawData = "101";
            var result = converter.Convert(rawData);

            var expected = 101;
            
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void Convert_ToInt_Exception_ContainsChar()
        {
            var convertProvider = new ConvertProvider();
            var converter = convertProvider.GetConverter(typeof(int));

            var rawData = "101" + "some string";

            Assert.Throws<InvalidDataException>(() =>
            {
                converter.Convert(rawData);
            });
        }
        
        [Test]
        public void Convert_ToInt_Exception_FromDouble()
        {
            var convertProvider = new ConvertProvider();
            var converter = convertProvider.GetConverter(typeof(int));

            var rawData = "10.56";

            Assert.Throws<InvalidDataException>(() =>
            {
                converter.Convert(rawData);
            });
        }
        
        [Test]
        public void Convert_ToBool_True_Success()
        {
            var convertProvider = new ConvertProvider();
            var converter = convertProvider.GetConverter(typeof(bool));

            var rawData = "TrUe";
            var result = converter.Convert(rawData);

            var expected = true;
            
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void Convert_ToBool_False_Success()
        {
            var convertProvider = new ConvertProvider();
            var converter = convertProvider.GetConverter(typeof(bool));

            var rawData = "FaLsE";
            var result = converter.Convert(rawData);

            var expected = false;
            
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void Convert_ToBool_Exception()
        {
            var convertProvider = new ConvertProvider();
            var converter = convertProvider.GetConverter(typeof(bool));

            var rawData = "";

            Assert.Throws<InvalidDataException>(() =>
            {
                converter.Convert(rawData);
            });
        }
        
        [Test]
        public void Convert_ToLong_Success()
        {
            var convertProvider = new ConvertProvider();
            var converter = convertProvider.GetConverter(typeof(long));

            var rawData = ((long)int.MaxValue + 10).ToString();
            var result = converter.Convert(rawData);

            var expected = 2147483657;
            
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void Convert_ToLong_Exception_ContainsChar()
        {
            var convertProvider = new ConvertProvider();
            var converter = convertProvider.GetConverter(typeof(long));

            var rawData = ((long)int.MaxValue + 10).ToString() + "some string";

            Assert.Throws<InvalidDataException>(() =>
            {
                converter.Convert(rawData);
            });
        }
        
        [Test]
        public void Convert_ToLong_Exception_FromDouble()
        {
            var convertProvider = new ConvertProvider();
            var converter = convertProvider.GetConverter(typeof(long));

            var rawData = "10.56";

            Assert.Throws<InvalidDataException>(() =>
            {
                converter.Convert(rawData);
            });
        }
        
        [Test]
        public void Convert_ToDouble_Success_FromInt()
        {
            var convertProvider = new ConvertProvider();
            var converter = convertProvider.GetConverter(typeof(double));

            var rawData = "10";
            var result = converter.Convert(rawData);

            var expected = 10;
            
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void Convert_ToDouble_Exception_ContainsChar()
        {
            var convertProvider = new ConvertProvider();
            var converter = convertProvider.GetConverter(typeof(double));

            var rawData = "19999999999" + "some string";

            Assert.Throws<InvalidDataException>(() =>
            {
                converter.Convert(rawData);
            });
        }
    }
}