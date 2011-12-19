
namespace EffectSizeCalc.ExcelImport
{
    public class ExcelCellCoordinates
    {
        public ExcelCellCoordinates(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public int Row { get; private set; }
        public int Column { get; private set; }
    }
}
