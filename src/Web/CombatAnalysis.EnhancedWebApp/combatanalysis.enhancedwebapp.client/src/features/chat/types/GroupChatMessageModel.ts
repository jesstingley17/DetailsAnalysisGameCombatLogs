
export type GroupChatMessageModel = {
    id: number;
    username: string;
    message: string;
    time: Date;
    status?: number;
    type: number;
    markedType?: number;
    isEdited?: boolean;
    chatId: number;
    groupChatUserId: string;
    groupChatMessageId?: number;
}