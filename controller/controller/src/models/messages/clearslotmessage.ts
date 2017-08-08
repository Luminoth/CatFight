import { IMessage, MessageType } from "./message";

export class ClearSlotMessage implements IMessage {

    public type: MessageType = MessageType.ClearSlot;

    public slotId: number;

    constructor(slotId: number) {

        this.slotId = slotId;
    }
}
