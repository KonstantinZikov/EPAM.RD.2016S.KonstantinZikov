using System;

namespace Attributes
{
    // Should be applied to properties and fields.
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class StringValidatorAttribute : Attribute
    {
        public int SymbolCount { get; set; }

        public StringValidatorAttribute(int symbolCount)
        {
            SymbolCount = symbolCount;
        }
    }
}
