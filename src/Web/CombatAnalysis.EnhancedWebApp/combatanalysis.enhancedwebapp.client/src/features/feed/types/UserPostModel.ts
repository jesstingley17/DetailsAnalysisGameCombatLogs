import type { PostModel } from "./PostModel";

export interface UserPostModel extends PostModel {
    publicType: number;
    tags: string;
}