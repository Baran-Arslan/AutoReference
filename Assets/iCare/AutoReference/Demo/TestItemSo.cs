using System;
using System.Collections.Generic;
using UnityEngine;

namespace iCare.AutoReference.Demo {
    [CreateAssetMenu] //----------------Enum target interface is optional you can use string instead!
    public sealed class TestItemSo : ScriptableObject, IEnumTarget<TestItems>, IProvideReference
    {
        public object RefService => this;

        public IEnumerable<Type> RefTypes {
            get {
                yield return RefService.GetType();
                //You can bind more types if needed like ITestItem interface etc...
            }
        }

        public string RefID => name;
    }
}
