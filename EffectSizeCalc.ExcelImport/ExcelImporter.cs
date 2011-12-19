using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace EffectSizeCalc.ExcelImport
{
    public class ExcelImporter : IExcelImporter
    {
        private readonly ICellParser _cellParser;

        public ExcelImporter(ICellParser cellParser)
        {
            if (cellParser == null) throw new ArgumentNullException("cellParser");

            _cellParser = cellParser;
        }

        public ExcelDataSet GetCellValues(string fileName)
        {
            using (var document = SpreadsheetDocument.Open(fileName, false))
            {
                var wbPart = document.WorkbookPart;
                var sheet = wbPart.Workbook.Descendants<Sheet>().FirstOrDefault();

                if (sheet == null)
                {
                    throw new InvalidOperationException("There is no worksheet in the excel document.");
                }

                var wsPart = (WorksheetPart)(wbPart.GetPartById(sheet.Id));
                var cells = wsPart.Worksheet.Descendants<Cell>();

                var cellList = new List<ExcelSheetValue>();
                int columnCount = 0;
                int rowCount = 0;
                foreach (var cell in cells)
                {
                    var sheetValue = _cellParser.ParseCell(wbPart, cell);
                    cellList.Add(sheetValue);
                    if(sheetValue.Coordinates.Column > columnCount)
                    {
                        columnCount = sheetValue.Coordinates.Column;
                    }

                    if(sheetValue.Coordinates.Row > rowCount)
                    {
                        rowCount = sheetValue.Coordinates.Row;
                    }
                }

                var trialDataSet = new ExcelDataSet(rowCount);
                for (var i = 0; i < rowCount+1; i++)
                {
                    var row = new ExcelDataRow(columnCount+1);
                    trialDataSet.TrialDataRows.Add(row);

                    int i1 = i;
                    foreach (var excelSheetValue in cellList.Where(excelSheetValue => excelSheetValue.Coordinates.Row == i1))
                    {
                        row.Values[excelSheetValue.Coordinates.Column] = excelSheetValue.Value;
                    }
                }

                return trialDataSet;
            }
        }
    }
}