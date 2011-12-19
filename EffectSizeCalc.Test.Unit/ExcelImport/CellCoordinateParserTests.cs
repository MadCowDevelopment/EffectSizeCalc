using System;
using EffectSizeCalc.ExcelImport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EffectSizeCalc.Test.Unit.ExcelImport
{
    [TestClass]
    public class CellCoordinateParserTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [DeploymentItem(@"..\..\..\EffectSizeCalc.Test.Unit\TestData\CellCoordinateParserTestData.xlsx")]
        [DataSource("System.Data.Odbc", "Dsn=Excel Files;dbq=|DataDirectory|\\CellCoordinateParserTestData.xlsx", "Valid$", DataAccessMethod.Sequential)]
        public void TestValidValues()
        {
            var value = TestContext.DataRow["Value"].ToString();
            var expectedRow = Convert.ToInt32(TestContext.DataRow["Row"]);
            var expectedColumn = Convert.ToInt32(TestContext.DataRow["Column"]);

            var parser = CreateCellCoordinateParser();
            var coords = parser.ParseCoordinates(value);

            Assert.AreEqual(expectedColumn, coords.Column);
            Assert.AreEqual(expectedRow, coords.Row);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestEmptyArgument()
        {
            var parser = CreateCellCoordinateParser();
            parser.ParseCoordinates(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullArgument()
        {
            var parser = CreateCellCoordinateParser();
            parser.ParseCoordinates(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestWhitespaceArgument()
        {
            var parser = CreateCellCoordinateParser();
            parser.ParseCoordinates(" ");
        }

        [TestMethod]
        [DeploymentItem(@"..\..\..\EffectSizeCalc.Test.Unit\TestData\CellCoordinateParserTestData.xlsx")]
        [DataSource("System.Data.Odbc", "Dsn=Excel Files;dbq=|DataDirectory|\\CellCoordinateParserTestData.xlsx", "Invalid$", DataAccessMethod.Sequential)]
        [ExpectedException(typeof(FormatException))]
        public void TestInvalidValues()
        {
            var value = TestContext.DataRow["Value"].ToString();

            var parser = CreateCellCoordinateParser();
            parser.ParseCoordinates(value);
        }

        #region Private helpers

        private static CellCoordinateParser CreateCellCoordinateParser()
        {
            return new CellCoordinateParser();
        }

        #endregion
    }
}
