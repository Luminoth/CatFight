import { IMessage, MessageType } from "./message";

export class StartGameMessage implements IMessage {

    public type: MessageType = MessageType.StartGame;
}
