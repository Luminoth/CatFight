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

const CurrentGameDataVersion = 1;

var MessageType = Object.freeze({
    None: 0,
    StartGame: 1,
    ConfirmStaging: 2,
    SetTeam: 3,
    SetSlot: 4,
    ClearSlot: 5,
    ControllerAction: 6
});

var templateScript;

var airconsole;
var viewManager;

var isMasterPlayer = false;
var isConfirmed = false;
var playerName;
var playerTeamName;

function App() {

    app = this;

    // compile content template and set it with the initial values
    // so that the airconsole scripts don't choke on missing ids
    var template = $("#content-template").html();
    app.templateScript = Handlebars.compile(template);

    // init AirConsole
    app.airconsole = new AirConsole({ "orientation": "landscape" });

    app.airconsole.onReady = function(code) {

        app.debugLog("onReady", code);

        app.playerName = app.airconsole.getNickname();
        app.updateContent();

        app.viewManager = new AirConsoleViewManager(app.airconsole);

        // load game data
        app.debugLog("Requesting game data...");
        var gameData = {};
        loadJSON("/data/GameData.json", function(response) {
            if(!response.version || CurrentGameDataVersion != response.version) {
                app.debugLog("Invalid game data version. Got " + response.version + ", expected " + CurrentGameDataVersion);
                return;
            }

            gameData = response;
            app.debugLog("Received game data version " + gameData.version);
        });
    }

    app.airconsole.onMessage = function(from, data) {

        app.debugLog("onMessage", from, data);

        var messageType = data.type;
        switch(messageType) {
            case MessageType.SetTeam:
                app.playerTeamName = data.teamName;
                app.updateContent();
                app.airconsole.setCustomDeviceStateProperty("teamData", data);
                break;
            case MessageType.SetSlot:
app.debugLog("TODO: handle set slot message");
                break;
            case MessageType.ClearSlot:
app.debugLog("TODO: handle set slot message");
                break;
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
}

App.prototype.debugLog = function() {

    var args = Array.from(arguments);
    if(app.airconsole) {
        args.unshift(app.airconsole.getDeviceId());
    }
    return window.console && console.log && Function.apply.call(console.log, console, args);
}

App.prototype.updateContent = function() {

    app.debugLog("Updating content...");
    var compiledHtml = app.templateScript({
        "playerName": app.playerName,
        "playerTeamName": app.playerTeamName,
        "isMasterPlayer": app.isMasterPlayer
    });
    $("#content-placeholder").html(compiledHtml);

    // control hookups
    if(app.isMasterPlayer) {
        var buttonStart = new Button("button-start", { 
            "down": function() {
                app.startGame(); 
            } 
        });
    }

    var buttonConfirm = new Button("button-confirm", { 
        "down": function() {
            app.confirmStaging(); 
        } 
    });

    //Controller Inputs
    var fireDown = new Button("button-fire", { 
        "down": function() {
            app.controlleraction("","fire","down",0); 
        }
    });

    var fireUp = new Button("button-fire", {
        "up": function () {
            app.controlleraction("", "fire", "up",0);
        }
    });

    var upDown = new Button("button-up", {
        "down": function () {
            app.controlleraction("", "up", "down", 0);
        }
    });

    var upUp = new Button("button-up", {
        "up": function () {
            app.controlleraction("", "up", "up", 0);
        }
    });

    var leftDown = new Button("button-left", {
        "down": function () {
            app.controlleraction("", "left", "down", 0);
        }
    });

    var leftUp = new Button("button-left", {
        "up": function () {
            app.controlleraction("", "left", "up", 0);
        }
    });

    var rightDown = new Button("button-right", {
        "down": function () {
            app.controlleraction("", "right", "down", 0);
        }
    });

    var rightUp = new Button("button-right", {
        "up": function () {
            app.controlleraction("", "right", "up", 0);
        }
    });

    var downDown = new Button("button-down", {
        "down": function () {
            app.controlleraction("", "down", "down", 0);
        }
    });

    var downUp = new Button("button-down", {
        "up": function () {
            app.controlleraction("", "down", "up", 0);
        }
    });


    // view manager must reset views
    if(app.viewManager) {
        app.viewManager.setupViews();
    }
}

App.prototype.checkForMasterPlayer = function(data) {

    if(!data.masterPlayer) {
        return;
    }

    var wasMasterPlayer = app.isMasterPlayer;
    app.isMasterPlayer = data.masterPlayer === app.airconsole.getDeviceId();
    if(wasMasterPlayer != isMasterPlayer) {
        app.updateContent();
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

App.prototype.controlleraction = function (msg, buttonType, state, fireType) {
    app.debugLog("Input Pressed:", buttonType, state);
    app.sendMessageToScreen({
        "type": MessageType.ControllerAction,
        "button": buttonType,
        "buttonState": state,
        "fireType" : fireType
    });
}

App.prototype.sendMessageToScreen = function(msg) {

    app.airconsole.message(AirConsole.SCREEN, msg);
}

App.prototype.broadcastMessage = function(msg) {

    app.airconsole.broadcast(msg);
}
