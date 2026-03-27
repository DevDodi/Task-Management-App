import { apiClient } from './httpClient.js';

export async function getTaskLogs(id) {
  return apiClient.get(`/tasklogs/${id}`);
}

export async function getTaskLogsRange(id, startDate, endDate) {
  return apiClient.get(`/tasklogs/${id}/${startDate}/${endDate}`);
}
