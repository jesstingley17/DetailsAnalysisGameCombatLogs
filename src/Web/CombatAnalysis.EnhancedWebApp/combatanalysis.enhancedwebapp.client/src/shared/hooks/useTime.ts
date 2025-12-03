type Time = {
    getTimeWithoutMs: (time: string) => string;
    getSeconds: (time: string) => number;
    getDuration: (time1: string, time2: string) => string
}

const useTime = (): Time => {
    const getTimeWithoutMs = (time: string): string => {
        const ms = time.indexOf('.');
        const timeWithoutMs = time.substring(0, ms);

        return timeWithoutMs;
    }

    const getSeconds = (time: string): number => {
        const timeElementsByTime = time.split(':');
        const hoursByTime = +timeElementsByTime[0];
        const minutesByTime = (hoursByTime * 60) + +timeElementsByTime[1];
        const secondsByTime = (minutesByTime * 60) + +timeElementsByTime[2];

        return secondsByTime;
    }

    const getDuration = (time1: string, time2: string): string => {
        const secondsByTime1 = getSeconds(time1);
        const secondsByTime2 = getSeconds(time2);

        let durationToMinutes = 0;
        let durationToHours = 0;
        let durationToSeconds = secondsByTime1 - secondsByTime2;

        if (durationToSeconds > 60) {
            durationToMinutes = Math.trunc(durationToSeconds / 60);
            durationToSeconds -= durationToMinutes * 60;
        }

        if (durationToMinutes > 60) {
            durationToHours = Math.trunc(durationToMinutes / 60);
            durationToMinutes -= durationToHours * 60;
        }

        const duration = `${durationToHours}:${durationToMinutes}:${durationToSeconds > 9 ? durationToSeconds : `0${durationToSeconds}`}`;

        return duration;
    }

    return { getTimeWithoutMs, getSeconds, getDuration };
}

export default useTime;