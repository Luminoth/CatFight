import { GameData } from "./data/gamedata";
import { Schematic } from "./schematic";

export class Player {

    private _deviceId: number;

    public get deviceId(): number {
        return this._deviceId;
    }

    public set deviceId(deviceId: number) {
        this._deviceId = deviceId;
    }

    private _name: string = "Guest";

    public get name(): string {
        return this._name;
    }

    public set name(name: string) {
        this._name = name;
    }

    private _teamId: string = "None";

    public get teamId(): string {
        return this._teamId;
    }

    public set teamId(teamId: string) {
        this._teamId = teamId;
    }

    private _teamName: string = "Unaffiliated";

    public get teamName(): string {
        return this._teamName;
    }

    public set teamName(teamName: string) {
        this._teamName = teamName;
    }

    private _isMasterPlayer: boolean;

    public get isMasterPlayer(): boolean {
        return this._isMasterPlayer;
    }

    private _isConfirmed: boolean;

    public get isConfirmed(): boolean {
        return this._isConfirmed;
    }

    public set isConfirmed(isConfirmed: boolean) {
        this._isConfirmed = isConfirmed;
    }

    private _schematic: Schematic = new Schematic();

    public get schematic(): Schematic {
        return this._schematic;
    }

    public reset(gameData: GameData): void {

        this.schematic.reset(gameData);

        this.isConfirmed = false;
    }

    public update(data: any): void {

        if(!data.masterPlayer) {
            return;
        }
        this._isMasterPlayer = data.masterPlayer === this.deviceId;
    }
}
