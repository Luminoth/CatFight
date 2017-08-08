import { IItem } from "../item";

export interface IWeaponDataEntry extends IItem {

}

export interface IWeaponData {

    Weapons: IWeaponDataEntry[];
}
