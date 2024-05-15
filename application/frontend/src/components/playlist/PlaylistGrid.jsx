import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import PlaylistElement from "./PlaylistElement";
import PlaylistAddElement from "./PlaylistAddElement";
import Playlist from '../../models/Playlist.js';
import { apiAuth } from "../../api/mpFetch.js";
import { useUserContext } from "../../context/UserContext.js";
import { AlertTypes, useAlertContext } from "../../context/AlertContext.js";

const PlaylistCreateInput = ({ onApply }) => {
	const { user } = useUserContext();
	const { addAlert } = useAlertContext();
	const [title, setTitle] = useState('');
	const navigate = useNavigate();

	function handleTitleSubmit() {
		if (!user) {
			navigate('/auth');
			return;
		}
		apiAuth.post('playlists', { ...Playlist, userId: user.id, title: title })
			.then(() => {
				addAlert(AlertTypes.info, 'Плейлист успешно создан');
				onApply()
			})
			.catch(error => console.error(error));
	}

	return (
		<div style={{
			position: 'fixed',
		}}>
			<input style={{ width: 400 }}
				type="text"
				placeholder="Введите название (Enter для сохранения)"
				value={title}
				onChange={e => setTitle(e.target.value)}
				onKeyDown={e => {
					if (e.key === 'Enter') {
						handleTitleSubmit();
					}
				}}
			/>
		</div>
	);
}

const PlaylistGrid = ({ playlists, onPlaylistsChange }) => {
	const navigate = useNavigate();
	const [showCreateInput, setShowCreateInput] = useState(false);

	useEffect(() => {
		const trackCursor = () => {
			let playlists = document.querySelectorAll('.playlist-element');
			playlists.forEach(p => {
				p.onmousemove = function (e) {
					let x = e.pageX - p.offsetLeft;
					let y = e.pageY - p.offsetTop;

					p.style.setProperty('--x', x + 'px');
					p.style.setProperty('--y', y + 'px');
				}
			});
		}
		trackCursor();
	}, [playlists]);

	function handleDoubleClick(playlist) {
		navigate(`/audiotracks?playlistId=${playlist.id}`);
	}

	return (
		<div style={{
			display: 'flex',
			gap: 40
		}}>
			{playlists.map((playlist, index) => (
				<div key={index}>
					<PlaylistElement
						playlist={playlist}
						onDoubleClick={() => handleDoubleClick(playlist)}
						needUpdate={onPlaylistsChange} />
				</div>
			))}
			<div
				onMouseLeave={() => setShowCreateInput(false)}
			>
				<PlaylistAddElement onClick={() => setShowCreateInput(true)} />
				{showCreateInput && <PlaylistCreateInput onApply={onPlaylistsChange} />}
			</div>
		</div>
	);
};

export default PlaylistGrid;