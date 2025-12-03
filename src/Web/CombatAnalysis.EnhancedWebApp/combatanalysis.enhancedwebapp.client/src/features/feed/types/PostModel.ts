export type PostModel = {
    id: number;
    owner: string;
    content: string;
    createdAt: Date;
    likeCount: number;
    dislikeCount: number;
    commentCount: number;
    appUserId: string;
}