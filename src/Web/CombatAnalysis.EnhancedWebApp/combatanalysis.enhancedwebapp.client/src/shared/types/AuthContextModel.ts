export type AuthContextModel = {
    isAuthenticated: boolean;
    authInProgress: boolean;
    checkAuthAsync: () => Promise<void>;
    logoutAsync: () => Promise<void>;
}