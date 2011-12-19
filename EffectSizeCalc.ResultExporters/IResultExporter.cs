using EffectSizeCalc.Models;

namespace EffectSizeCalc.ResultExporters
{
    public interface IResultExporter
    {
        void SaveResult(EffectSizeResult result, CohensSettings settings, string filename);
    }
}