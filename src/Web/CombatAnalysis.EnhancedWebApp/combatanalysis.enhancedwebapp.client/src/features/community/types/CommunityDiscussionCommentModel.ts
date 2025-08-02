export type CommunityDiscussionCommentModel = {
    id: number;
    content: string;
    when: string;
    appUserId: string;
    communityDiscussionId: number;
}