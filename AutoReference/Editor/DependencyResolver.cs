using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using iCare.Utilities;
using UnityEngine;

namespace iCare.AutoReference.Editor {
    internal sealed class DependencyResolver {
        readonly IEnumerable<IProvideReference> _services;

        internal DependencyResolver(IEnumerable<IProvideReference> services) {
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        internal object Resolve(FieldInfo field) {
            var attribute = field.GetCustomAttribute<ReferenceAttribute>();
            if (attribute == null)
                throw new ArgumentException("Field must have a ReferenceAttribute", nameof(field));

            var fieldElementType = field.GetElementType();
            var result = ResolveData(attribute, fieldElementType);

            return result;
        }

        object ResolveData(ReferenceAttribute attribute, Type fieldElementType) {
            var result = attribute switch {
                AutoAttribute autoAttribute => ResolveAutoAttribute(fieldElementType, autoAttribute.ID),
                AutoListAttribute autoListAttribute => ResolveAutoListAttribute(fieldElementType, autoListAttribute.ID),
                _ => throw new ArgumentOutOfRangeException(nameof(attribute), "Unsupported attribute type")
            };

            if (result.IsUnityNull())
                Debug.LogWarning(
                    $"Data not found for field type: {fieldElementType.ToString().Highlight()} and ID: {attribute.ID.Highlight()}");

            return result;
        }

        object ResolveAutoAttribute(Type targetType, string targetID) {
            var result = GetMatchingServices(targetType, targetID);

            if (result.Length > 1)
                Debug.LogWarning(
                    $"Multiple services found for type {targetType.ToString().Highlight()} and ID: {targetID.Highlight()}");


            return result.FirstOrDefault();
        }

        IEnumerable<object> ResolveAutoListAttribute(Type type, string targetID) {
            var result = GetMatchingServices(type, targetID, true);

            if (result.Any(data => data.IsUnityNull()))
                Debug.LogWarning($"Null data found for type {type} and ID: {targetID}");

            return result;
        }

        object[] GetMatchingServices(Type type, string targetID, bool allowEmptyID = false) {
            return _services.Where(service =>
                    service.RefTypes.Contains(type) &&
                    (allowEmptyID || IsServiceIDMatching(targetID, service.RefID)))
                .Select(service => service.RefService)
                .ToArray();
        }

        static bool IsServiceIDMatching(string targetID, string serviceID) {
            return targetID == serviceID || (string.IsNullOrEmpty(targetID) && string.IsNullOrEmpty(serviceID));
        }
    }
}