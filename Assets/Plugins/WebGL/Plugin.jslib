mergeInto(LibraryManager.library, {
    CallJSFunction: function(message) {
        alert("Message from Unity : " + UTF8ToString(message));
    }
});