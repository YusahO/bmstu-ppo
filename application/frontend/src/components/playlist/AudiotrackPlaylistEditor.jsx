import { useContext, useEffect, useState } from "react"
import { UserContext } from "../../App";
import PlaylistSelector from "./PlaylistSelector";
import { useNavigate } from "react-router-dom";

const AudiotrackPlaylistEditor = ({ audiotrackId }) => {

	const navigate = useNavigate();
	const { user } = useContext(UserContext);
	const [allPlaylists, setAllPlaylists] = useState([]);
	const [busyPlaylists, setBusyPlaylists] = useState([]);

	useEffect(() => {
		fetch(`http://localhost:9898/api/playlists/users/${user.id}`, {
			mode: 'cors',
			method: 'GET',
			headers: {
				'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
			},
		})
			.then(response => response.json())
			.then(data => setAllPlaylists(data))
			.catch(error => console.error(error));
	}, [user.id, audiotrackId]);

	useEffect(() => {
		fetch(`http://localhost:9898/api/playlistsAudiotracks?userId=${user.id}&audiotrackId=${audiotrackId}`, {
			mode: 'cors',
			method: 'GET',
			headers: {
				'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
			},
		})
			.then(response => response.json())
			.then(data => setBusyPlaylists(data))
			.catch(error => console.error(error));
	}, [user.id, audiotrackId]);

	useEffect(() => {
	}, [allPlaylists, busyPlaylists]);

	function deleteFromPlaylist(p) {
		fetch(`http://localhost:9898/api/playlistsAudiotracks`, {
			mode: 'cors',
			method: 'DELETE',
			headers: {
				'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
				'Content-Type': 'application/json; charset=utf-8'
			},
			body: JSON.stringify({ audiotrackId: audiotrackId, playlistId: p.id })
		})
			.then(response => {
				if (response.status === 401) {
					navigate('/auth');
				}
			})
			.catch(error => console.error(error));
	}

	function addToPlaylist(p) {
		fetch(`http://localhost:9898/api/playlistsAudiotracks`, {
			mode: 'cors',
			method: 'POST',
			headers: {
				'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
				'Content-Type': 'application/json; charset=utf-8'
			},
			body: JSON.stringify({ audiotrackId: audiotrackId, playlistId: p.id })
		})
			.then(response => {
				if (response.status === 401) {
					navigate('/auth');
				}
			})
			.catch(error => console.error(error));
	}

	function handleClickedPlaylist(p) {
		if (busyPlaylists.some(bp => p.id === bp.id)) {
			console.log('del');
			deleteFromPlaylist(p);
			setBusyPlaylists(prev => prev.filter(bp => bp.id !== p.id));
		} else {
			console.log('add');
			addToPlaylist(p);
			setBusyPlaylists(prev => [...prev, p]);
		}
	}

	return (
		<div style={{
			display: 'flex',
			flexDirection: 'column',
			position: 'fixed',
		}}>
			{allPlaylists.map(p =>
				<PlaylistSelector
					playlist={p}
					isSelected={busyPlaylists.some(bp => p.id === bp.id)}
					onClick={() => handleClickedPlaylist(p)}
				/>
			)}
		</div>
	)
}

export default AudiotrackPlaylistEditor;