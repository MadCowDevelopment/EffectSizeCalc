using EffectSizeCalc.ExcelImport;
using EffectSizeCalc.Models;

namespace EffectSizeCalc.Calculators
{
    public interface ICohensCalculator
    {
        EffectSizeResult Calculate(CohensSettings cohensSettings, ExcelDataSet excelDataSet);
    }
}