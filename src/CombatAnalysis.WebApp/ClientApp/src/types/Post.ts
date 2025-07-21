export type Post = {
    id: number;
    owner: string;
    content: string;
    createdAt: string;
    likeCount: number;
    dislikeCount: number;
    commentCount: number;
    appUserId: string;
}