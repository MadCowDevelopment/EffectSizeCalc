using System.Collections.Generic;

namespace EffectSizeCalc.ExcelImport
{
    public class ColumnReducer : IColumnReducer
    {
        public void ReduceToColumnsWithDoubleValues(ExcelDataSet excelDataSet)
        {
            var indicesOfColumnsToRemove = FindIndicesOfColumnsThatContainValuesOtherThanDouble(excelDataSet);
            RemoveColumnsWithIndices(indicesOfColumnsToRemove, excelDataSet);
        }

        private void RemoveColumnsWithIndices(List<int> indicesOfColumnsToRemove, ExcelDataSet excelDataSet)
        {
            foreach (var trialDataRow in excelDataSet.TrialDataRows)
            {
                var values = new string[trialDataRow.Values.Length - indicesOfColumnsToRemove.Count];
                int currentIndexInNewArray = 0;
                for (int i = 0; i < trialDataRow.Values.Length; i++)
                {
                    if (indicesOfColumnsToRemove.Contains(i))
                    {
                        continue;
                    }

                    values[currentIndexInNewArray++] = trialDataRow.Values[i];
                }

                trialDataRow.Values = values;
            }
        }

        private List<int> FindIndicesOfColumnsThatContainValuesOtherThanDouble(ExcelDataSet excelDataSet)
        {
            var indicesOfColumnsToRemove = new List<int>();

            for (int i = 1; i < excelDataSet.TrialDataRows.Count; i++)
            {
                for (int k = 0; k < excelDataSet.TrialDataRows[i].Values.Length; k++)
                {
                    double doubleValue;
                    if (!double.TryParse(excelDataSet.TrialDataRows[i].Values[k], out doubleValue) &&
                        excelDataSet.TrialDataRows[i].Values[k] != null)
                    {
                        if (!indicesOfColumnsToRemove.Contains(k))
                        {
                            indicesOfColumnsToRemove.Add(k);
                        }
                    }
                }
            }

            return indicesOfColumnsToRemove;
        }
    }
}
