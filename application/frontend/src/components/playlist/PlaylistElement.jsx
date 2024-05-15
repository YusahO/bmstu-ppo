import './PlaylistElement.css';
import DeletePrompt from "../common/DeletePrompt";
import { useState } from 'react';
import { apiAuth } from '../../api/mpFetch';
import { useUserContext } from '../../context/UserContext';
import { AlertTypes, useAlertContext } from '../../context/AlertContext';

const PlaylistEditField = ({ playlist, onClose }) => {
	const [newTitle, setNewTitle] = useState('');
	const { addAlert } = useAlertContext();

	function handlePlaylistEdited() {
		apiAuth.put('playlists', { ...playlist, title: newTitle })
			.then(() => {
				addAlert(AlertTypes.info, 'Название плейлиста обновлено');
				onClose()
			})
			.catch(error => console.error(error));
	}

	return (
		<div style={{
			position: 'fixed',
			display: 'flex',
			flexDirection: 'column',
			width: 300
		}} onMouseLeave={onClose}>
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
	const [showEditField, setShowEditField] = useState(false);
	const [showDeletePrompt, setShowDeletePrompt] = useState(false);
	const { addAlert } = useAlertContext();

	function handlePlaylistDelete() {
		apiAuth.delete(`playlists/${playlist.id}`,)
			.then(() => {
				setShowDeletePrompt(false); needUpdate();
				addAlert(AlertTypes.info, 'Плейлист успешно удален');
			})
			.catch(error => console.error(error));
	}

	return (
		<div className='playlist-actions' >
			<div>
				<button
					className='playlist-actions-button'
					onClick={() => setShowEditField(!showEditField)}
				>
					&#9998;
				</button>
				{showEditField &&
					<PlaylistEditField playlist={playlist} onClose={() => { setShowEditField(false); needUpdate(); }} />}
			</div>

			<div>
				<button
					className='playlist-actions-button'
					onClick={() => setShowDeletePrompt(true)}
				>
					&#215;
				</button>
				{showDeletePrompt &&
					<DeletePrompt onAccept={handlePlaylistDelete} onClose={() => { setShowDeletePrompt(false); needUpdate(); }} />}
			</div>
		</div>
	);
}

const PlaylistElement = ({ playlist, onDoubleClick, needUpdate }) => {
	const { user } = useUserContext();

	return (
		<div onDoubleClick={onDoubleClick} className='playlist-element'>
			<div className="playlist-element-icon"></div>
			<label className='playlist-element-label'>
				{playlist.title}
			</label>
			{user && user.favouritesId !== playlist.id &&
				<PlaylistActions playlist={playlist} needUpdate={needUpdate} />}
		</div>
	);
}

export default PlaylistElement;