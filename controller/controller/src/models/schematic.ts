import { GameData } from "./data/gamedata";
import { IItem } from "./item";

export class Schematic {

    private _slots: { [id: number]: number };

    private _filledSlots: number;

    public get filledSlots(): number {
        return this._filledSlots;
    }

    private _slotItems: IItem[][];

    public get slotItems(): IItem[][] {
        return this._slotItems;
    }

    public reset(gameData: GameData): void {

        this._slots = gameData.Fighter ? new Array<number>(gameData.Fighter.Schematic.Slots.length).fill(0) : [];
        this._filledSlots = 0;

        this._slotItems = [];
        if(gameData.Fighter) {

            gameData.Fighter.Schematic.Slots.forEach((slot) => {

                const idx: number = this._slotItems.push([]) - 1;
                gameData.getItems(slot.Type).forEach(item => {
                    this._slotItems[idx].push(item);
                });
            });
        }
    }

    public getSlotItems(slotId: number): IItem[] {

        const slotIdx: number = slotId - 1;
        return this._slotItems[slotIdx];
    }

    public isMaxSlotsSet(gameData: GameData): boolean {
        return this.filledSlots >= gameData.Fighter.Schematic.MaxFilledSlots;
    }

    public isSlotSet(slotId: number): boolean {

        const slotIdx: number = slotId - 1;
        return this._slots[slotIdx] !== 0;
    }

    public isSlotItem(slotId: number, itemId: number): boolean {

        const slotIdx: number = slotId - 1;
        return this._slots[slotIdx] === itemId;
    }

    public setSlot(slotId: number, itemId: number): void {

        if(!this.isSlotSet(slotId)) {
            ++this._filledSlots;
        }

        const slotIdx: number = slotId - 1;
        this._slots[slotIdx] = itemId;
    }

    public clearSlot(slotId: number): void {

        if(this.isSlotSet(slotId)) {
            --this._filledSlots;
        }

        const slotIdx: number = slotId - 1;
        this._slots[slotIdx] = 0;
    }
}
