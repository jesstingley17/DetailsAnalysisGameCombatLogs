import type { PostReactionModel } from './PostReactionModel';

export interface CommunityPostReactionModel extends PostReactionModel {
    communityPostId: number;
    communityId: number;
}