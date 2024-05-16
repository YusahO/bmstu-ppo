import { apiAuth } from "../api/mpFetch";
import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import AudiotrackGrid from "../components/audiotrack/AudiotrackGrid";
import { AlertTypes, useAlertContext } from "../context/AlertContext";

const PlaylistAudiotracks = () => {
	const { addAlert } = useAlertContext();
	const navigate = useNavigate();
	const location = useLocation();
	const searchParams = new URLSearchParams(location.search);
	const queryPlaylistId = searchParams.get('playlistId');
	const [audiotracks, setAudiotracks] = useState(null);
	const [needUpdate, setNeedUpdate] = useState(false);

	useEffect(() => {
		apiAuth.get(`playlists/${queryPlaylistId}/audiotracks`)
			.then(response => setAudiotracks(response.data))
			.catch(error => {
				if (error.response.status === 403) {
					addAlert(AlertTypes.warn, 'Ресурс недоступен');
					navigate('/');
				}
				console.error(error)
			});
	}, [needUpdate]);

	if (!audiotracks) {
		return <div>Loading...</div>
	}

	return (
		<div style={{ display: 'flex', flexDirection: 'column' }}>
			<h2>Аудиотреки из плейлиста</h2>
			{audiotracks.length > 0 ?
				<AudiotrackGrid
					renderAdd={false}
					audiotracks={audiotracks}
					onAudiotrackUpdate={() => setNeedUpdate(!needUpdate)}
					showActions={false}
				/> :
				<h3>Пусто</h3>
			}
		</div>
	)
}

export default PlaylistAudiotracks;