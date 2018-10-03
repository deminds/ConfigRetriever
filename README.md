# ConfigRetriever
Retrieve and fill out you config object from the Consul

# Install
You can install via [nuget](https://www.nuget.org/packages/GH.DD.ConfigRetriever/)

# Common
You can find element in Consul by two paths.  
In native path or `ConfigRetrieverPath`. Native path is path that is defined by you config class structure.  
And in `ConfigRetieverFailbackPath`.

# Restriction
You can fill in config object from Consul only several types:  
* string
* bool
* int
* long
* double

If in config object will find other type will throw exception.

# Attributes
| *Attribute*                 | *Level for use*  | *Description*                                                                                                                    |
|-----------------------------|------------------|----------------------------------------------------------------------------------------------------------------------------------|
| ConfigRetrieverElementName  | Class / Property | Redefine class or property name in Consul. By default will find in Consul by class or propery name.                              |
| ConfigRetrieverPath         | Class / Property | Set path to element in Consul (without element name). By default will find in Consul by native path (path from class structure). |
| ConfigRetrieverFailbackPath | Property         | If element will not find in Consul by native path or `ConfigRetrieverPath`. Second place for search that path.                   |
| ConfigRetrieverIgnore       | Property         | Ignore this property when fill config object from Consul                                                                         |

# Simple example
### Config object
```c#
private class TestClass1 : IConfigObject
{
    [ConfigRetrieverIgnore]
    public int PropInt_Ignore { set; get; }
    
    [ConfigRetrieverFailbackPath()]
    public TestClass2 PropTestClass2 { set; get; }
}

private class TestClass2 : IConfigObject
{
    public string PropString { set; get; }
}
```

### Code for fill config object from Consul
```c#
var retriever = new ConsulRetriever("http://localhost:8500", null);
var configRetriever = new ConfigRetriever<TestClass1>(retriever);

var result = await configRetriever.Fill();
```

### Explain. Paths appears in priority
* Property `TestClass1.PropInt_Ignore` will ignore
* Paths for find property `TestClass1.PropTestClass2.PropString`
  1. _TestClass1 / PropTestClass2 / PropString_
  2. _PropTestClass2 / PropString_

# Complex example
### Config object
```c#
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
```

##### Result (immitation)
```c#
var result = new TestClass1()
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
}
```
### Explain. Paths appears in priority
* Property `TestClass1.PropTestClass1_1_Ignore` will ignore
* Paths for find property `TestClass1.PropTestClass1_1.PropInt`
  1. _FakePath0 / FakePath1 / TestClass1 / PropTestClass1_1_FakeName / PropInt_
* Paths for find property `TestClass1.PropTestClass1_1.PropDouble`
  1. _FakePath0 / FakePath1 / TestClass1 / PropTestClass1_1_FakeName / PropDoubleFakeName_
* Paths for find property `TestClass1.PropTestClass1_1.PropDouble_Ignore` will ignore
* Paths for find property `TestClass1.PropTestClass2.PropString`
  1. _FakePath0 / FakePath1 / PropTestClass2 / PropString_ 
  2. _FakePath0 / PropTestClass2 / PropString_ 
* Paths for find property `TestClass1.PropTestClass2.PropTestClass3.PropBool`
  1. _FakePath0 / FakePath1 / PropTestClass2 / PropTestClass3 / PropBool_ 
  2. _FakePath0 / PropTestClass2 / PropTestClass3 / PropBool_ 
  3. _FakePath0 / PropTestClass3 / PropBool_
* Paths for find property `TestClass1.PropTestClass2.PropTestClass3.PropLong`
  1. _FakePath0 / FakePath1 / PropTestClass2 / PropTestClass3 / PropLongFakeName_ 
  2. _FakePath0 / PropTestClass2 / PropTestClass3 / PropLongFakeName_ 
  3. _FakePath0 / PropTestClass3 / PropLongFakeName_
  4. _PropLongFakeName_

# Debug
You can enable debug mode. Via logger `ILogger`.  
This is two loggers:
* `StubLogger` - default logger. Do nothing
* `StdoutLogger` - write to stdout

You can define you own logger. Just implement `ILogger` interface.

### Example debug mode
```c#
var retriever = new ConsulRetriever("http://localhost:8500", null);

var logger = new StdoutLogger();
var configRetriever = new ConfigRetriever<TestClass1>(retriever, logger);

var result = await configRetriever.Fill();
```

# Enhansment
You can implement `IRetriever` for other config systems.  

# Notes
* This tool work on `property` level. If you have `ConfigRetrieverFailbackPath` attribute on you nested class and some of props that class will find in base path in Consul but rest props will not set in Consul by base path then that props will get in Consul `FailbackPath`.
* You can define attributes `ConfigRetrieverElementName` and `ConfigRetrieverPath` for class. But only for you root class. For nested classes it will ignore. For nested classes use that attributes on property.
* Every you configuration class must implement interface `IConfigObject`. Nested classes also must implement that interface.
* For `ConsulRetriever` in `url` param must contain http schema type (`http` / `https`)

## Feel free for open any issues