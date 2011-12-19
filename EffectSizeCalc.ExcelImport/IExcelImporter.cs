using System.Collections.Generic;

namespace EffectSizeCalc.ExcelImport
{
    public interface IExcelImporter
    {
        ExcelDataSet GetCellValues(string fileName);
    }
}