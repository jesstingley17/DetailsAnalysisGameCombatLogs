import { SetStateAction } from "react";
import { GroupChatUser } from "../../../GroupChatUser";

export interface GroupChatMembersProps {
    me: any;
    communicationUsers: GroupChatUser[];
    removeUsersAsync(peopleToRemove: GroupChatUser[]): Promise<void>;
    setShowMembers(value: SetStateAction<boolean>): void;
    isPopup: boolean;
    canRemovePeople(): boolean;
}