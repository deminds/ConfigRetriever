﻿using System;

namespace GH.DD.ConfigRetriever.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ConfigRetrieverIgnoreAttribute : Attribute
    {
    }
}