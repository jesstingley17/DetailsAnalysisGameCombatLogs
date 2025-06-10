import { SetStateAction } from "react";
import { AppUser } from "../../../AppUser";
import { Community } from "../../../Community";

export interface CommunityMenuProps {
    setShowMenu(value: SetStateAction<boolean>): void;
    user: AppUser;
    community: Community;
    setCommunity(value: SetStateAction<Community>): void;
}