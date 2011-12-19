using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace EffectSizeCalc.ExcelImport
{
    public class CellValueParser : ICellValueParser
    {
        public string GetCellValue(OpenXmlPartContainer wbPart, EnumValue<CellValues> dataType, string value)
        {
            if (dataType != null)
            {
                switch (dataType.Value)
                {
                    case CellValues.SharedString:
                        var stringTable = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                        if (stringTable != null)
                        {
                            value = stringTable.SharedStringTable.ElementAt(int.Parse(value)).InnerText;
                        }
                        break;

                    case CellValues.Boolean:
                        switch (value)
                        {
                            case "0":
                                value = "False";
                                break;
                            default:
                                value = "True";
                                break;
                        }
                        break;
                }

            }

            return value;
        }
    }
}
