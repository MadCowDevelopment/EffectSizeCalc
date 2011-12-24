using EffectSizeCalc.Models;

namespace EffectSizeCalc.ResultExporters
{
    public class HedgesResultExporter : IResultExporter
    {
        private readonly HedgesSettings _settings;

        public HedgesResultExporter(HedgesSettings settings)
        {
            _settings = settings;
        }

        public void SaveResult(EffectSizeResult result, string filename)
        {
            // TODO: implement saving result.
        }
    }
}
