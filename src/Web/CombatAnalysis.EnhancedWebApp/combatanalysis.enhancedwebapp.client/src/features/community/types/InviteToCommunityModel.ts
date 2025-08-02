export type InviteToCommunityModel = {
    id: number;
    communityId: number;
    toAppUserId: string;
    when: string;
    appUserId: string;
}