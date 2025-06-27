import { SetStateAction } from "react";
import { GroupChatUser } from "../../../GroupChatUser";
import { AppUser } from "../../../AppUser";

export interface GroupChatMembersItemProps {
    myself: AppUser;
    groupChatUser: GroupChatUser;
    usersToRemove: GroupChatUser[];
    setUsersToRemove(value: SetStateAction<GroupChatUser[]>): void;
    showRemoveUser: boolean;
}