import { apiClient } from './httpClient.js';

export async function getTaskById(id) {
  return apiClient.get(`/tasks/${id}`);
}

export async function getTasks() {
  return apiClient.get('/tasks');
}

export async function getProjectTasks(projectId) {
  return apiClient.get(`/tasks?projectId=${projectId}`);
}

export async function createTask(task) {
  return apiClient.post('/tasks', task);
}

export async function updateTask(id, task){
  return apiClient.put(`/tasks/${id}`, task)
}

export async function deleteTask(id){
  return apiClient.delete(`/tasks/${id}`)
}