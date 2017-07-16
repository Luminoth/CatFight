var airconsole;

function App() {

    app = this;
    app.airconsole = new AirConsole({"orientation": "landscape"});

    app.airconsole.onMessage = function(from, data) {

        console.log("onMessage", from, data);

        $("#debug_logging").html("device " + from + " says: " + data);
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

App.prototype.sendMessageToScreen = function(msg) {
    this.airconsole.message(AirConsole.SCREEN, msg);
}

App.prototype.broadcastMessage = function(msg) {
    this.airconsole.broadcast(msg);
}
