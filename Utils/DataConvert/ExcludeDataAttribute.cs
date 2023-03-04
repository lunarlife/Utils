using System;

namespace Utils.DataConvert
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ExcludeDataAttribute : Attribute
    {
        public string DataName { get; }

        public ExcludeDataAttribute()
        {

        }

        public ExcludeDataAttribute(string dataName)
        {
            DataName = dataName;
        }
    }
}