import type { MessageStatus } from '../enums/MessageStatus';

export type ChatMessagePatch = {
    id: number;
    chatId: number;
    message?: string;
    status?: MessageStatus;
    markedType?: number;
}