using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using EffectSizeCalc.ExcelImport;

namespace EffectSizeCalc.TypeGenerator
{
    public class DynamicListGenerator : IDynamicListGenerator
    {
        private readonly ITrialResultGenerator _trialResultGenerator;

        public DynamicListGenerator(ITrialResultGenerator trialResultGenerator)
        {
            _trialResultGenerator = trialResultGenerator;
        }

        public ExpandoObject CreateDynamicData(ExcelDataSet excelDataSet)
        {
            var data = _trialResultGenerator.GenerateTrialResult(excelDataSet);

            var t = data[0].GetType();
            var list = (IList)Activator.CreateInstance((typeof(List<>).MakeGenericType(t)));

            foreach (var o in data)
            {
                list.Add(o);
            }

            dynamic result = new ExpandoObject();
            result.Data = list;

            return result;
        }
    }
}
