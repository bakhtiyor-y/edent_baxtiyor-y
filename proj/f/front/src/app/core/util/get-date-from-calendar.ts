export function getDateFromCalendar(date: any) {
    const day = date.getDate();
    const month = date.getMonth();
    const year = date.getFullYear();
    return new Date(year, month, day);
}

export function getDateTime(time: string): Date {
    let tmp = time.split(':');
    const date = new Date();
    date.setHours(parseInt(tmp[0]));
    date.setMinutes(parseInt(tmp[1]));
    return date;
}

export function getTimeString(date: Date): string {
    let time = `${date.getHours().toString().padStart(2, '0')}:${date.getMinutes().toString().padStart(2, '0')}`;
    return time;
}
