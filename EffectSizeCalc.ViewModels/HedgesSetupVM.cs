using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using EffectSizeCalc.ExcelImport;
using EffectSizeCalc.Models;
using EffectSizeCalc.ViewModels.Framework;

namespace EffectSizeCalc.ViewModels
{
    public class HedgesSetupVM : BaseViewModel, IEffectSizeSetupVM
    {
        private readonly ExcelDataSet _excelDataSet;

        private readonly HedgesSettings _settings;

        private ICommand _calculateCommand;

        private ObservableCollection<string> _availableFirstValues;

        private ObservableCollection<string> _availableSecondValues; 

        public HedgesSetupVM(ExcelDataSet excelDataSet, HedgesSettings settings)
        {
            _excelDataSet = excelDataSet;
            _settings = settings;

            AvailableVariables =
                new ObservableCollection<string>(excelDataSet.TrialDataRows[0].Values.Select(p => p.ToString()));
            AvailableVariables.Insert(0, string.Empty);

            AvailableGroups =
                new ObservableCollection<string>(excelDataSet.TrialDataRows[0].Values.Select(p => p.ToString()));
            AvailableGroups.Insert(0, string.Empty);

            AvailableFirstValues = new ObservableCollection<string>();
            AvailableSecondValues = new ObservableCollection<string>();
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

        public string SelectedVariable
        {
            get
            {
                return _settings.SelectedVariable;
            }

            set
            {
                _settings.SelectedVariable = value;
                RaisePropertyChanged(() => SelectedVariable);
            }
        }

        public string SelectedGroup
        {
            get
            {
                return _settings.SelectedGroup;
            }

            set
            {
                _settings.SelectedGroup = value;
                RaisePropertyChanged(() => SelectedGroup);

                if (string.IsNullOrEmpty(_settings.SelectedGroup))
                {
                    AvailableFirstValues = new ObservableCollection<string>();
                    AvailableSecondValues = new ObservableCollection<string>();
                }
                else
                {
                    int indexOfSelectedGroup = -1;
                    for (int i = 0; i < _excelDataSet.TrialDataRows[0].Values.Length; i++)
                    {
                        if (_excelDataSet.TrialDataRows[0].Values[i] == SelectedGroup)
                        {
                            indexOfSelectedGroup = i;
                            break;
                        }
                    }

                    if(indexOfSelectedGroup == -1)
                    {
                        throw new InvalidOperationException("Couldn't find column index for selected group.");
                    }

                    AvailableFirstValues =
                        new ObservableCollection<string>(
                            _excelDataSet.TrialDataRows.Select(p => p.Values[indexOfSelectedGroup]));
                    AvailableFirstValues.Insert(0, string.Empty);
                }
            }
        }

        public double SelectedFirstValue
        {
            get
            {
                return _settings.SelectedFirstValue;
            }

            set
            {
                _settings.SelectedFirstValue = value;
                RaisePropertyChanged(() => SelectedFirstValue);
            }
        }

        public double SelectedSecondValue
        {
            get
            {
                return _settings.SelectedSecondValue;
            }

            set
            {
                _settings.SelectedSecondValue = value;
                RaisePropertyChanged(() => SelectedSecondValue);
            }
        }

        public ObservableCollection<string> AvailableVariables { get; private set; }
        public ObservableCollection<string> AvailableGroups { get; private set; }

        public ObservableCollection<string> AvailableFirstValues
        {
            get
            {
                return _availableFirstValues;
            }
            
            private set
            {
                _availableFirstValues = value;
                RaisePropertyChanged(() => AvailableFirstValues);
            }
        }

        public ObservableCollection<string> AvailableSecondValues
        {
            get
            {
                return _availableSecondValues;
            }

            set
            {
                _availableSecondValues = value;
                RaisePropertyChanged(() => AvailableSecondValues);
            }
        }
    }
}