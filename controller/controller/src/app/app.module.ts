import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { NgModule } from "@angular/core";
import { HttpModule } from "@angular/http";
import { MdButtonModule } from "@angular/material";

import { AppComponent } from "./app.component";

import { GameDataService } from "../services/gamedata/gamedata.service";
import { UtilService } from "../services/util/util.service";

import { TouchEventModule } from "../plugins/touch";

import "rxjs/add/operator/map";
import "rxjs/add/operator/toPromise";
import "hammerjs";

@NgModule({
    declarations: [
        AppComponent
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        HttpModule,
        MdButtonModule,
        TouchEventModule
    ],
    providers: [
        GameDataService,
        UtilService
    ],
    bootstrap: [AppComponent]
})

export class AppModule { }
