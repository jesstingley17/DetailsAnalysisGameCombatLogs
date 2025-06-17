import { SetStateAction } from "react";
import { SelectedChat } from "../SelectedChat";
import { GroupChatUser } from "../../../GroupChatUser";

export interface GroupChatListItemProps {
    meInChat: GroupChatUser;
    setSelectedGroupChat(value: SetStateAction<SelectedChat>): void;
    subscribeToUnreadGroupMessagesUpdated(callback: any): void;
}