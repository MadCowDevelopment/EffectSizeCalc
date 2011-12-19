using System.Windows.Input;
using EffectSizeCalc.Models;
using EffectSizeCalc.ViewModels.Framework;

namespace EffectSizeCalc.ViewModels
{
    public class ResultVM : BaseViewModel
    {
        public ResultVM(EffectSizeResult result)
        {
            Result = result;
        }

        public EffectSizeResult Result { get; private set; }

        public ICommand SaveCommand { get; set; }

        public ICommand CloseCommand { get; set; }
    }
}
