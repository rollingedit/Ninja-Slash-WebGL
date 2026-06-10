using UnityEngine;

namespace NinjaSlash.Port
{
    /// <summary>
    /// Attach this to a persistent GameObject if recovered code expects browser/plugin
    /// callbacks by string name. Add methods here to match missing SendMessage targets.
    /// </summary>
    public sealed class LegacyExternalCallSink : MonoBehaviour
    {
        public void OnKongregateReady(string ignored) { NinjaSlashPlatform.LogEvent("KongregateReadyIgnored", ignored); }
        public void OnFacebookReady(string ignored) { NinjaSlashPlatform.LogEvent("FacebookReadyIgnored", ignored); }
        public void OnAdClosed(string ignored) { NinjaSlashPlatform.LogEvent("AdClosedIgnored", ignored); }
        public void OnExternalError(string msg) { NinjaSlashPlatform.LogEvent("ExternalError", msg); }
    }
}
