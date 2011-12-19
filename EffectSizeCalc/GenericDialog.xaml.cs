using EffectSizeCalc.ViewModels.Framework;

namespace EffectSizeCalc
{
    /// <summary>
    /// Interaction logic for GenericDialog.xaml
    /// </summary>
    public partial class GenericDialog
    {
        public GenericDialog()
        {
        }

        public GenericDialog(ISimpleViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
            viewModel.CloseRequest += ViewModelCloseRequest;
        }

        private void ViewModelCloseRequest(object sender, CloseRequestEventArgs e)
        {
            Close();
        }
    }
}
