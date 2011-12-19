namespace EffectSizeCalc.ExcelImport
{
    public class ExcelDataRow
    {
        public ExcelDataRow(int columnCount)
        {
            Values = new string[columnCount];
        }

        public int RowNumber { get; set; }
        public string[] Values { get; private set; }
    }
}
