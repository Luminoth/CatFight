import { IMessage, MessageType } from "./message";

export class UseSpecialMessage implements IMessage {

    public type: MessageType = MessageType.UseSpecial;

    public specialType: string;

    constructor(specialType: string) {

        this.specialType = specialType;
    }
}
