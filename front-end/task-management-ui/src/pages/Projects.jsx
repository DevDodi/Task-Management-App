import { useState, useEffect, use } from "react";
import { useNavigate } from "react-router-dom";
import "./css/Projects.css";
import {createProject, getProjects} from "../api/projects.api.js"
import ProjectRow from "../components/ProjectRow";
import Modal from "../components/Modal";

export default function Projects() {

    const navigate = useNavigate();
    const [showModal, setShowModal] = useState(false);
    const [projects, setProjects] = useState([]);

    useEffect(() => {
        const fetchProjects = async () => {
            const response = await getProjects();
            setProjects(response.projects);
        };
        fetchProjects();
    }, []); 

    const onRowClick = (id) => {
        navigate(`/project/${id}`);
    }

    const handleCreate = async () => {
        const projectId = crypto.randomUUID();
        const response = await createProject({Id: projectId, Name: title, Description: description});
        setShowModal(false);
        navigate(`/project/${projectId}`);
    }



    return (
        <div className="projectsContainer">
            <div className="projectsHeader">
                <h1 className="projectsTitle">Projects</h1>
                <button className="createProjectButton" onClick={() => setShowModal(true)}>
                    Create Project
                </button>
            </div>
            <div className="projectsList">
                <div className="projectsListHeader">
                    <p>PROJECT</p>
                    <p>CREATED</p>
                </div>
                {projects.map((project) => (
                    <ProjectRow
                        name={project.name}
                        description={project.description}
                        createdAt={project.createdAt}
                        onClick={() => onRowClick(project.id)}
                    />
                ))}
            </div>
            {showModal && (
                <Modal title="New Project" onClickOutside={() => setShowModal(false)} onClickAction={handleCreate}>
                    <input 
                        className="modalInput"
                        type="text" 
                        placeholder="Enter project name"
                        onChange={(e) => setTitle(e.target.value)}
                    />
                    <textarea 
                        className="modalDescription"
                        placeholder="Enter project description (optional)"
                        onChange={(e) => setDescription(e.target.value)}
                    />
                </Modal>
            )}
        </div>
    );

}