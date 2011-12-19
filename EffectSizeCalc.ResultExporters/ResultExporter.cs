using System;
using System.IO;
using EffectSizeCalc.Models;

namespace EffectSizeCalc.ResultExporters
{
    public class ResultExporter : IResultExporter
    {
        public void SaveResult(EffectSizeResult result, CohensSettings settings, string filename)
        {
            using (var streamWriter = new StreamWriter(filename))
            {
                streamWriter.WriteLine("Effektstärkeergebnisse vom {0}:", DateTime.Now);
                streamWriter.WriteLine();

                streamWriter.WriteLine("1. Variable: {0}", settings.SelectedFirstVariable);
                streamWriter.WriteLine("2. Variable: {0}", settings.SelectedSecondVariable);
                if(!string.IsNullOrEmpty(settings.SelectedFilterVariable))
                {
                    streamWriter.WriteLine("Filtervariable: {0}", settings.SelectedFilterVariable);
                    streamWriter.WriteLine("Filter: {0}", settings.Filter);
                }

                streamWriter.Write("Gleiche Varianzen: ");
                streamWriter.WriteLine(settings.SameVariances ? "ja" : "nein");

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
