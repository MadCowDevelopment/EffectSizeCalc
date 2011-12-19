using System.Collections.Generic;

namespace EffectSizeCalc.ExcelImport
{
    public class ExcelDataSet
    {
        public ExcelDataSet(int rowCount)
        {
            TrialDataRows = new List<ExcelDataRow>(rowCount);
        }

        public List<ExcelDataRow> TrialDataRows { get; private set; }
    }
}
