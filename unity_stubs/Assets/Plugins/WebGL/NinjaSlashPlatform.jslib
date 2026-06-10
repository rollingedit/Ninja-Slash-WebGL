mergeInto(LibraryManager.library, {
  NS_LogEvent: function(eventNamePtr, payloadPtr) {
    var eventName = UTF8ToString(eventNamePtr);
    var payload = payloadPtr ? UTF8ToString(payloadPtr) : "";
    if (typeof console !== 'undefined') console.log('[NinjaSlash]', eventName, payload);
  },

  NS_SubmitStat: function(namePtr, value) {
    var name = UTF8ToString(namePtr);
    try {
      var key = 'ninjaslash.stat.' + name;
      var prev = parseFloat(localStorage.getItem(key) || '0');
      if (value > prev) localStorage.setItem(key, String(value));
    } catch (e) {}
  },

  NS_SaveString: function(keyPtr, valuePtr) {
    var key = UTF8ToString(keyPtr);
    var value = UTF8ToString(valuePtr);
    try { localStorage.setItem('ninjaslash.' + key, value); } catch (e) {}
  },

  NS_LoadString: function(keyPtr) {
    var key = UTF8ToString(keyPtr);
    var value = '';
    try { value = localStorage.getItem('ninjaslash.' + key) || ''; } catch (e) {}
    var len = lengthBytesUTF8(value) + 1;
    var buffer = _malloc(len);
    stringToUTF8(value, buffer, len);
    return buffer;
  },

  NS_HasExternalService: function() {
    return 0;
  }
});
