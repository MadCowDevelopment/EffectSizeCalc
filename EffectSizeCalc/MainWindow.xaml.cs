using EffectSizeCalc.ExcelImport;
using EffectSizeCalc.TypeGenerator;
using EffectSizeCalc.ViewModels;
using EffectSizeCalc.ViewModels.Framework;

namespace EffectSizeCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private MainWindowVM _mainWindowVM;

        public MainWindow()
        {
            InitializeComponent();
            SetDataContext();
        }

        private void SetDataContext()
        {
            var openFileService = CreateOpenFileService();
            var saveFileService = CreateSaveFileService();
            var excelImporter = CreateExcelImporter();
            var dynamicListGenerator = CreateDynamicListGenerator();
            _mainWindowVM = new MainWindowVM(openFileService, saveFileService, excelImporter, dynamicListGenerator);

            _mainWindowVM.CloseRequest += MainWindowVMCloseRequest;
            _mainWindowVM.DialogRequest += MainWindowVMDialogRequested;

            DataContext = _mainWindowVM;
        }

        private static ISaveFileService CreateSaveFileService()
        {
            return new WpfSaveFileService();
        }

        private static void MainWindowVMDialogRequested(object sender, ViewModels.Events.ShowDialogEventArgs e)
        {
            var genericDialog = new GenericDialog(e.ViewModel);
            genericDialog.ShowDialog();
        }

        private void MainWindowVMCloseRequest(object sender, CloseRequestEventArgs e)
        {
            Close();
        }

        private static IOpenFileService CreateOpenFileService()
        {
            return new WpfOpenFileService();
        }

        private static IExcelImporter CreateExcelImporter()
        {
            var cellCoordinateParser = new CellCoordinateParser();
            var cellValueParser = new CellValueParser();
            var cellParser = new CellParser(cellCoordinateParser, cellValueParser);
            var excelImporter = new ExcelImporter(cellParser);
            return excelImporter;
        }
        
        private static IDynamicListGenerator CreateDynamicListGenerator()
        {
            var trialResultGenerator = new TrialResultGenerator();
            return new DynamicListGenerator(trialResultGenerator);
        }
    }
}
