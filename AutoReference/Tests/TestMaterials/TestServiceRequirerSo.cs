using System.Collections.Generic;
using UnityEngine;

namespace iCare.AutoReference.Tests.TestMaterials {
    public sealed class TestServiceRequirerSo : ScriptableObject {
        [Auto("One")] [SerializeField] int autoIntOne;
        [Auto("Two")] public int AutoIntTwo;
        [Auto("Three")] public int AutoIntThree;
        [AutoList] public int[] AllAutoInts;

        [Auto("One")] public string AutoStringOne;
        [Auto("Two")] public string AutoStringTwo;
        [Auto("Three")] public string AutoStringThree;
        [AutoList] public string[] AllAutoStrings;

        [AutoList] [SerializeField] List<string> stringList;
        [AutoList] [SerializeField] List<int> intList;

        internal int AutoIntOneWrapper {
            get => autoIntOne;
            set => autoIntOne = value;
        }

        internal List<string> StringListWrapper {
            get => stringList;
            set => stringList = value;
        }

        internal List<int> IntListWrapper {
            get => intList;
            set => intList = value;
        }
    }
}