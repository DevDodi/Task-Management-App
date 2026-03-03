import { apiClient } from '../httpClient.js';

export async function getProjects() {
  return apiClient.get('/projects');
}

export async function getProjectById(id) {
  return apiClient.get(`/projects/${id}`);
}

export async function createProject(project) {
  return apiClient.post('/projects', project);
}