export function formatDateTime(utcString) {
  const date = new Date(utcString);
  const now = new Date();

  const isToday = date.toDateString() === now.toDateString();
  const yesterday = new Date(now);
  yesterday.setDate(now.getDate() - 1);
  const isYesterday = date.toDateString() === yesterday.toDateString();

  const time = date.toLocaleTimeString(undefined, { hour: "2-digit", minute: "2-digit" });

  if (isToday) return `Today ${time}`;
  if (isYesterday) return `Yesterday ${time}`;
  return date.toLocaleDateString(undefined, { day: "numeric", month: "short" }) + ` ${time}`;
}


export function truncateText(text, limit) {
    return text?.length > limit ? text.slice(0, limit) + "..." : text;
}