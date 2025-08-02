import { SetStateAction } from "react";
import { AppUser } from "../../../AppUser";
import { GroupChat } from "../../../GroupChat";
import { SelectedChat } from "../SelectedChat";

export interface GroupChatProps {
    myself: AppUser;
    chat: GroupChat;
    setSelectedChat(value: SetStateAction<SelectedChat>): void;
}