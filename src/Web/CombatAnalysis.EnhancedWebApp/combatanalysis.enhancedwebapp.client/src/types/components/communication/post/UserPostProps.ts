import { AppUser } from '../../../AppUser';
import { UserPost } from '../../../UserPost';

export interface UserPostProps {
    myself: AppUser;
    post: UserPost;
}