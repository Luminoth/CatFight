// NOTE: this *must* match the values from Unity
export const enum MessageType {

    None = 0,

    // unstaged
    SetTeam,

    // lobby
    StartGame,

    // staging
    ConfirmStaging,
    SetSlot,
    ClearSlot,

    // arena
    UseSpecial
}

export interface IMessage {

    type: MessageType;
}
