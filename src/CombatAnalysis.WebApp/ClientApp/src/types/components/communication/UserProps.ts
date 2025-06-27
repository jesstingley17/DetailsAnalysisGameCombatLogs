import { SetStateAction } from "react";
import { AppUser } from "../../AppUser";

export interface UserProps {
    myself: AppUser;
    targetUserId: string;
    targetUsername: string;
    setUserInformation(value: SetStateAction<any>): void;
    friendId?: number | 0;
}