import "./css/ProjectRow.css";
import editIcon from "../assets/edit.png";

export default function ProjectRow({name, description, owner, onClick, onEdit}) {

    return(
        <div className="rowContainer" onClick={onClick}>
            <p className="rowContainerName">{name}</p>
            <p className="rowContainerDescription">{description}</p>
            <p className="rowContainerOwner">{owner}</p>
            <button type="button" className="rowEditButton" onClick={(e) => { e.stopPropagation(); onEdit(); }} title="Edit Task">
                <img src={editIcon} alt="Edit" />
            </button>
        </div>
    );
}