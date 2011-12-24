using System;
using System.Collections.Generic;
using System.Linq;
using EffectSizeCalc.ExcelImport;
using EffectSizeCalc.Models;

namespace EffectSizeCalc.Calculators
{
    public class CohensCalculator : IEffectSizeCalculator
    {
        private readonly ExcelDataSet _excelDataSet;

        private readonly CohensSettings _cohensSettings;

        public CohensCalculator(ExcelDataSet excelDataSet, CohensSettings cohensSettings)
        {
            _excelDataSet = excelDataSet;
            _cohensSettings = cohensSettings;
        }

        public EffectSizeResult CalculateEffectSize()
        {
            var result = new EffectSizeResult();

            var firstVariableValues = GetVariableValues(_cohensSettings.SelectedFirstVariable, _excelDataSet);
            var secondVariableValues = GetVariableValues(_cohensSettings.SelectedSecondVariable, _excelDataSet);
            var filterVariableValues = GetFilterVariableValues(_cohensSettings, _excelDataSet);

            RemoveEmptyVariables(firstVariableValues, secondVariableValues, filterVariableValues);

            if (filterVariableValues != null)
            {
                ApplyFilterToVariables(_cohensSettings.Filter, firstVariableValues, secondVariableValues, filterVariableValues);
            }

            result.MeanValue1 = CalculateMeanValue(firstVariableValues);
            result.MeanValue2 = CalculateMeanValue(secondVariableValues);

            var sigma = CalculateSigma(_cohensSettings.SameVariances, firstVariableValues, secondVariableValues, result);

            result.Result = (result.MeanValue1 - result.MeanValue2) / sigma;

            var correlation = CalculateCorrelation(firstVariableValues, secondVariableValues);
            var standardError = CalculateStandardError(result.Result, correlation, firstVariableValues.Count);

            result.ConfidenceIntervalLower = result.Result - standardError * 1.96;
            result.ConfidenceIntervalUpper = result.Result + standardError * 1.96;

            return result;
        }

        private double CalculateCorrelation(List<double> firstVariableValues, List<double> secondVariableValues)
        {
            var meanValue1 = CalculateMeanValue(firstVariableValues);
            var meanValue2 = CalculateMeanValue(secondVariableValues);

            double nominator = 0;
            for (int i = 0; i < firstVariableValues.Count; i++)
            {
                nominator += ((firstVariableValues[i] - meanValue1) * (secondVariableValues[i] - meanValue2));
            }

            double factor1 = 0;
            double factor2 = 0;
            for (int i = 0; i < firstVariableValues.Count; i++)
            {
                factor1 += Math.Pow((firstVariableValues[i] - meanValue1), 2);
                factor2 += Math.Pow((secondVariableValues[i] - meanValue2), 2);
            }

            double denominator = Math.Sqrt(factor1) * Math.Sqrt(factor2);

            double correlation = nominator / denominator;
            return correlation;
        }

        private double CalculateStandardError(double d, double correlation, int count)
        {
            double summand1 = Math.Pow(d, 2) / (2 * (count - 1));
            double summand2 = (2 * 1 - correlation) / count;

            double sum = summand1 + summand2;
            var standardError = Math.Sqrt(sum);
            return standardError;
        }

        private double CalculateSigma(
            bool sameVariances,
            List<double> firstVariableValues,
            List<double> secondVariableValues,
            EffectSizeResult effectSizeResult)
        {
            double sigma;

            if (sameVariances)
            {
                sigma = CalculateVariance(firstVariableValues);
                sigma = Math.Sqrt(sigma);
            }
            else
            {
                var variance1 = CalculateVariance(firstVariableValues);
                effectSizeResult.StandardDeviation1 = Math.Sqrt(variance1);
                var variance2 = CalculateVariance(secondVariableValues);
                effectSizeResult.StandardDeviation2 = Math.Sqrt(variance2);
                var summand1 = (firstVariableValues.Count - 1) * variance1;
                var summand2 = (secondVariableValues.Count - 1) * variance2;

                var sum = summand1 + summand2;

                var bla = sum / (firstVariableValues.Count + secondVariableValues.Count);
                sigma = Math.Sqrt(bla);
            }

            return sigma;
        }

        private double CalculateVariance(List<double> variableValues)
        {
            var meanValue = CalculateMeanValue(variableValues);
            double sum = 0;
            foreach (var variableValue in variableValues)
            {
                sum += Math.Pow((variableValue - meanValue), 2);
            }

            var result = sum / (variableValues.Count - 1);
            return result;
        }

        private double CalculateMeanValue(List<double> variableValues)
        {
            double sum = 0;

            foreach (var firstVariableValue in variableValues)
            {
                sum += firstVariableValue;
            }

            var mean = sum / variableValues.Count;
            return mean;
        }

        private void ApplyFilterToVariables(
            double filter,
            List<double> firstVariableValues,
            List<double> secondVariableValues,
            List<double> filterVariableValues)
        {
            for (int i = filterVariableValues.Count - 1; i >= 0; i--)
            {
                if (filterVariableValues[i] != filter)
                {
                    firstVariableValues.RemoveAt(i);
                    secondVariableValues.RemoveAt(i);
                }
            }
        }

        private List<double> GetFilterVariableValues(CohensSettings cohensSettings, ExcelDataSet excelDataSet)
        {
            List<double> filterVariableValues = null;
            if (!string.IsNullOrEmpty(cohensSettings.SelectedFilterVariable))
            {
                filterVariableValues = GetVariableValues(cohensSettings.SelectedFilterVariable, excelDataSet);
            }

            return filterVariableValues;
        }

        private void RemoveEmptyVariables(
            List<double> firstVariableValues,
            List<double> secondVariableValues,
            List<double> filterVariableValues)
        {
            for (int i = firstVariableValues.Count - 1; i >= 0; i--)
            {
                if (double.IsNaN(firstVariableValues[i]) || double.IsNaN(secondVariableValues[i]))
                {
                    firstVariableValues.RemoveAt(i);
                    secondVariableValues.RemoveAt(i);

                    if (filterVariableValues != null)
                    {
                        filterVariableValues.RemoveAt(i);
                    }
                }
            }
        }

        private List<double> GetVariableValues(string selectedFilterVariable, ExcelDataSet excelDataSet)
        {
            if (string.IsNullOrEmpty(selectedFilterVariable))
            {
                return null;
            }

            var result = new List<double>();
            var indexOfColumn = excelDataSet.TrialDataRows[0].Values.ToList().IndexOf(selectedFilterVariable);

            for (int i = 1; i < excelDataSet.TrialDataRows.Count; i++)
            {
                var value = excelDataSet.TrialDataRows[i].Values[indexOfColumn] ?? string.Empty;

                if (value == string.Empty)
                {
                    result.Add(double.NaN);
                }
                else
                {
                    double doubleValue;
                    if (double.TryParse(value, out doubleValue))
                    {
                        result.Add(doubleValue);
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            "The provided value cannot be converted to double precision number.");
                    }
                }
            }

            return result;
        }
    }
}
