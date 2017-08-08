import { IMessage, MessageType } from "./message";

export class ConfirmStagingMessage implements IMessage {

    public type: MessageType = MessageType.ConfirmStaging;

    public isConfirmed: boolean;

    constructor(confirmed: boolean) {

        this.isConfirmed = confirmed;
    }
}
