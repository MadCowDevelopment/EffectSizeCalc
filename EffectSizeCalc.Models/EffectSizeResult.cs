
namespace EffectSizeCalc.Models
{
    public class EffectSizeResult
    {
        public double Result { get; set; }

        public double ConfidenceIntervalLower { get; set; }

        public double ConfidenceIntervalUpper { get; set; }

        public double MeanValue1 { get; set; }

        public double MeanValue2 { get; set; }

        public double StandardDeviation1 { get; set; }

        public double StandardDeviation2 { get; set; }
    }
}
