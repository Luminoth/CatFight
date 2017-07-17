function loadJSON(filename, callback) {
    // https://codepen.io/KryptoniteDove/post/load-json-file-locally-using-pure-javascript

    var xobj = new XMLHttpRequest();
    xobj.overrideMimeType("application/json");
    xobj.onreadystatechange = function() {
        if(4 == xobj.readyState && 200 == xobj.status) {
            callback(JSON.parse(xobj.response));
        }
    }
    xobj.open("GET", filename, true);
    xobj.send();
}

var airconsole;

var MessageType = {};
MessageType.None = 0;
MessageType.Debug = 1;

function App() {

    app = this;

    // controls
    var debug_log = $("#debug_log");

    // load game data
    console.log("Requesting game data...");
    var gameData = {};
    loadJSON("/data/GameData.json", function(response) {
        gameData = response;
        console.log("Received game data version " + gameData.version);
    });

    // init AirConsole
    app.airconsole = new AirConsole({"orientation": "landscape"});

    app.airconsole.onMessage = function(from, data) {

        console.log("onMessage", from, data);

        var messageType = data.type;
        switch(messageType) {
            case MessageType.Debug:
                debug_log.append("<div>" + from + ": " + data.message + "</div>");
                break;
            default:
                alert("Invalid message type: " + messageType);
                break;
        }
    };

    app.airconsole.onReady = function(code) {

        console.log("onReady", code);

        $("#loading").hide();
        $("#welcome").show();
    }

    /** 
     * Here we are adding support for mouse events manually. 
     * WE STRONGLY ENCOURAGE YOU TO USE THE AIRCONSOLE CONTROLS LIBRARY 
     * WHICH IS EVEN BETTER (BUT WE DONT WANT TO BLOAT THE CODE HERE). 
     * https://github.com/AirConsole/airconsole-controls/ 
     *  
     * NO MATTER WHAT YOU DECIDE, DO NOT USE ONCLICK HANDLERS. 
     * THEY HAVE A 200MS DELAY! 
     */ 
    if(!("ontouchstart" in document.createElement("div"))) {

        var elements = $("*").each(function() {
            var ontouchstart = $(this).attr("ontouchstart");
            if(ontouchstart) {
                $(this).attr("onmousedown", ontouchstart);
            } 

            var ontouchend = $(this).attr("ontouchend");
            if (ontouchend) { 
                $(this).attr("onmouseup", ontouchend);
            }
        });
    }
}

App.prototype.debugMessage = function(msg) {
    app.sendMessageToScreen({
        "type": MessageType.Debug,
        "message": msg
    });
}

App.prototype.sendMessageToScreen = function(msg) {
    this.airconsole.message(AirConsole.SCREEN, msg);
}

App.prototype.broadcastMessage = function(msg) {
    this.airconsole.broadcast(msg);
}
