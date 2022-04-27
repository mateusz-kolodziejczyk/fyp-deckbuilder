namespace Helper
{
    public static class RangeConversionHelpers
    {
        // Made with help from https://stackoverflow.com/a/929107
        public static float RangeConversion(float oldValue, float oldMax, float oldMin, float newMax, float newMin)
        {
            var oldRange = (oldMax - oldMin);
            var newValue = float.MinValue;
            if (oldRange == 0)
                newValue = newMin;
            else
            {
                var NewRange = (newMax - newMin);
                newValue = (((oldValue - oldMin) * NewRange) / oldRange) + newMin;
            }
            return newValue;
        }
    }
}