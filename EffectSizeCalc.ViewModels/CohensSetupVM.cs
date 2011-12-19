using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using EffectSizeCalc.ExcelImport;
using EffectSizeCalc.Models;
using EffectSizeCalc.ViewModels.Framework;

namespace EffectSizeCalc.ViewModels
{
    public class CohensSetupVM : BaseViewModel
    {
        private readonly CohensSettings _cohensSettings;

        private ICommand _calculateCommand;

        public CohensSetupVM(ExcelDataSet excelDataSet, CohensSettings cohensSettings)
        {
            _cohensSettings = cohensSettings;
            
            AvailableFirstVariables =
                new ObservableCollection<string>(excelDataSet.TrialDataRows[0].Values.Select(p => p.ToString()));
            AvailableFirstVariables.Insert(0, string.Empty);

            AvailableSecondVariables =
                new ObservableCollection<string>(excelDataSet.TrialDataRows[0].Values.Select(p => p.ToString()));
            AvailableSecondVariables.Insert(0, string.Empty);

            AvailableFilterVariables =
                new ObservableCollection<string>(excelDataSet.TrialDataRows[0].Values.Select(p => p.ToString()));
            AvailableFilterVariables.Insert(0, string.Empty);
        }

        public ICommand CalculateCommand
        {
            get
            {
                return _calculateCommand;
            }

            set
            {
                _calculateCommand = value;
                RaisePropertyChanged();
            }
        }

        public bool SameVariances
        {
            get
            {
                return _cohensSettings.SameVariances;
            }

            set
            {
                _cohensSettings.SameVariances = value;
                RaisePropertyChanged();
            }
        }

        public double FilterText
        {
            get
            {
                return _cohensSettings.Filter;
            }

            set
            {
                _cohensSettings.Filter = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedFirstVariable
        {
            get
            {
                return _cohensSettings.SelectedFirstVariable;
            }

            set
            {
                _cohensSettings.SelectedFirstVariable = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedSecondVariable
        {
            get
            {
                return _cohensSettings.SelectedSecondVariable;
            }

            set
            {
                _cohensSettings.SelectedSecondVariable = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedFilterVariable
        {
            get
            {
                return _cohensSettings.SelectedFilterVariable;
            }

            set
            {
                _cohensSettings.SelectedFilterVariable = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<string> AvailableFirstVariables { get; private set; }
        public ObservableCollection<string> AvailableSecondVariables { get; private set; }
        public ObservableCollection<string> AvailableFilterVariables { get; private set; } 
    }
}
