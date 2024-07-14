using System;
using System.Collections.Generic;
using UnityEngine;

namespace iCare.AutoReference.Tests.TestMaterials {
    public sealed class TestStringServiceSo : ScriptableObject, IProvideReference {
        [SerializeField] string id = "One";
        [SerializeField] string value;

        public IEnumerable<Type> RefTypes => new[] {typeof(string)};
        public string RefID => id;
        public object RefService => value;
    }
}