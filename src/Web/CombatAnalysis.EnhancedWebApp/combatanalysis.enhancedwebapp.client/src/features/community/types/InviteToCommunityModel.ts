export type InviteToCommunityModel = {
    id: number;
    communityId: number;
    toAppUserId: string;
    when: Date;
    appUserId: string;
}