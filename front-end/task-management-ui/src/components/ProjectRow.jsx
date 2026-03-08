import "./css/ProjectRow.css";

export default function ProjectRow({name, description, createdAt, onClick}) {

    return(
        <div className="rowContainer" onClick={onClick}>
            <p className="rowContainerName">{name}</p>
            <p className="rowContainerDescription">{description}</p>
            <p className="rowContainerDate">{createdAt}</p>
        </div>
    );
}