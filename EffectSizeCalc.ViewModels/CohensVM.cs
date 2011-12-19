using EffectSizeCalc.Calculators;
using EffectSizeCalc.ExcelImport;
using EffectSizeCalc.Models;
using EffectSizeCalc.ResultExporters;
using EffectSizeCalc.ViewModels.Framework;

namespace EffectSizeCalc.ViewModels
{
    public class CohensVM : SimpleViewModel
    {
        private readonly ICohensCalculator _cohensCalculator;
        private readonly ISaveFileService _saveFileService;
        private readonly IResultExporter _cohensResultExporter;
        private readonly ExcelDataSet _excelDataSet;
        private IBaseViewModel _content;

        private readonly CohensSettings _cohensSettings;

        public CohensVM(
            ICohensCalculator cohensCalculator, 
            ISaveFileService saveFileService, 
            IResultExporter cohensResultExporter,
            ExcelDataSet excelDataSet)
        {
            _cohensCalculator = cohensCalculator;
            _saveFileService = saveFileService;
            _cohensResultExporter = cohensResultExporter;
            _excelDataSet = excelDataSet;

            _cohensSettings = new CohensSettings();
            var cohensSetupVM = new CohensSetupVM(excelDataSet, _cohensSettings);
            cohensSetupVM.CalculateCommand = new RelayCommand(p => OnCalculateCohens());
            Content = cohensSetupVM;
        }

        private void OnCalculateCohens()
        {
            var result = _cohensCalculator.Calculate(_cohensSettings, _excelDataSet);
            var cohensResultVM = new ResultVM(result);
            cohensResultVM.CloseCommand = CloseCommand;
            cohensResultVM.SaveCommand = new RelayCommand(p => OnSaveResult(result));
            Content = cohensResultVM;
        }

        private void OnSaveResult(EffectSizeResult result)
        {
            _saveFileService.Filter = "Ergebnisdatei|*.txt";
            if (_saveFileService.ShowDialog() == true)
            {
                var filename = _saveFileService.FileName;
                _cohensResultExporter.SaveResult(result, _cohensSettings, filename);
            }
        }

        public IBaseViewModel Content
        {
            get
            {
                return _content;
            }

            set
            {
                _content = value;
                RaisePropertyChanged();
            }
        }
    }
}
