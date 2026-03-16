import "./css/TaskRow.css";

export default function TaskRow({name, description, assignee, status, onClick, onEdit}) {

    return(
        <div className="rowContainer" onClick={onClick}>
            <div className="rowContainerTask">
                <p className="rowContainerName">{name}</p>
                <p className="rowContainerDescription">{description}</p>
            </div>
            <p className="rowContainerAssignee">{assignee}</p>
            <p className="rowContainerStatus">{status}</p>
            <button
                type="button"
                className="rowEditButton"
                onClick={(e) => { e.stopPropagation(); onEdit(); }}
                title="Edit Task"
            />
        </div>
    );
}