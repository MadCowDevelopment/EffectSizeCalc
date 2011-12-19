namespace EffectSizeCalc.Models
{
    public class CohensSettings
    {
        public CohensSettings()
        {
            SameVariances = true;
        }

        public string SelectedFirstVariable { get; set; }

        public string SelectedSecondVariable { get; set; }

        public string SelectedFilterVariable { get; set; }

        public double Filter { get; set; }

        public bool SameVariances { get; set; }
    }
}
