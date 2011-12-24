using System;
using System.Windows.Input;
using EffectSizeCalc.Calculators;
using EffectSizeCalc.ExcelImport;
using EffectSizeCalc.Models;
using EffectSizeCalc.ResultExporters;
using EffectSizeCalc.TypeGenerator;
using EffectSizeCalc.ViewModels.Events;
using EffectSizeCalc.ViewModels.Framework;

namespace EffectSizeCalc.ViewModels
{
    public class MainWindowVM : SimpleViewModel
    {
        private readonly IOpenFileService _openFileService;
        private readonly ISaveFileService _saveFileService;
        private readonly IExcelImporter _excelImporter;
        private readonly IDynamicListGenerator _dynamicListGenerator;

        private ICommand _openCommand;

        private ICommand _calculateCohensCommand;

        private ICommand _calculateHedgesCommand;

        private dynamic _expando;

        private ExcelDataSet _excelDataSet;

        public MainWindowVM(
            IOpenFileService openFileService,
            ISaveFileService saveFileService,
            IExcelImporter excelImporter, 
            IDynamicListGenerator dynamicListGenerator)
        {
            _openFileService = openFileService;
            _saveFileService = saveFileService;
            _excelImporter = excelImporter;
            _dynamicListGenerator = dynamicListGenerator;

            // TODO: Unomment in release!
            OpenExcelSheet("data.xlsx");
        }

        public dynamic Expando
        {
            get
            {
                return _expando;
            }
            
            private set
            {
                _expando = value;
                RaisePropertyChanged();
            }
        }

        public ICommand OpenCommand
        {
            get
            {
                return _openCommand ?? (_openCommand = new RelayCommand(p => OnOpen()));
            }
        }

        private void OnOpen()
        {
            _openFileService.Filter = "Excel 2010 sheet|*.xlsx";
            if (_openFileService.ShowDialog() == true)
            {
                OpenExcelSheet(_openFileService.FileName);
            }
        }

        private void OpenExcelSheet(string filename)
        {
            _excelDataSet = _excelImporter.GetCellValues(filename);
            Expando = _dynamicListGenerator.CreateDynamicData(_excelDataSet);
        }

        public event EventHandler<ShowDialogEventArgs> DialogRequest;

        public void RaiseDialogRequested(ShowDialogEventArgs e)
        {
            var handler = DialogRequest;
            if (handler != null) handler(this, e);
        }

        public ICommand CalculateCohensCommand
        {
            get
            {
                return _calculateCohensCommand ??
                       (_calculateCohensCommand = new RelayCommand(p => OnCalculateCohens(), p => Expando != null));
            }
        }

        public ICommand CalculateHedgesCommand
        {
            get
            {
                return _calculateHedgesCommand ??
                       (_calculateHedgesCommand = new RelayCommand(p => OnCalculateHedges(), p => Expando != null));
            }
        }

        private void OnCalculateCohens()
        {
            var settings = new CohensSettings();
            var effectSizeCalculator = new CohensCalculator(_excelDataSet, settings);
            var resultExporter = new CohensResultExporter(settings);
            var cohensSetupVM = new CohensSetupVM(_excelDataSet, settings);

            ShowEffectSizeWindow(effectSizeCalculator, resultExporter, cohensSetupVM);
        }

        private void OnCalculateHedges()
        {
            var settings = new HedgesSettings();
            var effectSizeCalculator = new HedgesCalculator(_excelDataSet, settings);
            var resultExporter = new HedgesResultExporter(settings);
            var hedgesSetupVM = new HedgesSetupVM(_excelDataSet, settings);

            ShowEffectSizeWindow(effectSizeCalculator, resultExporter, hedgesSetupVM);
        }

        private void ShowEffectSizeWindow(
            IEffectSizeCalculator effectSizeCalculator, 
            IResultExporter resultExporter,
            IEffectSizeSetupVM hedgesSetupVM)
        {
            var effectSizeVM = new EffectSizeVM(
                effectSizeCalculator, _saveFileService, resultExporter, hedgesSetupVM);
            hedgesSetupVM.CalculateCommand = effectSizeVM.CalculateCommand;
            RaiseDialogRequested(new ShowDialogEventArgs(effectSizeVM));
        }
    }
}
