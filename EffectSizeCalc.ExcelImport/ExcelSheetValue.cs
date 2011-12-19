
namespace EffectSizeCalc.ExcelImport
{
    public class ExcelSheetValue
    {
        public ExcelSheetValue(ExcelCellCoordinates coordinates, string value)
        {
            Coordinates = coordinates;
            Value = value;
        }

        public ExcelCellCoordinates Coordinates { get; private set; }
        public string Value { get; private set; }
    }
}
