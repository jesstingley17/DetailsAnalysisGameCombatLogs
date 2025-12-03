export type CommunityDiscussionModel = {
    id: number;
    title: string;
    content: string;
    when: Date;
    appUserId: string;
    communityId: number;
}