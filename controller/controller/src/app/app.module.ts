import { BrowserModule } from "@angular/platform-browser";
import { HttpModule } from "@angular/http";
import { NgModule } from "@angular/core";

import { AppComponent } from "./app.component";

import { GameDataService } from "../services/gamedata/gamedata.service";
import { UtilService } from "../services/util/util.service";

@NgModule({
    declarations: [
        AppComponent
    ],
    imports: [
        BrowserModule,
        HttpModule
    ],
    providers: [
        GameDataService,
        UtilService
    ],
    bootstrap: [AppComponent]
})

export class AppModule { }
