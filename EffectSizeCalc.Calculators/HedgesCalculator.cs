using EffectSizeCalc.ExcelImport;
using EffectSizeCalc.Models;

namespace EffectSizeCalc.Calculators
{
    public class HedgesCalculator : IEffectSizeCalculator
    {
        private readonly ExcelDataSet _excelDataSet;

        private readonly HedgesSettings _hedgesSettings;

        public HedgesCalculator(ExcelDataSet excelDataSet, HedgesSettings hedgesSettings)
        {
            _excelDataSet = excelDataSet;
            _hedgesSettings = hedgesSettings;
        }

        public EffectSizeResult CalculateEffectSize()
        {
            // TODO: Implement calculation!
            return null;
        }
    }
}
