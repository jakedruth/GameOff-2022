namespace JDR.Utils
{
    [System.Serializable]
    public struct RangedFloat
    {
        public float minimum;
        public float maximum;

        public RangedFloat(float minimum, float maximum)
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }

        public float Mean
        {
            get { return (minimum + maximum) / 2f; }
        }

        public float Sum
        {
            get { return maximum + minimum; }
        }

        public float Difference
        {
            get { return maximum - minimum; }
        }

        public float Product
        {
            get { return minimum * maximum; }
        }

        public float GetRandomValue()
        {
            return UnityEngine.Random.Range(minimum, maximum);
        }

        public float Clamp(float value)
        {
            if (value < minimum)
                value = minimum;
            else if (value > maximum)
                value = maximum;

            return value;
        }

        public float Lerp(float t)
        {
            // V = A(1 - t) + Bt
            return minimum * (1f - t) + maximum * t;
        }

        public float InverseLerp(float value)
        {
            // V = A(1 - t) + Bt
            // V = A + -At + Bt
            // V - A = -At + Bt
            // (V - A) = t(-A + B)
            // (V - A) / (B - A) = t
            return (value - minimum) / (maximum - minimum);
        }

        public bool IsInRange(float value)
        {
            return value >= minimum && value <= maximum;
        }

        public static float MapValue(float value, RangedFloat inRange, RangedFloat outRange, bool clampOutputToOutRange = false)
        {
            return MapValue(value, inRange.minimum, inRange.maximum, outRange.minimum, outRange.maximum, clampOutputToOutRange);
        }

        public static float MapValue(float value, float inMin, float inMax, float outMin, float outMax, bool clampOutputToOutRange = false)
        {
            float t = (value - inMin) / (inMax - inMin);
            float output = outMin * (1f - t) + outMax * t;
            if (clampOutputToOutRange)
                return UnityEngine.Mathf.Clamp(output, outMin, outMax);
            return output;
        }
    }
}