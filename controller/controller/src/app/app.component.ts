import { Component, OnInit } from "@angular/core";

import { GameDataService } from "../services/gamedata/gamedata.service";

import { GameState } from "../models/gamestate";
import { Player } from "../models/player";
import { IGameData, GameData } from "../models/data/gamedata";
import { IMessage, MessageType } from "../models/messages/message";
import { StartGameMessage } from "../models/messages/startgamemessage";
import { ConfirmStagingMessage } from "../models/messages/confirmstagingmessage";
import { SetSlotMessage } from "../models/messages/setslotmessage";
import { ClearSlotMessage } from "../models/messages/clearslotmessage";
import { UseSpecialMessage } from "../models/messages/usespecialmessage";

@Component({
    selector: "app-root",
    templateUrl: "./app.component.html",
    styleUrls: ["./app.component.css"]
})

// TODO: go through and get rid of "any" variables where possible

export class AppComponent implements OnInit {

    private static readonly CurrentGameDataVersion: number = 1;

    public readonly Title: string = "Cat Fight Controller";

    private _airConsole: any;
    private _isAirConsoleReady: boolean;

    private _viewManager: any;

    private _gameData: GameData = new GameData();

    public get gameData(): GameData {
        return this._gameData;
    }

    private setGameData(gameData: IGameData): void {
        this._gameData = GameData.create(gameData);
    }

    private _gameState: GameState = new GameState();

    public get gameState(): GameState {
        return this._gameState;
    }

    public get player(): Player {
        return this.gameState.player;
    }

    constructor(
        private readonly gameDataService: GameDataService) {
    }

    public ngOnInit(): void {

        this._airConsole = new AirConsole({"orientation": "landscape"});

        this._airConsole.onReady = (code: string)  => {

            this._isAirConsoleReady = true;

            this.debugLog("onReady", code);

            this.player.deviceId = this._airConsole.getDeviceId();
            this.player.name = this._airConsole.getNickname();

            this._viewManager = new AirConsoleViewManager(this);
        };

        this._airConsole.onMessage = (from: number, data: any) => {

            this.debugLog("onMessage", from, data);

            const messageType: number = data.type;
            switch(messageType) {

            case MessageType.SetTeam:
                this.player.teamId = data.teamId;
                this.player.teamName = data.teamName;

                this._airConsole.setCustomDeviceStateProperty("teamData", data);
                break;
            case MessageType.SetSlot:
                this.gameState.setTeamSlot(data.slotId, data.itemId);
                break;
            case MessageType.ClearSlot:
                this.gameState.clearTeamSlot(data.slotId, data.itemId);
                break;
            default:
                this.debugLog("Invalid message type", messageType);
                break;
            }
        };

        this._airConsole.onCustomDeviceStateChange = (deviceId: number, data: any) => {

            this.debugLog("onCustomDeviceStateChange", deviceId, data);

            this.gameState.update(data);

            this._viewManager.onViewChange(data, (viewId: string) => {
                this.debugLog("onViewChange", viewId);

                if("lobby" === viewId) {
                    this.reset();
                }
            });
        };

        this.debugLog("Requesting game data...");
        this.gameDataService.getGameData()
            .then(gameData => {
                this.debugLog("Received game data", gameData);

                if(AppComponent.CurrentGameDataVersion !== gameData.Version) {
                    this.debugLog("Invalid game data version", gameData.Version, AppComponent.CurrentGameDataVersion);
                    return;
                }
                this.setGameData(gameData);

                this.reset();
            })
            .catch(this.handleError);
    }

    public debugLog(msg: string, ...args: any[]): void {

        if(this._isAirConsoleReady) {
            console.log(this._airConsole.getDeviceId(), msg, ...args);
        } else {
            console.log(msg, ...args);
        }
    }

    private reset(): void {

        this.gameState.reset(this.gameData);
    }

    public startGame(): void {

        this.debugLog("Starting game...");
        this.sendMessageToScreen(new StartGameMessage());
    }

    public confirmStaging(): void {

        this.player.isConfirmed = !this.player.isConfirmed;

        this.debugLog("Confirming staging", this.player.isConfirmed);
        this.sendMessageToScreen(new ConfirmStagingMessage(this.player.isConfirmed));
    }

    public selectSchematicSlot(slotId: number, value: any): void {

        if(value > 0) {
            this.setSlot(slotId, parseInt(value));
        } else {
            this.clearSlot(slotId);
        }
    }

    private setSlot(slotId: number, itemId: number): void {

        this.sendMessageToScreen(new SetSlotMessage(slotId, itemId));

        this.player.schematic.setSlot(slotId, itemId);
    }

    private clearSlot(slotId: number): void {

        this.sendMessageToScreen(new ClearSlotMessage(slotId));

        this.player.schematic.clearSlot(slotId);
    }

    public fireMissiles(): void {

        this.useSpecial(GameData.SpecialTypeMissiles);
        this.gameState.useMissile();
    }

    public launchChaff(): void {

        this.useSpecial(GameData.SpecialTypeChaff);
        this.gameState.useChaff();
    }

    private useSpecial(specialType: string) {

        if(!this.gameState.isGameStarted) {
            return;
        }

        this.debugLog("Using special", specialType);
        this.sendMessageToScreen(new UseSpecialMessage(specialType));
    }

    private sendMessageToScreen(message: IMessage): void {

        this._airConsole.message(AirConsole.SCREEN, message);
    }

    private broadcastMessage(message: IMessage): void {

        this._airConsole.broadcast(message);
    }

    private handleError(error: any): Promise<any> {

        console.error(error);
        return Promise.reject(error.message || error);
    }
}
