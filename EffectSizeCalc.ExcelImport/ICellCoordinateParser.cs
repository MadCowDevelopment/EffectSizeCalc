namespace EffectSizeCalc.ExcelImport
{
    public interface ICellCoordinateParser
    {
        ExcelCellCoordinates ParseCoordinates(string value);
    }
}