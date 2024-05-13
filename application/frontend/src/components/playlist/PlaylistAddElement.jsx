import './PlaylistElement.css';

const PlaylistAddElement = ({ onClick }) => {
	return (
		<div onClick={onClick} className='playlist-element'>
			<label className='playlist-add-icon'>+</label>
		</div>
	);
}

export default PlaylistAddElement;