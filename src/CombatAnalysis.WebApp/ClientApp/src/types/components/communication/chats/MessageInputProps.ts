import { SetStateAction } from "react";
import { AppUser } from "../../../AppUser";
import { GroupChatUser } from "../../../GroupChatUser";

export interface MessageInputProps {
    chatId: number;
    IasGroupChatUser: GroupChatUser | AppUser;
    setAreLoadingOldMessages(value: SetStateAction<boolean>): void;
    targetChatType: number;
    companionsId: string[];
    t(key: string): string;
}