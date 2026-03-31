import "./css/TaskRow.css";
import { TaskStatus } from "../api/types/models.js";
import editIcon from "../assets/edit.png";
import { desc } from "framer-motion/client";

function statusClass(status) {
  switch (status) {
    case 0: return "badge badge--warning";
    case 1: return "badge badge--danger";
    case 2: return "badge badge--success";
    default: return "badge badge--neutral";
  }
}

export function getStatusLabel(status) {
    const key = Object.keys(TaskStatus).find(
        (k) => TaskStatus[k] === status);

    // Convert `NotCompleted` → `Not Completed` (insert spaces between camel/Pascal case words)
    const formatKey = (k) => k.replace(/([a-z])([A-Z])/g, "$1 $2");

    return key ? formatKey(key) : status;
};

export default function TaskRow({name, description, assignee, status, onClick, onEdit}) {
    return(
        <div className="rowContainer" onClick={onClick}>
            <div className="rowContainerTask">
                <p className="rowContainerName">{name}</p>
                <p className="rowContainerDescription">{description}</p>
            </div>
            <p className="rowContainerAssignee">{assignee}</p>
            <p className={statusClass(status)}>{getStatusLabel(status)}</p>
            <button type="button" className="rowEditButton" onClick={(e) => { e.stopPropagation(); onEdit(); }} title="Edit Task">
                <img src={editIcon} alt="Edit" />
            </button>
        </div>
    );
}