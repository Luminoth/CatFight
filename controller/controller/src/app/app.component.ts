import { Component, OnInit } from "@angular/core";

import { GameDataService } from "../services/gamedata/gamedata.service";

import { GameData } from "../models/gamedata";
import { Player } from "../models/player";

declare var AirConsole: any;

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"]
})

export class AppComponent implements OnInit {

  public title: string = "Cat Fight Controller";

  private _airConsole: any;
  private _player: Player;
  private _gameData: GameData;

  constructor(
    private gameDataService: GameDataService) {

    this._player = new Player();
    this._gameData = new GameData();
  }

  public ngOnInit(): void {

    this._airConsole = new AirConsole({"orientation": "landscape"});

    this._gameData = this.gameDataService.getGameData()
      .then(gameData => this._gameData = gameData)
      .catch(this.handleError);
  }

  private handleError(error: any): Promise<any> {

    console.error(error);
    return Promise.reject(error.message || error);
  }
}
