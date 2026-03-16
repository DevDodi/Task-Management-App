import { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import {getProjectById} from "../api/projects.api.js"
import {getProjectTasks} from "../api/tasks.api.js"
import Modal from "../components/Modal";
import TaskRow from "../components/TaskRow";
import "./css/ProjectDetails.css";


export default function ProjectDetails() {

    const {id} = useParams();
    const [selectedProject, setSelectedProject] = useState(null);
    const [tasks, setTasks] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");    
    const [auditLogs, setAuditLogs] = useState([]);
    const [selectedTask, setSelectedTask] = useState(null);
    const [showCreateModal, setShowCreateModal] = useState(null);
    const [showEditModal, setShowEditModal] = useState(null);
    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");

    useEffect(() => {
        const fetchProjectAndTasks = async () => {
            try {
                let response = await getProjectById(id);
                setSelectedProject(response.project);

                response = await getProjectTasks(response.project.id);
                setTasks(response.tasks);
            } catch (err) {
                setError("Error fetching project and tasks");
            } finally {
                setLoading(false);
            }
        };
        fetchProjectAndTasks();
    }, [id]);

    const handleCreate = async () => {
        // const projectId = crypto.randomUUID();
        // const response = await createProject({Id: projectId, Name: title, Description: description, OwnerId: userId});
        // setShowCreateModal(false);
        // navigate(`/projects/${projectId}`);
    }

    const handleUpdate = async () => {
        // const projectId = crypto.randomUUID();
        // const response = await createProject({Id: projectId, Name: title, Description: description, OwnerId: userId});
        // setShowCreateModal(false);
        // navigate(`/projects/${projectId}`);
    }

    if (loading) return <div className="detailsContainer">Loading...</div>;
    if (error) return <div className="detailsContainer">{error}</div>;

    return (
        <div className="detailsContainer">
            <div className="detailsHeader">
                <div className="detailsTitle">
                    <h1 style={{fontSize: "24px"}}>
                        My Projects
                    </h1>
                    <h1 className="titleSeperator">/</h1>
                    <h1 className="projectName">
                        {selectedProject === null ? id : selectedProject.name}
                    </h1>
                </div>
                <button className="createTaskButton" onClick={() => setShowCreateModal(true)}>
                    Create Task
                </button>
            </div>
            <div className="detailsBody">
                <div className="tasksList">
                    <div className="tasksListHeader">
                        <p>Task</p>
                        <p>Assignee</p>
                        <p>Status</p>
                    </div>
                    {tasks.map((task) => (
                        <TaskRow
                            key={task.id}
                            name={task.title}
                            description={task.description ?? ""}
                            assignee= "Alice"
                            status={task.status}
                            onClick={() => setSelectedTask(task)}
                            onEdit={() => setShowEditModal(true)}
                        />
                    ))}
                </div>

                {selectedTask && (
                    <div className="auditContainer">
                        <div className="auditHeader">
                            <p className="auditHeader-label">Selected Task</p>
                            <p className="auditHeader-title">{selectedTask.title}</p>
                        </div>
                        <div className="auditLogs">
                            <p className="auditLogs-label">Audit Log</p>
                            <div className="auditLogs-items">
                                {auditLogs.map((log, index) => (
                                    <div key={index} className="auditLogs-item">
                                        <span className="auditLogs-item-time">time</span>
                                        <p className="auditLogs-item-text">descrip</p>
                                    </div>
                                ))}
                            </div>
                        </div>
                    </div>
                )}
            </div>
            {showCreateModal && (
                <Modal title="New Task" onClickOutside={() => setShowCreateModal(false)} onClickAction={handleCreate}>
                    <input 
                        className="modalInput"
                        type="text" 
                        placeholder="Enter task name"
                        onChange={(e) => setTitle(e.target.value)}
                    />
                    <textarea 
                        className="modalDescription"
                        placeholder="Enter task description (optional)"
                        onChange={(e) => setDescription(e.target.value)}
                    />
                </Modal>
            )}

            {showEditModal && (
                <Modal title="Edit Task" onClickOutside={() => setShowEditModal(false)} onClickAction={handleUpdate}>
                    
                </Modal>
            )}

        </div>
    );
}