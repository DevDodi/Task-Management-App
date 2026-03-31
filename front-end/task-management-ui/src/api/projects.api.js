import { apiClient } from './httpClient.js';

export async function getProjectById(id) {
  return apiClient.get(`/projects/${id}`);
}

export async function getAllProjects() {
  return apiClient.get('/projects');
}

export async function getUnassignedProjects() {
  return apiClient.get('/projects?unassigned=true');
}

export async function getOwnedByProjects(id) {
  return apiClient.get(`/projects?ownerId=${id}`);
}

export async function createProject(project) {
  return apiClient.post('/projects', project);
}

export async function updateProject(id, project){
  return apiClient.patch(`/projects/${id}`, project)
}

export async function deleteProject(id){
  return apiClient.delete(`/projects/${id}`)
}

