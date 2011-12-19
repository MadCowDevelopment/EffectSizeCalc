using System;
using EffectSizeCalc.ViewModels.Framework;

namespace EffectSizeCalc.ViewModels.Events
{
    public class ShowDialogEventArgs : EventArgs
    {
        public ISimpleViewModel ViewModel { get; private set; }

        public ShowDialogEventArgs(ISimpleViewModel viewModel)
        {
            ViewModel = viewModel;
        }
    }
}
