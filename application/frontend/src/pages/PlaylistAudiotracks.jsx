import { apiAuth } from "../api/mpFetch";
import { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import AudiotrackGrid from "../components/audiotrack/AudiotrackGrid";

const PlaylistAudiotracks = () => {

	const location = useLocation();
	const searchParams = new URLSearchParams(location.search);
	const queryPlaylistId = searchParams.get('playlistId');
	const [audiotracks, setAudiotracks] = useState(null);
	const [needUpdate, setNeedUpdate] = useState(false);

	useEffect(() => {
		apiAuth.get(`playlists/${queryPlaylistId}/audiotracks`)
			.then(response => { console.log(response.data); setAudiotracks(response.data) })
			.catch(error => console.error(error));
	}, [needUpdate]);

	if (!audiotracks) {
		return <div>Loading...</div>
	}

	return (
		<div style={{ display: 'flex', flexDirection: 'column', gap: '20px', marginTop: '10px' }}>
			<h2>Аудиотреки из плейлиста</h2>
			{audiotracks.length > 0 ?
				<AudiotrackGrid
					renderAdd={false}
					audiotracks={audiotracks}
					onAudiotrackUpdate={() => setNeedUpdate(!needUpdate)}
				/> :
				<h3>Пусто</h3>
			}
		</div>
	)
}

export default PlaylistAudiotracks;