export type AppConfig = typeof import('../../public/config.json');

export const loadConfig = async (): Promise<AppConfig> => {
    const response = await fetch("/config.json");
    if (!response.ok) throw new Error("Failed to load config.json");
    return response.json();
}