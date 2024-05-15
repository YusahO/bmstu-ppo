import { useEffect, useState } from "react"
import PlaylistSelector from "./PlaylistSelector";
import { apiAuth } from "../../api/mpFetch";
import { useUserContext } from "../../context/UserContext";
import { useNavigate } from "react-router-dom";

const AudiotrackPlaylistEditor = ({ audiotrackId }) => {
	const { user } = useUserContext();
	const navigate = useNavigate()
	const [allPlaylists, setAllPlaylists] = useState([]);
	const [busyPlaylists, setBusyPlaylists] = useState([]);

	function handleDeleteFromPlaylist(p) {
		apiAuth.delete(`playlists/${p.id}/audiotracks/${audiotrackId}`)
			.catch(error => console.error(error));
	}

	function handleAddToPlaylist(p) {
		apiAuth.post(`playlists/${p.id}/audiotracks/${audiotrackId}`)
			.catch(error => console.error(error));
	}

	useEffect(() => {
		apiAuth.get(`users/${user.id}/playlists`)
			.then(response => setAllPlaylists(response.data))
			.catch(error => console.error(error));

		apiAuth.get(`audiotracks/${audiotrackId}/playlists`, {
			params: { userId: user.id }
		})
			.then(response => setBusyPlaylists(response.data))
			.catch(error => console.error(error));
	}, [user.id, audiotrackId]);

	if (!allPlaylists || !busyPlaylists) {
		return <div>Loading...</div>;
	}

	function handleClickedPlaylist(p) {
		if (busyPlaylists.some(bp => p.id === bp.id)) {
			handleDeleteFromPlaylist(p);
			setBusyPlaylists(prev => prev.filter(bp => bp.id !== p.id));
		} else {
			handleAddToPlaylist(p);
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