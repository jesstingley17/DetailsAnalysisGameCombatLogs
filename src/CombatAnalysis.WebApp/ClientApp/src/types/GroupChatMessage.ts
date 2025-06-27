export interface GroupChatMessage {
    id: number;
    username: string;
    message: string;
    time: string;
    status: number;
    type: number;
    markedType: number;
    isEdited: boolean;
    chatId: number;
    groupChatUserId: string;
    groupChatMessageId?: number;
}