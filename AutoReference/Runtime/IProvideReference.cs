using System;
using System.Collections.Generic;

namespace iCare.AutoReference {
    public interface IProvideReference {
        object RefService { get; }

        IEnumerable<Type> RefTypes {
            get {
                yield return RefService.GetType();
                foreach (var type in GetType().GetInterfaces())
                    yield return type;
            }
        }

        string RefID {
            get {
                if (RefService is UnityEngine.Object unityObject)
                    return unityObject.name;

                return null;
            }
        }
    }
}