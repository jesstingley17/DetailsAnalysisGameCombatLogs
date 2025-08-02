import { Post } from "./Post";

export interface UserPost extends Post {
    publicType: number;
    tags: string;
}