var airconsole;

function App() {

    this.airconsole = new AirConsole({"orientation": "landscape"});

    this.airconsole.onMessage = function(from, data) {

        console.log("onMessage", from, data);

        document.getElementById("info").innerHTML = "device " + from + " says: " + data;
    };

    this.airconsole.onReady = function(code) {
        console.log("onReady", code);
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
        var elements = document.getElementsByTagName("*");
        for(var i = 0; i < elements.length; ++i) {
            var element = elements[i];
            
            var ontouchstart = element.getAttribute("ontouchstart");
            if(ontouchstart) {
                element.setAttribute("onmousedown", ontouchstart);
            } 

            var ontouchend = element.getAttribute("ontouchend");
            if (ontouchend) { 
                element.setAttribute("onmouseup", ontouchend);
            }
        }
    }
}

App.prototype.sendMessageToScreen = function(msg) {
    this.airconsole.message(AirConsole.SCREEN, msg);
}

App.prototype.broadcastMessage = function(msg) {
    this.airconsole.broadcast(msg);
}
