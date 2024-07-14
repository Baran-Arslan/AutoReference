using System;
using System.Collections.Generic;
using UnityEngine;

namespace iCare.AutoReference.Tests.TestMaterials {
    public sealed class TestIntServiceSo : ScriptableObject, IProvideReference {
        [SerializeField] string id = "One";
        [SerializeField] int value;

        public IEnumerable<Type> RefTypes => new[] {typeof(int)};
        public string RefID => id;
        public object RefService => value;
    }
}