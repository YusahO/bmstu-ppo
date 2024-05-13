import './AudiotrackCommentaries.css'

import { useContext, useEffect, useState } from "react";
import { UserContext } from "../../App.js";
import Commentary from '../../models/Commentary.js';
import CommentaryElement from "./CommentaryElement.jsx";

const AudiotrackCommentaries = ({ audiotrackId }) => {
	const { user } = useContext(UserContext);

	const [comms, setComms] = useState([]);
	const [needUpdate, setNeedUpdate] = useState(false);
	const [commText, setCommText] = useState('');

	function handleCommSubmit() {
		if (commText === '') {
		// 	document.getElementById('ac-comm-inp').style.borderColor = 'red';
			return;
		}

		fetch('http://localhost:9898/api/commentaries/', {
			mode: 'cors',
			method: 'POST',
			headers: {
				'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
				"Content-Type": "application/json; charset=utf-8",
			},
			body: JSON.stringify({
				...Commentary,
				authorId: user.id,
				audiotrackId: audiotrackId,
				text: commText
			}),
		})
			.then(() => {
				setNeedUpdate(!needUpdate);
				setCommText('');
			});
	}

	function handleCommUpdate() {
		setNeedUpdate(!needUpdate);
	}

	useEffect(() => {
		setComms([]);
		fetch(`http://localhost:9898/api/commentaries/${audiotrackId}`, {
			mode: 'cors',
			method: 'GET'
		})
			.then((response) => response.json())
			.then((data) => { setComms(data); console.log('refetch!') });
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