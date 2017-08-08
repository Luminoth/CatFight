import { IItem } from "../item";

export interface IBrainDataEntry extends IItem {

}

export interface IBrainData {

    Brains: IBrainDataEntry[];
}
