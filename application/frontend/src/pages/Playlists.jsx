import { useEffect, useState } from 'react';
import PlaylistGrid from '../components/playlist/PlaylistGrid.jsx';
import { apiAuth } from '../api/mpFetch.js';
import { useUserContext } from '../context/UserContext.js';

const Playlists = () => {
	const { user } = useUserContext();
	const [needUpdate, setNeedUpdate] = useState(false);
	const [playlists, setPlaylists] = useState(null);

	function handlePlaylistsChange() {
		setNeedUpdate(!needUpdate);
	}

	useEffect(() => {
		apiAuth.get(`users/${user.id}/playlists`)
			.then(response => setPlaylists(response.data))
			.catch(error => console.error(error));
	}, [user.id, needUpdate]);

	if (!playlists) {
		return <div>Loading...</div>
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