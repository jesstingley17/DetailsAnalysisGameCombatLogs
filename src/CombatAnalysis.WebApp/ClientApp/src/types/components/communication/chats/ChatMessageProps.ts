import { AppUser } from '../../../AppUser';
import { GroupChatMessage } from '../../../GroupChatMessage';
import { PersonalChatMessage } from '../../../PersonalChatMessage';

export type ChatMessageProps = {
    chatType: number;
    myself: AppUser;
    chatUserAsUserId: string;
    chatUserUsername: string;
    reviewerId: string;
    messageOwnerId: string;
    message: PersonalChatMessage | GroupChatMessage;
    updateMessageAsync(message: PersonalChatMessage | GroupChatMessage): Promise<void>;
    hubConnection: any;
    subscribeToChatMessageHasBeenRead(callback: any): void;
}