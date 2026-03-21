import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { getProjectById } from "../api/projects.api.js";
import { createTask, getProjectTasks, updateTask } from "../api/tasks.api.js";
import { getUser } from "../api/users.api.js";
import Modal from "../components/Modal";
import TaskRow from "../components/TaskRow";
import {getStatusLabel} from "../components/TaskRow";
import {TaskStatus} from "../api/types/models.js";
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
    const [assignee, setAssignee] = useState('00000000-0000-0000-0000-000000000000');
    const [status, setStatus] = useState(0);

    const fetchTasks = async () => {
        const response = await getProjectTasks(id);
        const tasksWithExtras = await Promise.all(
            (response.tasks ?? []).map(async (task) => {
                let assigneeEmail = "Unassigned";
                if (task.assignedUser) {
                    try {
                        const userResponse = await getUser(task.assignedUser);
                        assigneeEmail = userResponse?.user.email ?? assigneeEmail;
                    } catch { /* eat exception and keep default*/}
                }

                return {
                    ...task,
                    assigneeEmail,
                };
            })
        );
        return tasksWithExtras;
    }

    useEffect(() => {
        const fetchProjectAndTasks = async () => {
            try {
                const response = await getProjectById(id);
                setSelectedProject(response.project);

                const tasksWithExtras = await fetchTasks();
                setTasks(tasksWithExtras);
            } catch (err) {
                setError("Error fetching project and tasks");
            } finally {
                setLoading(false);
            }
        };
        fetchProjectAndTasks();
    }, [id]);

    const handleCreate = async () => {
        const taskId = crypto.randomUUID();
        await createTask({
            Id: taskId,
            Title: title,
            Description: description,
            AssignedUser: assignee,
            Status: status,
            AssignedProject: id, // TODO: add choice of changing task to different project
        });
        setShowCreateModal(false);
        fetchTasks().then(setTasks);
    }

    const handleUpdate = async () => {
        await updateTask(selectedTask.id, {
            Id: selectedTask.id,
            Title: title,
            Description: description,
            AssignedUser: assignee,
            Status: status,
            AssignedProject: id, // TODO: add choice of changing task to different project
        });
        setShowEditModal(false);
        fetchTasks().then(setTasks);
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
                            assignee={task.assigneeEmail}
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
                    <input 
                        className="modalAssignee"
                        type="text" 
                        placeholder="Enter assignee's email"
                        onChange={(e) => setAssignee(e.target.value)}
                    />
                    <select className="modalStatus" onChange={(e) => setStatus(parseInt(e.target.value))}>
                        {Object.values(TaskStatus).map((v) => (
                            <option key={v} value={v}>
                                {getStatusLabel(v)}
                            </option>
                        ))}
                    </select>
                </Modal>
            )}

            {showEditModal && (
                <Modal title="Edit Task" onClickOutside={() => setShowEditModal(false)} onClickAction={handleUpdate}>
                    <input 
                        className="modalInput"
                        type="text" 
                        placeholder="Enter task name"
                        defaultValue={selectedTask.title}
                        onChange={(e) => setTitle(e.target.value)}
                    />
                    <textarea 
                        className="modalDescription"
                        placeholder="Enter task description (optional)"
                        defaultValue={selectedTask.description}
                        onChange={(e) => setDescription(e.target.value)}
                    />
                    <input 
                        className="modalAssignee"
                        type="text" 
                        placeholder="Enter assignee's email"
                        defaultValue={selectedTask.assigneeEmail == "Unassigned" ? "" : selectedTask.assigneeEmail}
                        onChange={(e) => setAssignee(e.target.value)}
                    />
                    <select className="modalStatus" onChange={(e) => setStatus(parseInt(e.target.value))} defaultValue={selectedTask.status}>
                        {Object.values(TaskStatus).map((v) => (
                            <option key={v} value={v}>
                                {getStatusLabel(v)}
                            </option>
                        ))}
                    </select>
                </Modal>
            )}

        </div>
    );
}