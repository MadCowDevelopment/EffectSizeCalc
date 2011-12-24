using EffectSizeCalc.Models;

namespace EffectSizeCalc.ResultExporters
{
    public interface IResultExporter
    {
        void SaveResult(EffectSizeResult result, string filename);
    }
}