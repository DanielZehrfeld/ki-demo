function convertUtcToLocal(utcTimestamp) {
    const date = new Date(utcTimestamp);
    return date.toLocaleString(); // Converts to local time and formats as a string
}