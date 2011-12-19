using System;
using System.Windows.Input;
using EffectSizeCalc.Calculators;
using EffectSizeCalc.ExcelImport;
using EffectSizeCalc.ResultExporters;
using EffectSizeCalc.TypeGenerator;
using EffectSizeCalc.ViewModels.Events;
using EffectSizeCalc.ViewModels.Framework;

namespace EffectSizeCalc.ViewModels
{
    public class MainWindowVM : SimpleViewModel
    {
        private readonly ICohensCalculator _cohensCalculator;
        private readonly IOpenFileService _openFileService;
        private readonly ISaveFileService _saveFileService;
        private readonly IExcelImporter _excelImporter;
        private readonly IDynamicListGenerator _dynamicListGenerator;
        private readonly IResultExporter _resultExporter;

        private ICommand _openCommand;

        private ICommand _calculateCohensCommand;

        private ICommand _calculateHedgesCommand;

        private dynamic _expando;

        private ExcelDataSet _excelDataSet;

        public MainWindowVM(
            ICohensCalculator cohensCalculator,
            IOpenFileService openFileService,
            ISaveFileService saveFileService,
            IExcelImporter excelImporter, 
            IDynamicListGenerator dynamicListGenerator,
            IResultExporter resultExporter)
        {
            _cohensCalculator = cohensCalculator;
            _openFileService = openFileService;
            _saveFileService = saveFileService;
            _excelImporter = excelImporter;
            _dynamicListGenerator = dynamicListGenerator;
            _resultExporter = resultExporter;

            //OpenExcelSheet("data.xlsx");
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
            RaiseDialogRequested(
                new ShowDialogEventArgs(new CohensVM(_cohensCalculator, _saveFileService, _resultExporter, _excelDataSet)));
        }

        private void OnCalculateHedges()
        {

        }
    }
}
