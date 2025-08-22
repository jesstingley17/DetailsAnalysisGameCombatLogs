export type CommunityPostCommentModel = {
    id: number;
    content: string;
    commentType: number;
    createdAt: Date;
    communityPostId: number;
    communityId: number;
    appUserId: string;
}