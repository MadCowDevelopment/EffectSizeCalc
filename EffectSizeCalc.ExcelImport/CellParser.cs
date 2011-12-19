using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace EffectSizeCalc.ExcelImport
{
    public class CellParser : ICellParser
    {
        private readonly ICellCoordinateParser _cellCoordinateParser;
        private readonly ICellValueParser _cellValueParser;

        public CellParser(ICellCoordinateParser cellCoordinateParser, ICellValueParser cellValueParser)
        {
            _cellCoordinateParser = cellCoordinateParser;
            _cellValueParser = cellValueParser;
        }

        public ExcelSheetValue ParseCell(OpenXmlPartContainer wbPart, Cell cell)
        {
            var coords = _cellCoordinateParser.ParseCoordinates(cell.CellReference);
            var value = _cellValueParser.GetCellValue(wbPart, cell.DataType, cell.InnerText);

            var result = new ExcelSheetValue(coords, value);
            return result;
        }
    }
}
