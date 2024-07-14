using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using iCare.Utilities;
using UnityEngine;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("iCare.AutoReference.Tests")]

namespace iCare.AutoReference.Editor {
    internal static class FieldInjector {
        internal static void Inject(this FieldInfo fieldInfo, object fieldObj, object data) {
            if (fieldObj == null)
                throw new Exception("Field object is null");


            fieldInfo.SetValue(fieldObj, null);

            if (data.IsUnityNull())
                return;

            if (TrySetFieldDirectly(fieldInfo, fieldObj, data))
                return;

            if (TrySetFieldFromPrefab(fieldInfo, fieldObj, data))
                return;

            if (data is not IEnumerable dataAsEnumerable)
                throw new Exception("Data is not enumerable");


            SetFieldFromEnumerable(fieldInfo, fieldObj, dataAsEnumerable);
        }

        static bool TrySetFieldFromPrefab(FieldInfo fieldInfo, object fieldObj, object data) {
            if (data is GameObject gameObject)
                if (gameObject.TryGetComponent(fieldInfo.FieldType, out var component)) {
                    fieldInfo.SetValue(fieldObj, component);
                    return true;
                }

            return false;
        }

        static void SetFieldFromEnumerable(FieldInfo fieldInfo, object fieldObj, IEnumerable dataAsEnumerable) {
            var fieldType = fieldInfo.FieldType;

            var castedData = dataAsEnumerable.Cast<object>().ToArray();

            if (!fieldType.IsIEnumerable() && TrySetFieldDirectly(fieldInfo, fieldObj, castedData.FirstOrDefault()))
                return;


            if (fieldType.IsList())
                SetList(fieldInfo, fieldObj, castedData);
            else if (fieldType.IsArray)
                SetArray(fieldInfo, fieldObj, castedData);
            else
                throw new Exception("Field type is unsupported IEnumerable type");
        }

        static void SetList(FieldInfo field, object fieldObject, IEnumerable<object> data) {
            var elementType = field.GetElementType();
            var list = CreateListInstance(elementType);

            foreach (var item in data) {
                var convertedValue = ConvertToType(item, elementType);
                list.Add(convertedValue);
            }

            field.SetValue(fieldObject, list);
        }

        static void SetArray(FieldInfo field, object fieldObject, IEnumerable<object> data) {
            var elementType = field.GetElementType();
            var convertedArray = CreateArrayInstance(data, elementType);
            field.SetValue(fieldObject, convertedArray);
        }

        static IList CreateListInstance(Type elementType) {
            var listType = typeof(List<>).MakeGenericType(elementType);
            return (IList)Activator.CreateInstance(listType);
        }

        static Array CreateArrayInstance(IEnumerable<object> data, Type elementType) {
            var dataArray = data.ToArray();
            var convertedArray = Array.CreateInstance(elementType, dataArray.Length);

            for (var i = 0; i < dataArray.Length; i++) {
                var convertedValue = ConvertToType(dataArray[i], elementType);
                convertedArray.SetValue(convertedValue, i);
            }

            return convertedArray;
        }

        static object ConvertToType(object value, Type targetType) {
            return targetType.IsInstanceOfType(value) ? value : Convert.ChangeType(value, targetType);
        }

        static bool TrySetFieldDirectly(FieldInfo fieldInfo, object fieldObj, object data) {
            if (!fieldInfo.FieldType.IsInstanceOfType(data)) return false;
            fieldInfo.SetValue(fieldObj, data);
            return true;
        }
    }
}