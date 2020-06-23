[![Version: 1.0 Release](https://img.shields.io/badge/Version-1.0%20Release-green.svg)](http://github.com/Z3r0z0/makeupdate/releases/latest/download/argumnetReader.zip) [![Build Status](https://www.travis-ci.org/0x007E/argumentreader.svg?branch=master)](https://www.travis-ci.org/Z3r0z0/makeupdate) [![License: GPL v3](https://img.shields.io/badge/License-GPL%20v3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0) [![codecov](https://codecov.io/gh/Z3r0z0/argumentreader/branch/master/graph/badge.svg)](https://codecov.io/gh/Z3r0z0/makeupdate)

# Argument Reader

## Description:

With Argument Reader command line arguments can be passed into a .net core application. The standard project assist 4 types of arguments:

* Boolean
* Strings *(\*)*
* Integers *(#)*
* Doubles *(##)*

Own argument types can be build with own classes. They need to inherit from the **ArgumentMarshalerLib**. Libraries are loading dynamically on startup. It is not necessary to recompile the complete solution.

---

## Structure

``` csharp
Arguments parameter = new Arguments("Path to Marshaler Libraries", "Schema", "Argument Array");
```

### Available Marshalers (Standard)

* BooleanMarshalerLib.dll
* StringMarshalerLib.dll
* IntegerMarshalerLib.dll
* DoubleMarshalerLib.dll

### Schema

1. Parameter name
1. Marshaler type

**Example**

``` csharp
Arguments parameter = new Arguments("...", "enabled,text*,number#,comma##", "...");
bool enabled = parameter.GetValue<bool>("enabled");
string text = parameter.GetValue<string>("text");
int number = parameter.GetValue<int>("number");
double comma = parameter.GetValue<double>("comma");
```

---

## Parse Arguments

### Boolean:

``` bash
Arguments.exe -a
```

``` csharp
static void Main(string[] args)
{
Arguments parameter = new Arguments(@".\Marshaler", "a,b*", args);
bool a = parameter.GetValue<bool>("a"); // True
bool b = parameter.GetValue<bool>("b"); // False
// ...
}
```

### String:

``` bash
Arguments.exe -a "This is a Text"
```

``` csharp
static void Main(string[] args)
{
Arguments parameter = new Arguments(@".\Marshaler", "a*", args);
string a = parameter.GetValue<string>("a"); // This is a Text
// ...
}
```

### Integer:

``` bash
Arguments.exe -a 1234
```

``` csharp
static void Main(string[] args)
{
Arguments parameter = new Arguments(@".\Marshaler", "a#", args);
int a = parameter.GetValue<int>("a"); // 1234
// ...
}
```

### Double:

``` bash
Arguments.exe -a 1234,4321
```

``` csharp
static void Main(string[] args)
{
Arguments parameter = new Arguments(@".\Marshaler", "a##", args);
double a = parameter.GetValue<double>("a"); // 1234,4321
// ...
}
```

---

## Build your own Marshaler

1. Create a new VisualStudio .NET Standard Classlibrary (**??MarshalerLib**)
1. Link a new project reference to ArgumentMarshalerLib.dll (in this repository)
1. Write Marshaler (See example code below)
1. Copy the TestMarshalerLib.dll to the Marshaler directory in your project
1. Implement the *?* in your schema (e.g. "mymarshaler?")

``` csharp
using System;
using ArgumentMarshalerLib;

namespace TestMarshalerLib
{
    public class TestMarshalerLib : ArgumentMarshaler
    {
        // Only schemas allowed that are not used (string.Empty, *, #, ## are already used from standard marshalers)
        public override string Schema => "?";

        public override void Set(Iterator<string> currentArgument)
        {
            try
            {
                // If implementation should use an argument behind the command (e.g. -a "??"),
                // it is necessary to move the Iterator to the next position.
                Value = currentArgument.Next();
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new TestMarshalerException(ErrorCode.MISSING);
            }

            // If no argument behind the command is used just add your value
            Value = "This is my personal marshaler";
        }

        public class TestMarshalerException : ArgumentsException
        {
            public TestMarshalerException() { }

            public TestMarshalerException(ErrorCode errorCode) : base(errorCode) { }

            public override string ErrorMessage()
            {
                switch (ErrorCode)
                {
                    case ErrorCode.MISSING:
                        return $"Could not find test parameter for -{ErrorArgumentId}";
                    default:
                        return string.Empty;
                }
            }
        }

    }
}
```

---

## References

The original Argument Marshaler was written in Java and published by Robert C. Martin in his book Clean Code. This project adapt his implementations and extends it dynamically.
