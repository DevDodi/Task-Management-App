import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { getProjectById } from "../api/projects.api.js";
import { createTask, getProjectTasks, updateTask, deleteTask } from "../api/tasks.api.js";
import { getTaskLogs } from "../api/tasklogs.api.js";
import { getUser, getUsers } from "../api/users.api.js";
import Modal from "../components/Modal";
import TaskRow from "../components/TaskRow";
import {getStatusLabel} from "../components/TaskRow";
import {TaskStatus} from "../api/types/models.js";
import { formatDateTime } from "../utils/dateTimeUtil.js";
import { useNavigate } from "react-router-dom";
import toast from "react-hot-toast";
import "./css/ProjectDetails.css";


export default function ProjectDetails() {

    const {id} = useParams();
    const navigate = useNavigate();
    const [users, setUsers] = useState([]);
    const [selectedProject, setSelectedProject] = useState(null);
    const [tasks, setTasks] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");    
    const [auditLogs, setAuditLogs] = useState([]);
    const [totalLogCount, setTotalLogCount] = useState(0);
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
                        const userResponse = task.assignedUser === '00000000-0000-0000-0000-000000000000' ? null : await getUser(task.assignedUser);
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

                const userResponses = await getUsers();
                setUsers(userResponses.users);
            } catch (err) {
                setError("Error fetching project and tasks");
            } finally {
                setLoading(false);
            }
        };
        fetchProjectAndTasks();
    }, [id]);


    const handleSelectTask = async (task) => {
        try {
            const response = await getTaskLogs(task.id);
            const logs = response.taskLogs ?? [];
            
            // Store total count of task logs and limit to first 10
            setTotalLogCount(logs.length);
            const limitedLogs = logs.slice(0, 10);
            setAuditLogs(limitedLogs);

        } catch (err) {
            setSelectedTask(null);
        }
        setSelectedTask(task);
    }

    const handleCreate = async () => {
        const taskId = crypto.randomUUID();
        await createTask({
            Id: taskId,
            Title: title,
            Description: description,
            AssignedUser: assignee === "" ? "00000000-0000-0000-0000-000000000000" : assignee,
            Status: status,
            AssignedProject: id, // TODO: add choice of changing task to different project
        });
        setShowCreateModal(false);
        refreshPage();
    }

    const handleUpdate = async () => {
        await updateTask(selectedTask.id, {
            Id: selectedTask.id,
            Title: title,
            Description: description,
            AssignedUser: assignee === "" ? "00000000-0000-0000-0000-000000000000" : assignee,
            Status: status,
            AssignedProject: id, // TODO: add choice of changing task to different project
        });
        setShowEditModal(false);
        refreshPage();
    }

    const handleDelete = async () => {
        await deleteTask(selectedTask.id);
        setShowEditModal(false);
        setSelectedTask(null);    
        refreshPage();
    }

    const refreshPage = () => {
        setTitle("");
        setDescription("");
        setAssignee('00000000-0000-0000-0000-000000000000');
        setStatus(0);
        fetchTasks().then(setTasks);
    }

    if (loading) return <div className="detailsContainer">Loading...</div>;
    if (error) return <div className="detailsContainer">{error}</div>;

    return (
        <div className="detailsContainer">
            <div className="detailsHeader">
                <div className="detailsTitle">
                    <h1 className="projectsBreadcrumb" onClick={() => navigate("/projects")} style={{fontSize: "24px"}}>
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
                            onClick={() => handleSelectTask(task)}
                            onEdit={() => {
                                handleSelectTask(task);
                                setTitle(task.title);
                                setDescription(task.description || "");
                                setAssignee(task.assigneeEmail === "Unassigned" ? "" : task.assigneeEmail);
                                setStatus(task.status);
                                setShowEditModal(true);
                            }}
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
                            <div style={{display: "flex", justifyContent: "space-between", alignItems: "center"}}>
                                <p className="auditLogs-label">Audit Log</p>
                                {totalLogCount > 10 && <span style={{fontSize: "12px", color: "#666"}}>Showing {auditLogs.length} of {totalLogCount}</span>}
                            </div>
                            <div className="auditLogs-items">
                                {auditLogs.map((log, index) => (
                                    <div key={index} className="auditLogs-item">
                                        <span className="auditLogs-item-time">{formatDateTime(log.lastUpdatedUtc)}</span>
                                        <p className="auditLogs-item-text">{log.message}</p>
                                    </div>
                                ))}
                            </div>
                        </div>
                    </div>
                )}
            </div>
            {showCreateModal && (
                <Modal title="New Task" onClickOutside={() => { setShowCreateModal(false); setTitle(""); setDescription(""); setAssignee('00000000-0000-0000-0000-000000000000'); setStatus(0); }} onClickAction={handleCreate}>
                    <label className="modalLabel">Task Name</label>
                    <input 
                        className="modalInput"
                        type="text" 
                        placeholder="Enter task name"
                        onChange={(e) => setTitle(e.target.value)}
                    />
                    <label className="modalLabel">Task Description</label>
                    <textarea 
                        className="modalDescription"
                        placeholder="Enter task description (optional)"
                        onChange={(e) => setDescription(e.target.value)}
                    />
                    <label className="modalLabel">Assignee's Email</label>
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
                    <label className="modalLabel">Status</label>
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
                <Modal title="Edit Task" onClickOutside={() => { setShowEditModal(false); setTitle(""); setDescription(""); setAssignee('00000000-0000-0000-0000-000000000000'); setStatus(0); }} onClickDelete = {handleDelete} onClickAction={handleUpdate}>
                    <label className="modalLabel">Task Name</label>
                    <input 
                        className="modalInput"
                        type="text" 
                        placeholder="Enter task name"
                        value={title}
                        onChange={(e) => setTitle(e.target.value)}
                    />
                    <label className="modalLabel">Task Description</label>
                    <textarea 
                        className="modalDescription"
                        placeholder="Enter task description (optional)"
                        value={description}
                        onChange={(e) => setDescription(e.target.value)}
                    />
                    <label className="modalLabel">Assignee's Email</label>
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
                    <label className="modalLabel">Status</label>
                    <select className="modalStatus" value={status} onChange={(e) => setStatus(parseInt(e.target.value))}>
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