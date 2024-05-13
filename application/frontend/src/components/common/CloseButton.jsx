import './CloseButton.css';

const CloseButton = ({ onClose }) => {
	return (
		<button id='close-button' onClick={onClose} style={{ fontSize: "25px" }}>
			&#10005;
		</button>
	);
}

export default CloseButton;