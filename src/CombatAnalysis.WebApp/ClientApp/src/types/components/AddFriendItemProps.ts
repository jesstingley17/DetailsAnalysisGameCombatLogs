import { AppUser } from "../AppUser";

export interface AddFriendItemProps {
    friendUserId: string;
    addUserToList(user: AppUser): void;
    removeUserToList(user: AppUser): void;
    filterContent: string;
    peopleIdToJoin: AppUser[];
}