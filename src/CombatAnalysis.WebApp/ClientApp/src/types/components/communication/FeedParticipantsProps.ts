import { AppUser } from "../../AppUser";

export interface FeedParticipantsProps {
    myself: AppUser;
    t(key: string): string;
}