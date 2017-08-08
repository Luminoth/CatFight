import { GameData } from "./data/gamedata";
import { Player } from "./player";

export class GameState {

    private _player: Player = new Player();

    public get player(): Player {
        return this._player;
    }

    private _isGameStarted: boolean;

    public get isGameStarted(): boolean {
        return this._isGameStarted;
    }

    private _teamSlots: { [id: number]: number }[];

    private _missilesRemaining: number;

    public get missilesRemaining(): number {
        return this._missilesRemaining;
    }

    private _chaffRemaining: number;

    public get chaffRemaining(): number {
        return this._chaffRemaining;
    }

    public reset(gameData: GameData): void {

        this.player.reset(gameData);

        this._isGameStarted = false;
        this._teamSlots = gameData.Fighter ? new Array<{ [id: number]: number }>(gameData.Fighter.Schematic.Slots.length).fill({}) : [];
        this._missilesRemaining = 0;
        this._chaffRemaining = 0;
    }

    public update(data: any): void {

        this.player.update(data);

        if(!data.gameState) {
            return;
        }

        this._isGameStarted = data.gameState.isGameStarted;

        // TODO: add an interface for the game state and fighter state
        const fighterState: any = data.gameState.fighterState[this.player.teamId];
        this._missilesRemaining = fighterState ? fighterState.specialsRemaining[GameData.SpecialTypeMissiles] : 0;
        this._chaffRemaining = fighterState ? fighterState.specialsRemaining[GameData.SpecialTypeChaff] : 0;
    }

    public setTeamSlot(slotId: number, itemId: number): void {

        const slotIdx: number = slotId - 1;
        const itemIdx: number = itemId - 1;

        const slot: { [id: number]: number } = this._teamSlots[slotIdx];
        if(!(itemIdx in slot)) {
            slot[itemIdx] = 0;
        }

        //app.debugLog("Setting team slot", slotId, itemId, slot[itemIdx]);
        ++slot[itemIdx];
    }

    public clearTeamSlot(slotId: number, itemId: number): void {

        const slotIdx: number = slotId - 1;
        const itemIdx: number = itemId - 1;

        const slot: { [id: number]: number } = this._teamSlots[slotIdx];
        if(!(itemIdx in slot)) {
            slot[itemIdx] = 1;
        }

        //app.debugLog("Clearing team slot", slotId, itemId, slot[itemIdx]);
        --slot[itemIdx];
    }

    public useMissile(): void {

        --this._missilesRemaining;
    }

    public useChaff(): void {

        --this._chaffRemaining;
    }
}
