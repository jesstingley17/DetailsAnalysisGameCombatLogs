export type AuthContextModel = {
    isAuthenticated: boolean;
    checkAuthAsync: () => Promise<void>;
    logoutAsync: () => Promise<void>;
}