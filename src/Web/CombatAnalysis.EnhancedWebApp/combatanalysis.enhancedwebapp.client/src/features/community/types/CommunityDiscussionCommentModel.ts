export type CommunityDiscussionCommentModel = {
    id: number;
    content: string;
    when: Date;
    appUserId: string;
    communityDiscussionId: number;
}