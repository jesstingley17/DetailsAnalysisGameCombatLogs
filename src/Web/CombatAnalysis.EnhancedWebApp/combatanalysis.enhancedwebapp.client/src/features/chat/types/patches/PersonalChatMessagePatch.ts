import type { MessageStatus } from '../enums/MessageStatus';

export type PersonalChatMessagePatch = {
    id: number;
    message?: string;
    status?: MessageStatus;
    markedType?: number;
}