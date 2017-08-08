import { Injectable } from "@angular/core";

@Injectable()
export class UtilService {

    private readonly assetPath: string = "assets/";

    public getAssetPathUrl(path: string): string {

        return this.assetPath + path + "/";
    }

    public getAssetFileUrl(path: string, name: string): string {

        return this.getAssetPathUrl(path) + name;
    }
}
