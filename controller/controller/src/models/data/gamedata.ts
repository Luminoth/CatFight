import { IArmorData } from "./armordata";
import { IBrainData } from "./braindata";
import { IFighterData } from "./fighterdata";
import { ISpecialData } from "./specialdata";
import { IWeaponData } from "./weapondata";
import { IItem } from "../item";

export interface IGameData {

    Version: number;

    Fighter: IFighterData;

    Brains: IBrainData;

    Armor: IArmorData;

    Weapons: IWeaponData;

    Specials: ISpecialData;
}

export class GameData implements IGameData {

    public static readonly ItemTypeBrain: string = "Brain";
    public static readonly ItemTypeArmor: string = "Armor";
    public static readonly ItemTypeWeapon: string = "Weapon";
    public static readonly ItemTypeSpecial: string = "Special";

    public static readonly SpecialTypeMissiles: string = "Missiles";
    public static readonly SpecialTypeChaff: string = "Chaff";

    public static create(gd: IGameData): GameData {

        const gameData: GameData = new GameData();
        gameData.Version = gd.Version;
        gameData.Fighter = gd.Fighter;
        gameData.Brains = gd.Brains;
        gameData.Armor = gd.Armor;
        gameData.Weapons = gd.Weapons;
        gameData.Specials = gd.Specials;
        return gameData;
    }

    public Version: number;

    public Fighter: IFighterData;

    public Brains: IBrainData;

    public Armor: IArmorData;

    public Weapons: IWeaponData;

    public Specials: ISpecialData;

    public getItems(type: string): IItem[] {

        switch(type) {

        case GameData.ItemTypeBrain:
            return this.Brains.Brains;
        case GameData.ItemTypeArmor:
            return this.Armor.Armor;
        case GameData.ItemTypeWeapon:
            return this.Weapons.Weapons;
        case GameData.ItemTypeSpecial:
            return this.Specials.Specials;
        }

        //app.debugLog("Unknown item type", type);
        return null;
    }
}
