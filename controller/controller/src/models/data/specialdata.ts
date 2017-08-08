import { IItem } from "../item";

export interface ISpecialDataEntry extends IItem {

}

export interface ISpecialData {

    Specials: ISpecialDataEntry[];
}
