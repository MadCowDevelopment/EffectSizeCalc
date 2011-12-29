using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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

            var availableVariableNames = GetAvailableVariableNames().ToList();
            AvailableVariables = new ObservableCollection<string>(availableVariableNames);
            AvailableGroups = new ObservableCollection<string>(availableVariableNames);

            AvailableFirstValues = new ObservableCollection<string>();
            AvailableSecondValues = new ObservableCollection<string>();
        }

        private IEnumerable<string> GetAvailableVariableNames()
        {
            var result =
                new ObservableCollection<string>(
                    _excelDataSet.TrialDataRows[0].Values.Select(p => p.ToString(CultureInfo.InvariantCulture)));
            result.Insert(0, string.Empty);

            return result;
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
                    SelectedFirstValue = 0;
                    SelectedSecondValue = 0;
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

                    if (indexOfSelectedGroup == -1)
                    {
                        throw new InvalidOperationException("Couldn't find column index for selected group.");
                    }

                    var valuesInSelectedGroup = GetDistinctValuesInSelectedGroup(indexOfSelectedGroup).ToList();

                    AvailableFirstValues = new ObservableCollection<string>(valuesInSelectedGroup);
                    AvailableSecondValues = new ObservableCollection<string>(valuesInSelectedGroup);
                }
            }
        }

        private IEnumerable<string> GetDistinctValuesInSelectedGroup(int indexOfSelectedGroup)
        {
            var result = _excelDataSet.TrialDataRows.Select(p => p.Values[indexOfSelectedGroup]).Distinct().ToList();
            result.RemoveAt(0);
            result.Insert(0, string.Empty);

            return result;
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