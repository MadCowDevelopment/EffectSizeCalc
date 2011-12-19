
using System;
using System.Globalization;
using System.Linq;

namespace EffectSizeCalc.ExcelImport
{
    public class CellCoordinateParser : ICellCoordinateParser
    {
        public ExcelCellCoordinates ParseCoordinates(string cellReference)
        {
            if(string.IsNullOrWhiteSpace(cellReference))
            {
                throw new ArgumentNullException("cellReference");
            }

            var i = 0;
            try
            {
                while (char.IsLetter(cellReference[i]))
                {
                    i++;
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new FormatException();
            }

            var columnString = cellReference.Substring(0, i);
            columnString = columnString.ToUpper(CultureInfo.InvariantCulture);
            CheckColumnString(columnString);
            
            var rowString = cellReference.Substring(i);
            CheckRowString(rowString);

            var column = ParseColumnString(columnString);
            var row = int.Parse(rowString);
            row--;

            return new ExcelCellCoordinates(row, column);
        }

        private static int ParseColumnString(string value)
        {
            var result = 0;

            var chars = value.ToCharArray();
            for (var i = 0; i < chars.Length; i++)
            {
                var charValue = value[i] - 64;
                var posValue = charValue* (int)Math.Pow(26, chars.Length - i - 1);
                result += posValue;
            }

            result--;
            return result;
        }

        private static void CheckColumnString(string columnString)
        {
            if(string.IsNullOrWhiteSpace(columnString))
            {
                throw new FormatException();
            }

            if (columnString.ToCharArray().Any(c => !char.IsLetter(c) || c < 65 || c > 90))
            {
                throw new FormatException();
            }
        }

        private static void CheckRowString(string rowString)
        {
            if(string.IsNullOrWhiteSpace(rowString))
            {
                throw new FormatException();
            }

            if(rowString[0] == '0')
            {
                throw new FormatException();
            }

            if (rowString.ToCharArray().Any(c => !char.IsNumber(c)))
            {
                throw new FormatException(rowString);
            }
        }
    }
}
