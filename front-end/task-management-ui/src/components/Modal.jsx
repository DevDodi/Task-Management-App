import "./css/Modal.css";

export default function Modal({title, children, onClickOutside, OnClickAction}) {

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
                <div className="modalActionButton">
                    <button className="modalConfirmButton" onClick={OnClickAction}>
                        Confirm
                    </button>
                </div>
            </div>
        </div>
    );
}