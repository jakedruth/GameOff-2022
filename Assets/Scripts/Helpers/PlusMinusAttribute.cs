using UnityEngine;

namespace JDR.Utils
{
    public class PlusMinusAttribute : PropertyAttribute
    {
        public int clampMin;
        public int clampMax;

        public PlusMinusAttribute(int min = int.MinValue, int max = int.MaxValue)
        {
            clampMin = min;
            clampMax = max;
        }
    }
}
