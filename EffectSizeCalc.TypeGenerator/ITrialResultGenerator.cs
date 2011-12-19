using System.Collections.Generic;
using EffectSizeCalc.ExcelImport;

namespace EffectSizeCalc.TypeGenerator
{
    public interface ITrialResultGenerator
    {
        List<object> GenerateTrialResult(ExcelDataSet dataSet);
    }
}