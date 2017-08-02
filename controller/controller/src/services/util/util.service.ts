import { Injectable } from '@angular/core';

@Injectable()
export class UtilService {

  private assetPath: string = "";

  constructor() {

    this.assetPath = "assets/";
  }

  public getAssetPathUrl(path: string): string {

    return this.assetPath + path + "/";
  }

  public getAssetFileUrl(path: string, name: string): string {

    return this.getAssetPathUrl(path) + name;
  }
}
