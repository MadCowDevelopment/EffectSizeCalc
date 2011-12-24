using System.Windows.Input;

using EffectSizeCalc.ViewModels.Framework;

namespace EffectSizeCalc.ViewModels
{
    public interface IEffectSizeSetupVM : IBaseViewModel
    {
        ICommand CalculateCommand { get; set; }
    }
}