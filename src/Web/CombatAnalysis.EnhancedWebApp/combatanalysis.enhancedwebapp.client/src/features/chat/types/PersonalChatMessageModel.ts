import type { MessageStatus } from './enums/MessageStatus';

export type PersonalChatMessageModel = {
    id: number;
    username: string;
    message: string;
    time: Date;
    status: MessageStatus;
    type: number;
    markedType: number;
    isEdited: boolean;
    personalChatId: number;
    appUserId: string;
}