using UnityEngine;

namespace Game.Utils.Saving
{
    public static class SavableExtensions
    {
        private const string Tag = "Saving";

        private static string GetKey<T>(int? uniqueId = null) where T : SaveParams
        {
            var id = uniqueId.HasValue ? uniqueId.Value.ToString() : "default";
            var key = $"save_{typeof(T).Name}_{id}";
            Debug.Log($"Key created {key}");
            return key;
        }

        public static void SaveToDisk<T>(this ISavable<T> savable, int? uniqueId = null) where T : SaveParams
        {
            var key = GetKey<T>(uniqueId);
            var json = savable.GetCurrentSaveParams().ToJson();

            var message = $"{key} {json}";

            Debug.Log($"{Tag} {message}");

            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public static void RestoreSavedState<T>(this ISavable<T> savable, int? uniqueId = null)
            where T : SaveParams, new()
        {
            var key = GetKey<T>(uniqueId);

            if (!PlayerPrefs.HasKey(key))
            {
                Debug.Log($"{Tag} No data found for {key}");
                savable.ApplyParams(null);
            }
            else
            {
                var param = new T();
                savable.ApplyParams((T) param.FromJson(PlayerPrefs.GetString(key)));
            }
        }
    }
}