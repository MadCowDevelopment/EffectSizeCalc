using System.Dynamic;
using EffectSizeCalc.ExcelImport;

namespace EffectSizeCalc.TypeGenerator
{
    public interface IDynamicListGenerator
    {
        ExpandoObject CreateDynamicData(ExcelDataSet excelDataSet);
    }
}