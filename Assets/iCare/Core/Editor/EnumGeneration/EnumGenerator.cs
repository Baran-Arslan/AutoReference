using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace iCare.Editor.EnumGeneration {
    internal static class EnumGenerator {
        internal static void GenerateByEnumType(Type enumType, IEnumerable<string> enumsToBeAdded) {
            if (enumType == null)
                throw new ArgumentNullException(nameof(enumType));

            if (enumsToBeAdded == null)
                throw new ArgumentNullException(nameof(enumsToBeAdded));

            var path = FindPathByType(enumType);
            GenerateEnums(path, enumsToBeAdded);
        }

        static void GenerateEnums(string filePathAndName, IEnumerable<string> enumsToBeAdded) {
            var targetEnumName = Path.GetFileNameWithoutExtension(filePathAndName);
            var oldEnums = GetCurrentEnums(filePathAndName);

            var highestValue = 1;
            if (oldEnums != null && oldEnums.Any())
                highestValue = oldEnums.Values.Max() + 1;

            WriteEnums(targetEnumName, enumsToBeAdded, filePathAndName, oldEnums, highestValue);

            AssetDatabase.Refresh();
        }

        static void WriteEnums(string targetEnumName, IEnumerable<string> enumsToBeAdded, string filePathAndName,
            IReadOnlyDictionary<string, int> oldEnums, int highestValue) {
            using (var streamWriter = new StreamWriter(filePathAndName, false, Encoding.UTF8, 65536)) {
                streamWriter.WriteLine("// ReSharper disable CheckNamespace");
                streamWriter.WriteLine("public enum " + targetEnumName);
                streamWriter.WriteLine("{");
                streamWriter.WriteLine("    Empty = 0,");

                WriteNewEnums(enumsToBeAdded, oldEnums, highestValue, streamWriter);

                streamWriter.WriteLine("}");
            }
        }

        static void WriteNewEnums(IEnumerable<string> enumsToBeAdded, IReadOnlyDictionary<string, int> oldEnums,
            int highestValue,
            StreamWriter streamWriter) {
            var index = 0;
            foreach (var enumString in enumsToBeAdded) {
                if (oldEnums != null && oldEnums.TryGetValue(enumString, out var oldEnumNumber)) {
                    streamWriter.WriteLine("    " + enumString + " = " + oldEnumNumber + ",");
                }
                else {
                    var newEnumNumber = index + highestValue;
                    streamWriter.WriteLine("    " + enumString + " = " + newEnumNumber + ",");
                }

                index++;
            }
        }

        static Dictionary<string, int> GetCurrentEnums(string filePathAndName) {
            if (!File.Exists(filePathAndName))
                throw new FileNotFoundException("File not found: " + filePathAndName);

            var oldEnums = new Dictionary<string, int>();

            var lines = File.ReadAllLines(filePathAndName);
            foreach (var line in lines) {
                if (!line.Contains("=")) continue;

                var parts = line.Split('=');
                if (parts.Length != 2) {
                    Error("Invalid enum line: " + line);
                    continue;
                }

                var enumName = parts[0].Trim();
                var enumNumber = parts[1].Trim().TrimEnd(',');

                if (int.TryParse(enumNumber, out var number)) oldEnums.Add(enumName, number);
                else Error("Invalid enum number: " + enumNumber);
            }

            return oldEnums;
        }

        static string FindPathByType(Type type) {
            var typeName = type.Name;
            var guids = AssetDatabase.FindAssets(typeName);

            foreach (var guid in guids) {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (path.EndsWith(typeName + ".cs")) return path;
            }

            Error("Type " + typeName + " not found in project.");
            return string.Empty;
        }


        static void Error(string message) {
            Debug.LogError(message);
        }
    }
}