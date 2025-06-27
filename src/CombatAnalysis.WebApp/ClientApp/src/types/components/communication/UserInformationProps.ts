import { AppUser } from "../../AppUser";

export interface UserInformationProps {
    myself: AppUser;
    personId: string;
    closeUserInformation(): void;
}