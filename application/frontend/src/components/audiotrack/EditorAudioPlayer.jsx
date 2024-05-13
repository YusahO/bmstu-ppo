import './AudioPlayer.css';
import AudioPlaybackControls from './AudioPlaybackControls.jsx';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import DeletePrompt from '../common/DeletePrompt.jsx';

const AudiotrackActions = ({ audiotrack, onEditClicked, needUpdate }) => {
	const navigate = useNavigate();

	const [showDeletePrompt, setShowDeletePrompt] = useState(false);

	function handleAudiotrackDelete() {
		fetch(`http://localhost:9898/api/audiotracks`, {
			mode: 'cors',
			method: 'DELETE',
			headers: {
				'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
				'Content-Type': 'application/json; charset=utf-8'
			},
			body: JSON.stringify({ ...audiotrack, file: 'string' }),
		})
			.then(response => {
				if (response.status === 401) {
					navigate('/auth');
				}
				setShowDeletePrompt(false); needUpdate();
			})
			.catch(error => console.error(error));
	}

	return (
		<div className='audio-actions'>
			<div>
				<button className='audio-actions-button' onClick={onEditClicked}>
					&#9998;
				</button>
			</div>

			<div>
				<button className='audio-actions-button' onClick={() => setShowDeletePrompt(true)}>
					&#215;
				</button>
				{showDeletePrompt &&
					<div style={{ position: 'relative', top: '-50px', left: '-190px' }}>
						<DeletePrompt
							onAccept={handleAudiotrackDelete}
							onClose={() => { setShowDeletePrompt(false); needUpdate(); }}
						/>
					</div>
				}
			</div>
		</div>
	);
}

const EditorAudioPlayer = ({ audiotrack, onEditClicked, needUpdate }) => {
	const [totalScore, setTotalScore] = useState(0);

	useEffect(() => {
		fetch(`http://localhost:9898/api/scores/${audiotrack.id}`, {
			mode: 'cors',
			method: 'GET'
		})
			.then((response) => {
				if (response.status === 204) {
					return JSON.stringify({ value: 0 });
				}
				return response.json();
			})
			.then((data) => {
				if (data.length === 0) {
					setTotalScore(0);
					return;
				}

				let sum = 0;
				data.map(d => sum += d.value);
				setTotalScore(Math.floor(sum / data.length));
			})
	}, [audiotrack]);

	return (
		<div style={{ zIndex: 0, display: 'flex', flexDirection: 'column' }}>
			<AudiotrackActions audiotrack={audiotrack} needUpdate={needUpdate} onEditClicked={onEditClicked} />
			<AudioPlaybackControls audiotrackParam={audiotrack} />
			<div style={{
				display: 'flex',
				justifyContent: 'space-between',
				alignItems: 'center',
				gap: '30px'
			}}>
				<label id='audio-title-label'>{audiotrack.title}</label>
				<label style={{ zIndex: '1' }}>{totalScore}&nbsp;★</label>
			</div>
		</div>
	)
}

export default EditorAudioPlayer;