import { useState, useEffect, use } from "react";
import { useNavigate } from "react-router-dom";
import "./css/Projects.css";
import {createProject, getOwnedByProjects, getUnassignedProjects, updateProject} from "../api/projects.api.js"
import { getUser, getUsers } from "../api/users.api.js";
import ProjectRow from "../components/ProjectRow";
import Modal from "../components/Modal";

export default function Projects() {

    const navigate = useNavigate();
    const [showCreateModal, setShowCreateModal] = useState(false);
    const [showEditModal, setShowEditModal] = useState(false);
    const [users, setUsers] = useState([]);
    const [projects, setProjects] = useState([]);
    const [selectedProject, setSelectedProject] = useState(null);
    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [assignee, setAssignee] = useState('00000000-0000-0000-0000-000000000000');

    const userId = localStorage.getItem("userId");

    const fetchProjects = async () => {
        let projectList = [];
        const ownedResponse = await getOwnedByProjects(userId);
        const unassignedResponse = await getUnassignedProjects();

        projectList = ownedResponse.projects;
        projectList = projectList.concat(unassignedResponse.projects);

        setProjects(projectList);
    };

    const fetchUsers = async () => {
        const userResponses = await getUsers();
        setUsers(userResponses.users);
    }

    const onRowClick = (id) => {
        navigate(`/projects/${id}`);
    }

    const handleCreate = async () => {
        const projectId = crypto.randomUUID();
        const response = await createProject({Id: projectId, Name: name, Description: description, OwnerId: assignee});
        setShowCreateModal(false);
        navigate(`/projects/${projectId}`);
    }

    const handleUpdate = async () => {
        const response = await updateProject(selectedProject.id, {Id: selectedProject.id, Name: name, Description: description, OwnerId: assignee});
        setShowEditModal(false);
        setName("");
        setDescription("");
        setAssignee('00000000-0000-0000-0000-000000000000');
        fetchProjects();
    }

    useEffect(() => {
        fetchProjects();
        fetchUsers();
    }, []); 

    return (
        <div className="projectsContainer">
            <div className="projectsHeader">
                <h1 className="projectsTitle"> My Projects</h1>
                <button className="createProjectButton" onClick={() => setShowCreateModal(true)}>
                    Create Project
                </button>
            </div>
            <div className="projectsList">
                <div className="projectsListHeader">
                    <p>PROJECT</p>
                    <p>OWNER</p>
                </div>
                {projects.map((project) => (
                    <ProjectRow
                        key={project.id}
                        name={project.name}
                        description={project.description}
                        owner={users.find(u => u.id === project.ownerId)?.email ?? "Unassigned"}
                        onClick={() => onRowClick(project.id)}
                        onEdit={() => {
                            setName(project.name);
                            setDescription(project.description || "");
                            setAssignee(project.ownerId);
                            setShowEditModal(true);
                            setSelectedProject(project);
                        }}
                    />
                ))}
            </div>
            {showCreateModal && (
                <Modal title="New Project" onClickOutside={() => setShowCreateModal(false)} onClickAction={handleCreate}>
                    <label className="modalLabel">Project Name</label>
                    <input 
                        className="modalInput"
                        type="text" 
                        placeholder="Enter project name"
                        onChange={(e) => setName(e.target.value)}
                    />
                    <label className="modalLabel">Project Description</label>
                    <textarea 
                        className="modalDescription"
                        placeholder="Enter project description (optional)"
                        onChange={(e) => setDescription(e.target.value)}
                    />
                    <label className="modalLabel">Owner's Email</label>
                    <select className="modalAssignee" onChange={(e) => setAssignee((e.target.value))}>
                        {users.map((u) => (
                            <option key={u.id} value={u.id}>
                                {u.email}
                            </option>
                        ))}
                        <option key={'00000000-0000-0000-0000-000000000000'} value={'00000000-0000-0000-0000-000000000000'}>
                            Unassigned
                        </option>
                    </select>
                </Modal>
            )}

            {showEditModal && (
                <Modal title="Edit Project" onClickOutside={() => setShowEditModal(false)} onClickAction={handleUpdate}>
                    <label className="modalLabel">Project Name</label>
                    <input 
                        className="modalInput"
                        type="text" 
                        placeholder="Enter project name"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                    />
                    <label className="modalLabel">Project Description</label>
                    <textarea 
                        className="modalDescription"
                        placeholder="Enter project description (optional)"
                        value={description}
                        onChange={(e) => setDescription(e.target.value)}
                    />
                    <label className="modalLabel">Owner's Email</label>
                    <select className="modalAssignee" onChange={(e) => setAssignee((e.target.value))}>
                        {users.map((u) => (
                            <option key={u.id} value={u.id}>
                                {u.email}
                            </option>
                        ))}
                        <option key={'00000000-0000-0000-0000-000000000000'} value={'00000000-0000-0000-0000-000000000000'}>
                            Unassigned
                        </option>
                    </select>
                </Modal>
            )}
        </div>
    );

}