using EffectSizeCalc.ExcelImport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EffectSizeCalc.Test.Integration.ExcelImport
{
    [TestClass]
    public class ExcelImporterTests
    {
        [TestMethod]
        [DeploymentItem(@"..\..\..\Data\data.xlsx")]
        public void TestMethod1()
        {
            var cellCoordinateParser = new CellCoordinateParser();
            var cellValueParser = new CellValueParser();
            var cellParser = new CellParser(cellCoordinateParser, cellValueParser);
            var importer = new ExcelImporter(cellParser);

            var result = importer.GetCellValues(@"data.xlsx");

            Assert.AreEqual(15, result.TrialDataRows.Count);
            Assert.AreEqual("Vpn", result.TrialDataRows[0].Values[0]);
        }
    }
}
