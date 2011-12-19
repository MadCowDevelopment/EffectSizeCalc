using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace EffectSizeCalc.ExcelImport
{
    public interface ICellValueParser
    {
        string GetCellValue(OpenXmlPartContainer wbPart, EnumValue<CellValues> dataType, string value);
    }
}