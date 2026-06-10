using System.Runtime.InteropServices;
using UnityEngine;

namespace NinjaSlash.Port
{
    /// <summary>
    /// Local platform bridge for recovered Web Player code. Route old Kongregate,
    /// Facebook, ad, analytics, and browser calls here instead of letting them fail.
    /// </summary>
    public static class NinjaSlashPlatform
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern void NS_LogEvent(string eventName, string payloadJson);
        [DllImport("__Internal")] private static extern void NS_SubmitStat(string name, int value);
        [DllImport("__Internal")] private static extern void NS_SaveString(string key, string value);
        [DllImport("__Internal")] private static extern string NS_LoadString(string key);
        [DllImport("__Internal")] private static extern int NS_HasExternalService();
#endif

        public static bool HasExternalService
        {
            get
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                return NS_HasExternalService() != 0;
#else
                return false;
#endif
            }
        }

        public static void LogEvent(string eventName, string payloadJson = "")
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            NS_LogEvent(eventName, payloadJson ?? "");
#else
            Debug.Log($"[NinjaSlash] {eventName} {payloadJson}");
#endif
        }

        public static void SubmitStat(string name, int value)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            NS_SubmitStat(name, value);
#else
            var key = "stat." + name;
            if (value > PlayerPrefs.GetInt(key, 0)) PlayerPrefs.SetInt(key, value);
#endif
        }

        public static void SaveString(string key, string value)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            NS_SaveString(key, value ?? "");
#else
            PlayerPrefs.SetString(key, value ?? "");
#endif
        }

        public static string LoadString(string key, string fallback = "")
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            var value = NS_LoadString(key);
            return string.IsNullOrEmpty(value) ? fallback : value;
#else
            return PlayerPrefs.GetString(key, fallback);
#endif
        }
    }
}
