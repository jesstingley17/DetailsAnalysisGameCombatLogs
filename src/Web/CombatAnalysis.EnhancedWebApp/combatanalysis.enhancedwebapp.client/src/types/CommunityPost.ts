import { Post } from "./Post";

export interface CommunityPost extends Post {
    communityName: string;
    postType: number;
    publicType: number;
    restrictions: number;
    tags: string;
    communityId: number;
}