import { useContext, useEffect, useState } from 'react';
import { UserContext } from '../App.js';
import PlaylistGrid from '../components/playlist/PlaylistGrid.jsx';

const Playlists = () => {
	const { user } = useContext(UserContext);
	const [needUpdate, setNeedUpdate] = useState(false);
	const [playlists, setPlaylists] = useState([]);

	useEffect(() => {
		fetch(`http://localhost:9898/api/playlists/users/${user.id}`, {
			mode: 'cors',
			method: 'GET',
			headers: {
				'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
			},
		})
			.then(response => response.json())
			.then(data => {
				setPlaylists(data);
			})
			.catch(error => console.error(error));
	}, [user.id, needUpdate]);

	function handlePlaylistsChange() {
		setNeedUpdate(!needUpdate);
	}

	return (
		<div style={{ display: 'flex', flexDirection: 'column' }}>
			<h2>Мои плейлисты</h2>
			<div style={{ padding: '20px' }}>
				<PlaylistGrid playlists={playlists} onPlaylistsChange={handlePlaylistsChange} />
			</div>
		</div>
	);
}

export default Playlists;