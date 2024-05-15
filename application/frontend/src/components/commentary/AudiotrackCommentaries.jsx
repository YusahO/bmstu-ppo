import './AudiotrackCommentaries.css'
import { api, apiAuth } from '../../api/mpFetch.js';
import { useEffect, useState } from "react";
import Commentary from '../../models/Commentary.js';
import CommentaryElement from "./CommentaryElement.jsx";
import { useUserContext } from '../../context/UserContext.js';
import { AlertTypes, useAlertContext } from '../../context/AlertContext.js';

const AudiotrackCommentaries = ({ audiotrackId }) => {
	const { user } = useUserContext();
	const { addAlert } = useAlertContext();

	const [comms, setComms] = useState([]);
	const [needUpdate, setNeedUpdate] = useState(false);
	const [commText, setCommText] = useState('');

	function handleCommSubmit() {
		if (commText === '') {
			addAlert(AlertTypes.warn, 'Введите текст комментария');
			return;
		}

		apiAuth.post('commentaries', {
			...Commentary,
			authorId: user.id,
			audiotrackId: audiotrackId,
			text: commText
		})
			.then(() => {
				setNeedUpdate(!needUpdate);
				setCommText('');
			})
			.catch(error => console.error(error));
	}

	function handleCommUpdate() {
		setNeedUpdate(!needUpdate);
	}

	useEffect(() => {
		setComms([]);
		api.get(`audiotracks/${audiotrackId}/commentaries`)
			.then(response => setComms(response.data))
			.catch(error => console.error(error));
	}, [needUpdate, audiotrackId]);

	useEffect(() => { console.log(comms) }, [comms]);

	return (
		<div style={{
			display: 'flex',
			flexDirection: 'column',
			alignItems: 'space-between',
			height: '100%'
		}}>
			<div className='commentary-container'>
				{comms.map((c) =>
					<CommentaryElement commentary={c} needUpdate={handleCommUpdate} />)
				}
			</div>
			<div style={{ flexShrink: '0', display: 'flex' }}>
				<input
					id='ac-comm-inp'
					style={{ flex: '0 0 90%' }}
					type="text"
					placeholder="Введите комментарий"
					value={commText}
					required={true}
					onChange={(e) => setCommText(e.target.value)}
				/>
				<button
					style={{ flex: '1 1 auto', fontSize: "25px" }}
					onClick={handleCommSubmit}
				>
					&#10002;
				</button>
			</div>
		</div>
	);
}

export default AudiotrackCommentaries;