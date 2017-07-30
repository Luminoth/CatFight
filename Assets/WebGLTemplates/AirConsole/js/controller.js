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

// NOTE: must match the values from Unity
var MessageType = Object.freeze({
    None: 0,
    SetTeam: 1,
    StartGame: 2,
    ConfirmStaging: 3,
    SetSlot: 4,
    ClearSlot: 5,
    UseSpecial: 6
});

var SpecialType = Object.freeze({
    Missiles: "Missiles",
    Chaff: "Chaff"
});

function App() {

    app = this;

    app.templateScript = null;

    app.airconsole = null;
    app.viewManager = null;

    app.isMasterPlayer = false;
    app.isConfirmed = false;
    app.playerName = "Guest";
    app.playerTeamId = "None";
    app.playerTeamName = "Unaffiliated";

    app.playerSlots = [];
    app.filledSlots = 0;
    app.teamSlots = [];

    app.isGameStarted = false;

    app.missilesRemaining = 0;
    app.chaffRemaining = 0;

    app.gameData = {};

    app._initHandlebars()

    // init AirConsole
    app.airconsole = new AirConsole({"orientation": "landscape"});

    app.airconsole.onReady = function(code) {

        app.debugLog("onReady", code);

        app.playerName = app.airconsole.getNickname();
        app.updateContent();

        app.viewManager = new AirConsoleViewManager(app.airconsole);

        // load game data
        app.debugLog("Requesting game data...");
        loadJSON("/data/GameData.json", function(response) {
            app.debugLog("Received game data response", response);

            if(!response.Version || CurrentGameDataVersion != response.Version) {
                app.debugLog("Invalid game data version. Got " + response.Version + ", expected " + CurrentGameDataVersion);
                return;
            }
            app.gameData = response;

            app.reset();
        });
    }

    app.airconsole.onMessage = function(from, data) {

        app.debugLog("onMessage", from, data);

        var messageType = data.type;
        switch(messageType) {
            case MessageType.SetTeam:
                app.playerTeamId = data.teamId;
                app.playerTeamName = data.teamName;
                app.updateContent();

                app.airconsole.setCustomDeviceStateProperty("teamData", data);
                break;
            case MessageType.SetSlot:
                app._setTeamSlot(data.slotId, data.itemId);
                break;
            case MessageType.ClearSlot:
                app._clearTeamSlot(data.slotId, data.itemId);
                break;
            default:
                alert("Invalid message type: " + messageType);
                break;
        }
    };

    app.airconsole.onCustomDeviceStateChange = function(deviceId, data) {

        app.debugLog("onCustomDeviceStateChange", deviceId, data);
        app.checkForMasterPlayer(data);
        app.updateGameState(data);

        app.viewManager.onViewChange(data, function(viewId) {
            app.debugLog("onViewChange", viewId);

            if(viewId == "lobby") {
                app.reset();
            }
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

App.prototype._initHandlebars = function() {

    // register helpers

    // https://stackoverflow.com/questions/8853396/logical-operator-in-a-handlebars-js-if-conditional
    Handlebars.registerHelper('ifCond', function (v1, operator, v2, options) {
        switch(operator) {
            case '==':
                return (v1 == v2) ? options.fn(this) : options.inverse(this);
            case '===':
                return (v1 === v2) ? options.fn(this) : options.inverse(this);
            case '!=':
                return (v1 != v2) ? options.fn(this) : options.inverse(this);
            case '!==':
                return (v1 !== v2) ? options.fn(this) : options.inverse(this);
            case '<':
                return (v1 < v2) ? options.fn(this) : options.inverse(this);
            case '<=':
                return (v1 <= v2) ? options.fn(this) : options.inverse(this);
            case '>':
                return (v1 > v2) ? options.fn(this) : options.inverse(this);
            case '>=':
                return (v1 >= v2) ? options.fn(this) : options.inverse(this);
            case '&&':
                return (v1 && v2) ? options.fn(this) : options.inverse(this);
            case '||':
                return (v1 || v2) ? options.fn(this) : options.inverse(this);
            default:
                return options.inverse(this);
        }
    });

    // compile content template and set it with the initial values
    // so that the airconsole scripts don't choke on missing ids
    var template = $("#content-template").html();
    app.templateScript = Handlebars.compile(template);
}

App.prototype.reset = function() {

    app.isConfirmed = false;

    if(app.gameData && app.gameData.Fighter) {
        app.playerSlots = new Array(app.gameData.Fighter.Schematic.Slots.length).fill(0);
        app.teamSlots = new Array(app.gameData.Fighter.Schematic.Slots.length).fill({});
    } else {
        app.playerSlots = [];
        app.teamSlots = [];
    }
    app.filledSlots = 0;

    app.missilesRemaining = 0;
    app.chaffRemaining = 0;

    app.updateContent();
}

App.prototype.updateContent = function() {

    app.debugLog("Updating content...");
    var compiledHtml = app.templateScript({
        "app": app
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

    if(app.isGameStarted) {
        if(app.missilesRemaining > 0) {
            var buttonUseMissiles = new Button("button-missiles", {
                "down": function() {
                    app.fireMissiles();
                }
            });
        }

        if(app.chaffRemaining > 0) {
            var buttonUseChaff = new Button("button-chaff", {
                "down": function() {
                    app.launchChaff();
                }
            });
        }
    }

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
    if(wasMasterPlayer != app.isMasterPlayer) {
        app.updateContent();
    }
}

App.prototype.updateGameState = function(data) {

    if(!data.gameState) {
        return;
    }

    app.isGameStarted = data.gameState.isGameStarted;

    var fighterState = data.gameState.fighterState[app.playerTeamId];
    if(fighterState) {
        app.missilesRemaining = fighterState.specialsRemaining[SpecialType.Missiles];
        app.chaffRemaining = fighterState.specialsRemaining[SpecialType.Chaff];
    } else {
        app.missilesRemaining = 0;
        app.chaffRemaining = 0;
    }

    app.updateContent();
}

App.prototype.startGame = function(msg) { 

    app.debugLog("Starting game...");
    app.sendMessageToScreen({ 
        "type": MessageType.StartGame
    });
}

App.prototype.confirmStaging = function(msg) {

    app.isConfirmed = !app.isConfirmed;

    app.debugLog("Confirming staging", app.isConfirmed);
    app.sendMessageToScreen({
        "type": MessageType.ConfirmStaging,
        "isConfirmed": app.isConfirmed
    });

    app.updateContent();
}

App.prototype.selectSchematicSlot = function(slotId) {

    var value = $("#slot_" + slotId).val();
    if(value) {
        app._setSlot(slotId, parseInt(value));
    } else {
        app._clearSlot(slotId);
    }
}

App.prototype._enableSchematicSlots = function() {

    var disable = app.filledSlots >= app.gameData.Fighter.Schematic.MaxFilledSlots;
    app.gameData.Fighter.Schematic.Slots.forEach(function(slot) {
        var slotSelector = $("#slot_" + slot.Id);
        if(app.playerSlots[slot.Id - 1] == 0) {
            $("#slot_" + slot.Id).prop("disabled", disable);
        }
    });
}

App.prototype._setSlot = function(slotId, itemId) {

    app.debugLog("Setting slot", slotId, itemId, app.filledSlots);
    app.sendMessageToScreen({
        "type": MessageType.SetSlot,
        "slotId": slotId,
        "itemId": itemId
    });

    var idx = slotId - 1;
    if(app.playerSlots[idx] == 0) {
        ++app.filledSlots;
    }
    app.playerSlots[idx] = itemId;

    app._enableSchematicSlots();
}

App.prototype._setTeamSlot = function(slotId, itemId) {

    var slotIdx = slotId - 1;
    var itemIdx = itemId - 1;

    var slot = app.teamSlots[slotIdx];
    if(!(itemIdx in slot)) {
        slot[itemIdx] = 0;
    }

    app.debugLog("Setting team slot", slotId, itemId, slot[itemIdx]);
    ++slot[itemIdx];
}

App.prototype._clearSlot = function(slotId) {

    app.debugLog("Clearing slot", slotId, app.filledSlots);
    app.sendMessageToScreen({
        "type": MessageType.ClearSlot,
        "slotId": slotId
    });

    var idx = slotId - 1;
    if(app.playerSlots[idx] != 0) {
        --app.filledSlots;
    }
    app.playerSlots[idx] = 0;

    app._enableSchematicSlots();
}

App.prototype._clearTeamSlot = function(slotId, itemId) {

    var slotIdx = slotId - 1;
    var itemIdx = itemId - 1;

    var slot = app.teamSlots[slotIdx];
    if(!(itemId in slot)) {
        slot[itemIdx] = 1;
    }

    app.debugLog("Clearing team slot", slotId, itemId, slot[itemIdx]);
    --slot[itemIdx];
}

App.prototype.fireMissiles = function() {

    app._useSpecial(SpecialType.Missiles);

    --app.missilesRemaining;
    app.updateContent();
}

App.prototype.launchChaff = function() {

    app._useSpecial(SpecialType.Chaff);

    --app.chaffRemaining;
    app.updateContent();
}

App.prototype._useSpecial = function(specialType) {

    if(!app.isGameStarted) {
        return;
    }

    app.debugLog("Using special", specialType);
    app.sendMessageToScreen({
        "type": MessageType.UseSpecial,
        "specialType": specialType
    });
}

App.prototype.sendMessageToScreen = function(msg) {

    app.airconsole.message(AirConsole.SCREEN, msg);
}

App.prototype.broadcastMessage = function(msg) {

    app.airconsole.broadcast(msg);
}
