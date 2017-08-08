import { IMessage, MessageType } from "./message";

export class SetSlotMessage implements IMessage {

    public type: MessageType = MessageType.SetSlot;

    public slotId: number;

    public itemId: number;

    constructor(slotId: number, itemId: number) {

        this.slotId = slotId;
        this.itemId = itemId;
    }
}
