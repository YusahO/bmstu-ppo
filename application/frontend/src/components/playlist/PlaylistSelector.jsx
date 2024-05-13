import './PlaylistSelector.css';

const PlaylistSelector = ({ playlist, isSelected, onClick }) => {
	return (
		<div
			className='playlist-selector'
		>
			<label>
				{playlist.title}
			</label>
			<label
				className='playlist-select-button'
				onClick={onClick}
			>
				{isSelected ? '-' : '+'}
			</label>
		</div>
	);
}

export default PlaylistSelector;