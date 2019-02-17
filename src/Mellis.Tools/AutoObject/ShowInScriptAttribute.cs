using System;

namespace Mellis.Tools.AutoObject
{
    [AttributeUsage(AttributeTargets.Field |
                    AttributeTargets.Method |
                    AttributeTargets.Property)]
    public class ShowInScriptAttribute : Attribute
    {
        public string Name;
    }
}