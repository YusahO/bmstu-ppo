import { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import AudiotrackGrid from "../components/audiotrack/AudiotrackGrid";

const PlaylistAudiotracks = () => {

	const location = useLocation();
	const searchParams = new URLSearchParams(location.search);
	const queryPlaylist = searchParams.get('playlistId');
	const [audiotracks, setAudiotracks] = useState([]);

	function fetchPlaylistAudiotracks() {
		fetch(`http://localhost:9898/api/playlistsAudiotracks/${queryPlaylist}`, {
			mode: 'cors',
			method: 'GET',
			headers: {
				'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
			},
		})
			.then(response => response.json())
			.then(data => setAudiotracks(data))
			.catch(error => console.error(error))
	}

	useEffect(() => {
		fetchPlaylistAudiotracks();
	}, []);

	if (!audiotracks) {
		return <div>Loading...</div>
	}

	return (
		<div style={{ display: 'flex', flexDirection: 'column', gap: '20px', marginTop: '10px' }}>
			<h2>Аудиотреки из плейлиста</h2>
			<AudiotrackGrid renderAdd={false} audiotracks={audiotracks} onAudiotrackUpdate={fetchPlaylistAudiotracks} />
		</div>
	)
}

export default PlaylistAudiotracks;