import { apiClient } from '../httpClient.js';

export async function getProjectById(id) {
  return apiClient.get(`/projects/${id}`);
}

export async function createProject(project) {
  return apiClient.post('/projects', project);
}

export async function updateProject(id, project){
  return apiClient.post(`/projects/${id}`, project)
}

export async function deleteProject(id){
  return apiClient.delete(`/projects/${id}`)
}

