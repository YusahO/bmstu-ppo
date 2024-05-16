import './PlaylistSelector.css';

const PlaylistSelector = ({ playlist, isSelected, onClick }) => {
	return (
		<div className='playlist-selector'>
			<label>
				{playlist.title}
			</label>
			<input type="checkbox" checked={isSelected} onClick={onClick} />
		</div>
	);
}

export default PlaylistSelector;