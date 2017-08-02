import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
import "rxjs/add/operator/toPromise";

import { GameData } from "../../models/gamedata";
import { UtilService } from "../util/util.service";

const FilePath: string = "data";
const FileName: string = "GameData.json";

@Injectable()
export class GameDataService {

  constructor(
    private http: Http,
    private util: UtilService) {
  }

  public getGameData(): Promise<GameData> {

    const url: string = this.util.getAssetFileUrl(FilePath, FileName);
    console.log(`Loading game data from ${url}...`);

    return this.http.get(url)
      .toPromise()
      .then(response => response.json().data as GameData)
      .catch(this.handleError);
  }

  private handleError(error: any): Promise<any> {

    console.error(error);
    return Promise.reject(error.message || error);
  }
}
