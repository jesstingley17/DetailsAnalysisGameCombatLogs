import { AppUser } from "../../../AppUser";
import { GroupChatMessage } from "../../../GroupChatMessage";
import { PersonalChatMessage } from "../../../PersonalChatMessage";

export interface ChatMessageTitleProps {
    me: AppUser;
    itIsMe: boolean;
    message: PersonalChatMessage | GroupChatMessage;
    meInChatId: string;
}