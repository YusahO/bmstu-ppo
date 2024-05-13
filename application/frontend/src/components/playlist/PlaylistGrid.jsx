import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { UserContext } from '../../App';
import PlaylistElement from "./PlaylistElement";
import PlaylistAddElement from "./PlaylistAddElement";
import Playlist from '../../models/Playlist.js';
import DeletePrompt from "../common/DeletePrompt.jsx";

const PlaylistCreateInput = ({ onApply }) => {
	const [title, setTitle] = useState('');
	const { user } = useContext(UserContext);
	const navigate = useNavigate();

	function handleTitleSubmit() {
		if (!user) {
			navigate('/auth');
			return;
		}

		fetch(`http://localhost:9898/api/playlists`, {
			mode: 'cors',
			method: 'POST',
			headers: {
				'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
				'Content-Type': 'application/json; charset=utf-8'
			},
			body: JSON.stringify({ ...Playlist, userId: user.id, title: title })
		})
			.then(response => {
				if (response.status === 401) {
					navigate('/auth');
				}
				onApply();
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

const PlaylistUpdateInput = ({ playlist, onApply }) => {
	const [title, setTitle] = useState('');
	const { user } = useContext(UserContext);
	const navigate = useNavigate();

	function handleTitleSubmit() {
		if (!user) {
			navigate('/auth');
			return;
		}

		fetch(`http://localhost:9898/api/playlists`, {
			mode: 'cors',
			method: 'PUT',
			headers: {
				'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
				'Content-Type': 'application/json; charset=utf-8'
			},
			body: JSON.stringify({ ...playlist, title: title })
		})
			.then(response => {
				if (response.status === 401) {
					navigate('/auth');
				}
				onApply();
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

	const { user } = useContext(UserContext);
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

	function handlePlaylistDelete(playlist) {
		if (!user) {
			navigate('/auth');
			return;
		}

		fetch(`http://localhost:9898/api/playlists`, {
			mode: 'cors',
			method: 'DELETE',
			headers: {
				'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
				'Content-Type': 'application/json; charset=utf-8'
			},
			body: JSON.stringify(playlist)
		})
			.then(response => {
				if (response.status === 401) {
					navigate('/auth');
				}
				onPlaylistsChange();
			})
			.catch(error => console.error(error));
	}

	function handleDoubleClick(playlist) {
		navigate(`/audiotracks?playlistId=${playlist.id}`);
	}

	return (
		<>
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
		</>
	);
};

export default PlaylistGrid;