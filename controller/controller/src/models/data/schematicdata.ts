export interface ISchematicSlotData {

    Id: number;

    Type: string;

    Name: string;
}

export interface ISchematicData {

    MaxFilledSlots: number;

    Slots: ISchematicSlotData[];
}
