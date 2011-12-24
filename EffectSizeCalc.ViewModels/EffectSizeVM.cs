using System.Windows.Input;

using EffectSizeCalc.Calculators;
using EffectSizeCalc.Models;
using EffectSizeCalc.ResultExporters;
using EffectSizeCalc.ViewModels.Framework;

namespace EffectSizeCalc.ViewModels
{
    public class EffectSizeVM : SimpleViewModel
    {
        private readonly IEffectSizeCalculator _cohensCalculator;
        private readonly ISaveFileService _saveFileService;
        private readonly IResultExporter _resultExporter;
        private IBaseViewModel _content;

        private ICommand _calculateCommand;

        public EffectSizeVM(
            IEffectSizeCalculator cohensCalculator, 
            ISaveFileService saveFileService, 
            IResultExporter resultExporter,
            IEffectSizeSetupVM effectSizeSetupVM)
        {
            _cohensCalculator = cohensCalculator;
            _saveFileService = saveFileService;
            _resultExporter = resultExporter;

            Content = effectSizeSetupVM;
        }

        public ICommand CalculateCommand
        {
            get
            {
                return _calculateCommand ?? (_calculateCommand = new RelayCommand(p => OnCalculateCohens()));
            }
        }

        private void OnCalculateCohens()
        {
            var result = _cohensCalculator.CalculateEffectSize();
            var resultVM = new ResultVM(result);
            resultVM.CloseCommand = CloseCommand;
            resultVM.SaveCommand = new RelayCommand(p => OnSaveResult(result));
            Content = resultVM;
        }

        private void OnSaveResult(EffectSizeResult result)
        {
            _saveFileService.Filter = "Ergebnisdatei|*.txt";
            if (_saveFileService.ShowDialog() == true)
            {
                var filename = _saveFileService.FileName;
                _resultExporter.SaveResult(result, filename);
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
