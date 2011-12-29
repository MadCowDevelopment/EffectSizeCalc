namespace EffectSizeCalc.ExcelImport
{
    public interface IColumnReducer
    {
        void ReduceToColumnsWithDoubleValues(ExcelDataSet excelDataSet);
    }
}