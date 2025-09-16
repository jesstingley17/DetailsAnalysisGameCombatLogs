/* eslint-disable @typescript-eslint/no-explicit-any */
const API_URL = "/api/v1/Logs";

const sendLog = async (level: string, message: string, context?: any) => {
    try {
        await fetch(API_URL, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ level, message, context }),
        });
    } catch (err) {
        console.error("Failed to send log:", err);
    }
}

const logger = {
    info: (message: string, context?: any) => {
        console.info(message, context);
        sendLog("info", message, context);
    },
    warn: (message: string, context?: any) => {
        console.warn(message, context);
        sendLog("warn", message, context);
    },
    error: (message: string, context?: any) => {
        console.error(message, context);
        sendLog("error", message, context);
    },
}

export default logger;