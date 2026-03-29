import "./css/Modal.css";

export default function Modal({title, children, onClickOutside, onClickDelete, onClickAction}) {

    return(
        <div className="modalContainer" onClick={onClickOutside}>
            <div className="modal" onClick={(e) => e.stopPropagation()}>
                <div className="modalHeader">
                    <h3 className="modalTitle">{title}</h3>
                    <button className="modalClose" onClick={onClickOutside}>
                        ×
                    </button>              
                </div>  
                {children}
                <div className="modalActionRow">
                    {onClickDelete && (
                        <button className="modalDeleteButton" onClick={onClickDelete}>
                            Delete
                        </button>
                    )}
                    <div className="modalActionButton">
                        <button className="modalConfirmButton" onClick={onClickAction}>
                            Confirm
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
}