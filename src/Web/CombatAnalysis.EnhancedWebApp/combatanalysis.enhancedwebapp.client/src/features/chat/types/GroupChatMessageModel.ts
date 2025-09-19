import type { MessageStatus } from './enums/MessageStatus';

export type GroupChatMessageModel = {
    id: number;
    username: string;
    message: string;
    time: Date;
    status: MessageStatus;
    type: number;
    markedType: number;
    isEdited: boolean;
    groupChatId: number;
    groupChatUserId: string;
}