using UnityEngine;

namespace iCare.AutoReference.Tests.TestMaterials {
    public sealed class TestIntServiceSo : ScriptableObject, IProvideReference {
        [SerializeField] string id = "One";
        [SerializeField] int value;

        public string RefID => id;
        public object RefService => value;
    }
}