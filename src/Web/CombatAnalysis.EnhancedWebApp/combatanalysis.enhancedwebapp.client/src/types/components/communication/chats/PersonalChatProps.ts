import { SetStateAction } from "react";
import { AppUser } from "../../../AppUser";
import { PersonalChat } from "../../../PersonalChat";
import { SelectedChat } from "../SelectedChat";

export interface PersonalChatProps {
    myself: AppUser;
    chat: PersonalChat;
    setSelectedChat(value: SetStateAction<SelectedChat>): void;
    companionId: string;
}