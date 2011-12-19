using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using EffectSizeCalc.ExcelImport;

namespace EffectSizeCalc.TypeGenerator
{
    public class TrialResultGenerator : ITrialResultGenerator
    {
        public List<object> GenerateTrialResult(ExcelDataSet dataSet)
        {
            var result = new List<dynamic>();

            var generetedType = GenerateType(dataSet.TrialDataRows[0].Values);

            for (var i = 1; i < dataSet.TrialDataRows.Count; i++)
            {
                var generatedObject = GenerateObject(generetedType, dataSet.TrialDataRows[i].Values);
                result.Add(generatedObject);
            }

            return result;
        }

        private static object GenerateObject(Type type, object[] values)
        {
            var generatedObject = Activator.CreateInstance(type);
            var properties = type.GetProperties();

            for (var i = 0; i < values.Length; i++)
            {
                string value = null;
                if (values[i] != null)
                {
                    value = values[i].ToString();
                }

                properties[i].SetValue(generatedObject, value, null);
            }

            return generatedObject;
        }

        private static Type GenerateType(object[] properties)
        {
            var assemblyName = new AssemblyName();
            assemblyName.Name = "EffectSizeCalc.Dynamic";

            var assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var module = assemblyBuilder.DefineDynamicModule("dynamicModule");

            var typeBuilder = module.DefineType("TrialResult", TypeAttributes.Public | TypeAttributes.Class);

            foreach (var value in properties)
            {
                string propertyName = value.ToString();

                var field = typeBuilder.DefineField("_" + propertyName, typeof(string), FieldAttributes.Private);
                var property = typeBuilder.DefineProperty(propertyName, PropertyAttributes.None, typeof(string), new[] { typeof(string) });
                var getSetAttr = MethodAttributes.Public | MethodAttributes.HideBySig;

                // Define the "get" accessor.
                var currGetPropMthdBldr = typeBuilder.DefineMethod("get_value", getSetAttr, typeof(string), Type.EmptyTypes);
                var currGetIL = currGetPropMthdBldr.GetILGenerator();
                currGetIL.Emit(OpCodes.Ldarg_0);
                currGetIL.Emit(OpCodes.Ldfld, field);
                currGetIL.Emit(OpCodes.Ret);

                // Define the "set" accessor.
                var currSetPropMthdBldr = typeBuilder.DefineMethod("set_value", getSetAttr, null, new[] { typeof(string) });
                var currSetIL = currSetPropMthdBldr.GetILGenerator();
                currSetIL.Emit(OpCodes.Ldarg_0);
                currSetIL.Emit(OpCodes.Ldarg_1);
                currSetIL.Emit(OpCodes.Stfld, field);
                currSetIL.Emit(OpCodes.Ret);

                property.SetGetMethod(currGetPropMthdBldr);
                property.SetSetMethod(currSetPropMthdBldr);
            }

            var generatedType = typeBuilder.CreateType();
            return generatedType;
        }
    }
}
