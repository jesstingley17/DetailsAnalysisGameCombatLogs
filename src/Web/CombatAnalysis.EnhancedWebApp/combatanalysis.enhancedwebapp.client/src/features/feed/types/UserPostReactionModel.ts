import type { PostReactionModel } from './PostReactionModel';

export interface UserPostReactionModel extends PostReactionModel {
    userPostId: number;
}