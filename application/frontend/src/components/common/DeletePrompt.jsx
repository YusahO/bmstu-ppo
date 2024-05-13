const DeletePrompt = ({ onAccept, onClose }) => {
	return (
		<div style={{
			position: 'fixed',
			backgroundColor: 'var(--color-primary)',
			display: 'flex',
			alignItems: 'center',
			justifyContent: 'space-between',
			width: 200,
			padding: 10,
			border: '2px solid var(--accent-color1)'
		}} onMouseLeave={onClose}>
			<label>Вы уверены?</label>
			<div style={{
				display: 'flex',
				gap: 2
			}}>
				<button onClick={onAccept} style={{ padding: 5, width: 'auto' }}>Да</button>
				<button onClick={onClose} style={{ padding: 5, width: 'auto' }}>Нет</button>
			</div>
		</div>
	);
}

export default DeletePrompt;