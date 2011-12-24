using System;
using System.IO;
using EffectSizeCalc.Models;

namespace EffectSizeCalc.ResultExporters
{
    public class CohensResultExporter : IResultExporter
    {
        private readonly CohensSettings _cohensSettings;

        public CohensResultExporter(CohensSettings cohensSettings)
        {
            _cohensSettings = cohensSettings;
        }

        public void SaveResult(EffectSizeResult result, string filename)
        {
            using (var streamWriter = new StreamWriter(filename))
            {
                streamWriter.WriteLine("Effektstärkeergebnisse vom {0}:", DateTime.Now);
                streamWriter.WriteLine();

                streamWriter.WriteLine("1. Variable: {0}", _cohensSettings.SelectedFirstVariable);
                streamWriter.WriteLine("2. Variable: {0}", _cohensSettings.SelectedSecondVariable);
                if (!string.IsNullOrEmpty(_cohensSettings.SelectedFilterVariable))
                {
                    streamWriter.WriteLine("Filtervariable: {0}", _cohensSettings.SelectedFilterVariable);
                    streamWriter.WriteLine("Filter: {0}", _cohensSettings.Filter);
                }

                streamWriter.Write("Gleiche Varianzen: ");
                streamWriter.WriteLine(_cohensSettings.SameVariances ? "ja" : "nein");

                streamWriter.WriteLine();

                streamWriter.WriteLine("Cohen's d: {0}", result.Result);
                streamWriter.WriteLine("Konfidenzintervall: {0} - {1}", result.ConfidenceIntervalLower,
                                       result.ConfidenceIntervalUpper);
                streamWriter.WriteLine("Mittelwert 1. Variable: {0}", result.MeanValue1);
                streamWriter.WriteLine("Mittelwert 2. Variable: {0}", result.MeanValue2);
                streamWriter.WriteLine("Standardabweichung 1. Variable: {0}", result.StandardDeviation1);
                streamWriter.WriteLine("Standardabweichung 2. Variable: {0}", result.StandardDeviation2);
            }
        }
    }
}
