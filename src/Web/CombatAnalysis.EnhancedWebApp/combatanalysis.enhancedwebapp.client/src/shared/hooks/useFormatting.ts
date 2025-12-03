
const useFormatting = () => {
    const dateFormatting = (stringOfDate: string): string => {
        const date = new Date(stringOfDate);
        const month = date.getMonth();
        const monthes = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

        const formatted = `${date.getDate()} ${monthes[month]}, ${date.getHours()}:${date.getMinutes()}`;

        return formatted;
    }

    return { dateFormatting };
}

export default useFormatting;