using System;

namespace JDR.Utils
{
    public class MinMaxRangeAttribute : Attribute
    {
        public MinMaxRangeAttribute(float minIN, float maxIN)
        {
            Min = minIN;
            Max = maxIN;
        }
        public float Min { get; private set; }
        public float Max { get; private set; }
    }
}
