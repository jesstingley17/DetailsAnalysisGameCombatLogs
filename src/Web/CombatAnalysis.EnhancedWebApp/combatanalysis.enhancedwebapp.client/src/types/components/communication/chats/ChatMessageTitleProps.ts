import { AppUser } from "../../../AppUser";
import { GroupChatMessage } from "../../../GroupChatMessage";
import { PersonalChatMessage } from "../../../PersonalChatMessage";

export interface ChatMessageTitleProps {
    myself: AppUser;
    itIsMe: boolean;
    message: PersonalChatMessage | GroupChatMessage;
    chatUserAsUserId: string;
    chatUserUsername: string;
}