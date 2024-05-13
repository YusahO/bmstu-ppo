import './PlaylistElement.css';
import { useNavigate } from "react-router-dom";
import DeletePrompt from "../common/DeletePrompt";
import { useContext, useState } from 'react';
import { UserContext } from '../../App';

const PlaylistEditField = ({ playlist, onClose }) => {
	const [newTitle, setNewTitle] = useState('');
	const navigate = useNavigate();

	function handlePlaylistEdited() {
		fetch(`http://localhost:9898/api/playlists`, {
			mode: 'cors',
			method: 'PUT',
			headers: {
				'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
				'Content-Type': 'application/json; charset=utf-8'
			},
			body: JSON.stringify({ ...playlist, title: newTitle }),
		})
			.then(response => {
				if (response.status === 401) {
					navigate('/auth');
				}
				onClose();
			})
			.catch(error => console.error(error));
	}

	return (
		<div style={{
			position: 'fixed',
			display: 'flex',
			flexDirection: 'column',
			width: 300
		}}>
			<input
				placeholder="Новое название (Enter для сохранения)"
				type="text"
				value={newTitle}
				onChange={e => setNewTitle(e.target.value)}
				onKeyDown={e => {
					if (e.key === 'Enter') {
						handlePlaylistEdited();
					}
				}}
			/>
		</div>
	);
}

const PlaylistActions = ({ playlist, needUpdate }) => {
	const navigate = useNavigate();
	const [showEditField, setShowEditField] = useState(false);
	const [showDeletePrompt, setShowDeletePrompt] = useState(false);

	function handlePlaylistDelete() {
		fetch(`http://localhost:9898/api/playlists`, {
			mode: 'cors',
			method: 'DELETE',
			headers: {
				'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
				'Content-Type': 'application/json; charset=utf-8'
			},
			body: JSON.stringify(playlist),
		})
			.then(response => {
				if (response.status === 401) {
					navigate('/auth');
				}
				setShowDeletePrompt(false); needUpdate();
			})
			.catch(error => console.error(error));
	}

	return (
		<div
			className='playlist-actions'
		>
			<div>
				<button onClick={() => setShowEditField(!showEditField)}>
					&#9998;
				</button>
				{showEditField &&
					<PlaylistEditField playlist={playlist} onClose={() => { setShowEditField(false); needUpdate(); }} />}
			</div>

			<div>
				<button onClick={() => setShowDeletePrompt(true)}>
					&#215;
				</button>
				{showDeletePrompt &&
					<DeletePrompt onAccept={handlePlaylistDelete} onClose={() => { setShowDeletePrompt(false); needUpdate(); }} />}
			</div>
		</div>
	);
}

const PlaylistElement = ({ playlist, onDoubleClick, needUpdate }) => {
	const { user } = useContext(UserContext);

	return (
		<div onDoubleClick={onDoubleClick} className='playlist-element'>
			<div class="playlist-element-icon"></div>
			<label className='playlist-element-label'>
				{playlist.title}
			</label>
			{user && user.favouritesId !== playlist.id &&
				<PlaylistActions playlist={playlist} needUpdate={needUpdate} />}
		</div>
	);
}

export default PlaylistElement;