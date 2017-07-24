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
    ClearSlot: 5
});

function App() {

    app = this;

    var templateScript;

    var airconsole;
    var viewManager;

    var isMasterPlayer = false;
    var isConfirmed = false;
    var playerName = "Guest";
    var playerTeamName = "Unaffiliated";

    var playerSlots = [];
    var filledSlots = 0;
    var teamSlots = [];

    var gameData = {};

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
            if(!response.version || CurrentGameDataVersion != response.version) {
                app.debugLog("Invalid game data version. Got " + response.version + ", expected " + CurrentGameDataVersion);
                return;
            }

            app.gameData = response;
            app.debugLog("Received game data", app.gameData.version);

            // init the slots to 0 (cleared)
            app.playerSlots = new Array(app.gameData.fighter.schematic.slots.length).fill(0);
            app.filledSlots = 0;
            app.teamSlots = new Array(app.gameData.fighter.schematic.slots.length).fill({});

            app.updateContent();
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

App.prototype._initHandlebars = function() {

    // register helpers
    Handlebars.registerHelper('ifEq', function(a, b, opts) {
        if(a == b) {
            return opts.fn(this);
        } else {
            return opts.inverse(this);
        }
    });

    // compile content template and set it with the initial values
    // so that the airconsole scripts don't choke on missing ids
    var template = $("#content-template").html();
    app.templateScript = Handlebars.compile(template);
}

App.prototype.updateContent = function() {

    app.debugLog("Updating content...");
    var compiledHtml = app.templateScript({
        "playerName": app.playerName,
        "playerTeamName": app.playerTeamName,
        "isMasterPlayer": app.isMasterPlayer,
        "gameData": app.gameData
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
    $("#button-confirm-text").html(app.isConfirmed ? "Unconfirm" : "Confirm");
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

    var disable = app.filledSlots >= app.gameData.fighter.schematic.maxFilledSlots;
    app.gameData.fighter.schematic.slots.forEach(function(slot) {
        var slotSelector = $("#slot_" + slot.id);
        if(app.playerSlots[slot.id - 1] == 0) {
            $("#slot_" + slot.id).prop("disabled", disable);
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

App.prototype.sendMessageToScreen = function(msg) {

    app.airconsole.message(AirConsole.SCREEN, msg);
}

App.prototype.broadcastMessage = function(msg) {

    app.airconsole.broadcast(msg);
}
