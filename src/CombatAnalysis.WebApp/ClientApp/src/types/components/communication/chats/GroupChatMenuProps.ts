import { SetStateAction } from "react";
import { GroupChat } from "../../../GroupChat";
import { GroupChatUser } from "../../../GroupChatUser";
import { SelectedChat } from "../SelectedChat";

export interface GroupChatMenuProps {
    me: any;
    setSelectedChat(value: SetStateAction<SelectedChat>): void;
    groupChatUsers: GroupChatUser[];
    groupChatUsersId: string[];
    meInChat: GroupChatUser;
    chat: GroupChat;
    t(key: string): string;
}