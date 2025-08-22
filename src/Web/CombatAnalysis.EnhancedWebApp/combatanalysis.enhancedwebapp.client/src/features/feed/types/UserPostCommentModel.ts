export type UserPostCommentModel = {
    id: number;
    content: string;
    createdAt: Date;
    userPostId: number;
    appUserId: string;
}