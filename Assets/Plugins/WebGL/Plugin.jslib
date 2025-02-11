mergeInto(LibraryManager.library, {
    GetPlayerIdCookie: function() {
        var cookies = document.cookie.split(';');
        for (var i=0; i < cookies.length; i++) {
            var cookie = cookies[i].trim();
            var parts = cookie.split("=");
            var key = parts[0].trim();
            var value = parts.length > 1 ? decodeURIComponent(parts[1].trim()) : "";
            if (key === "id") {
                SendMessage("WebGLBridge", "SetPlayerID", value);
                console.log("Player ID: " + value);
                return;
            }
        }
    }
});