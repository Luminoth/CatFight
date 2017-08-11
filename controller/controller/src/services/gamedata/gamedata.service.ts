import { Injectable } from "@angular/core";
import { Http } from "@angular/http";

import { IGameData } from "../../models/data/gamedata";
import { UtilService } from "../util/util.service";

const FilePath: string = "data";
const FileName: string = "GameData.json";

@Injectable()
export class GameDataService {

    constructor(
        private readonly http: Http,
        private readonly util: UtilService) {
    }

    public getGameData(): Promise<IGameData> {

        const url: string = this.util.getAssetFileUrl(FilePath, FileName);
        console.log(`Loading game data from ${url}...`);

        return this.http.get(url)
            .map(response => response.json())
            .toPromise()
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {

        console.error(error);
        return Promise.reject(error.message || error);
    }
}
