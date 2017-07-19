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

var MessageType = {};
MessageType.None = 0;
MessageType.StartGame = 1;
MessageType.ConfirmStaging = 2;

var airconsole;
var viewManager;

var isMasterPlayer = false;
var isConfirmed = false;

function App() {

    app = this;

    // init AirConsole
    app.airconsole = new AirConsole({"orientation": "landscape"});

    app.airconsole.onReady = function(code) {

        app.debugLog("onReady", code);

        app.viewManager = new AirConsoleViewManager(app.airconsole);

        // load game data
        app.debugLog("Requesting game data...");
        var gameData = {};
        loadJSON("/data/GameData.json", function(response) {
            gameData = response;
            app.debugLog("Received game data version " + gameData.version);
        });
    }

    app.airconsole.onMessage = function(from, data) {

        app.debugLog("onMessage", from, data);

        var messageType = data.type;
        switch(messageType) {
            default:
                alert("Invalid message type: " + messageType);
                break;
        }
    };

    app.airconsole.onCustomDeviceStateChange = function(deviceId, data) {
        app.debugLog("onCustomDeviceStateChange", deviceId, data);
        app.checkForMasterPlayer(data);

        app.viewManager.onViewChange(data, function(viewId) {
            app.debugLog("onViewChange", viewId);
        });
    };

    // control hookups 
    var buttonStart = new Button("button-start", { 
        "down": function() {
            app.startGame(); 
        } 
    });

    var buttonConfirm = new Button("button-confirm", { 
        "down": function() {
            app.confirmStaging(); 
        } 
    });
}

App.prototype.debugLog = function() {

    var args = Array.from(arguments);
    args.unshift(app.airconsole.getDeviceId());
    return window.console && console.log && Function.apply.call(console.log, console, args);
}

App.prototype.checkForMasterPlayer = function(data) {

    if(!data.masterPlayer) {
        return;
    }

    app.isMasterPlayer = data.masterPlayer === app.airconsole.getDeviceId();
    if(app.isMasterPlayer && data.ctrl_view && data.ctrl_view == "lobby") {
        data.ctrl_view = "lobby-master";
    }
}

App.prototype.startGame = function(msg) { 

    app.debugLog("Starting game...");
    app.sendMessageToScreen({ 
        "type": MessageType.StartGame
    });
}

App.prototype.confirmStaging = function(msg) {

    isConfirmed = !isConfirmed;

    app.debugLog("Confirming staging: ", isConfirmed);
    app.sendMessageToScreen({
        "type": MessageType.ConfirmStaging,
        "isConfirmed": isConfirmed
    });
    $("#button-confirm-text").html(isConfirmed ? "Unconfirm" : "Confirm");
}

App.prototype.sendMessageToScreen = function(msg) {

    app.airconsole.message(AirConsole.SCREEN, msg);
}

App.prototype.broadcastMessage = function(msg) {

    app.airconsole.broadcast(msg);
}
