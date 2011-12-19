using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace EffectSizeCalc.ExcelImport
{
    public interface ICellParser
    {
        ExcelSheetValue ParseCell(OpenXmlPartContainer wbPart, Cell cell);
    }
}