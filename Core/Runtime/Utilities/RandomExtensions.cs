using System.Collections.Generic;
using UnityEngine;

namespace iCare.Utilities {
    public static class RandomExtensions {
        public static Vector3 RandomizeAll(this Vector3 vector, float randomness) {
            var randomValue = Random.Range(-randomness, randomness);
            return new Vector3(
                vector.x + randomValue,
                vector.y + randomValue,
                vector.z + randomValue
            );
        }

        public static T GetRandom<T>(this IReadOnlyList<T> list) {
            return list[Random.Range(0, list.Count)];
        }

        public static float GetRandom(this Vector2 range) {
            return Random.Range(range.x, range.y);
        }

        public static float GetRandom(this Vector2Int range) {
            return Random.Range(range.x, range.y);
        }
    }
}