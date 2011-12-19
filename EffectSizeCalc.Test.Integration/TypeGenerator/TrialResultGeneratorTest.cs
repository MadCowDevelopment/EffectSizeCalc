using EffectSizeCalc.ExcelImport;
using EffectSizeCalc.TypeGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EffectSizeCalc.Test.Integration.TypeGenerator
{
    [TestClass]
    [DeploymentItem(@"..\..\..\Data\data.xlsx")]
    public class TrialResultGeneratorTest
    {
        [TestMethod]
        public void Test()
        {
            var dataSet = CreateDataSet();

            var generator = new TrialResultGenerator();
            var result = generator.GenerateTrialResult(dataSet);

            Assert.AreEqual(14, result.Count);
            for (int i = 0; i < result.Count; i++)
            {
                AssertAllValuesAreEqual(dataSet.TrialDataRows[i + 1].Values, result[i]);
            }
        }

        private ExcelDataSet CreateDataSet()
        {
            var cellCoordinateParser = new CellCoordinateParser();
            var cellValueParser = new CellValueParser();
            var cellParser = new CellParser(cellCoordinateParser, cellValueParser);
            var importer = new ExcelImporter(cellParser);

            var dataSet = importer.GetCellValues(@"data.xlsx");
            return dataSet;
        }

        private void AssertAllValuesAreEqual(object[] expected, dynamic actual)
        {
            Assert.AreEqual(expected[0], actual.Vpn);
            Assert.AreEqual(expected[1], actual.Cutoff);
            Assert.AreEqual(expected[2], actual.De_zuverlässig);
            Assert.AreEqual(expected[3], actual.Tü_zuverlässig);
            Assert.AreEqual(expected[4], actual.Fächer);
            Assert.AreEqual(expected[5], actual.ZD_zuverl);
        }
    }
}
