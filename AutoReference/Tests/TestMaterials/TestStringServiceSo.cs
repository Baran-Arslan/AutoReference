using UnityEngine;

namespace iCare.AutoReference.Tests.TestMaterials {
    public sealed class TestStringServiceSo : ScriptableObject, IProvideReference {
        [SerializeField] string id = "One";
        [SerializeField] string value;

        public string RefID => id;
        public object RefService => value;
    }
}