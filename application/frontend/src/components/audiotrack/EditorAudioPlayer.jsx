import './AudioPlayer.css';
import AudioPlaybackControls from './AudioPlaybackControls.jsx';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import DeletePrompt from '../common/DeletePrompt.jsx';
import { api, apiAuth } from '../../api/mpFetch.js';

const AudiotrackActions = ({ audiotrack, onEditClicked, needUpdate }) => {
	const [showDeletePrompt, setShowDeletePrompt] = useState(false);

	function handleAudiotrackDelete() {
		apiAuth.delete(`audiotracks/${audiotrack.id}`)
			.then(() => { setShowDeletePrompt(false); needUpdate(); })
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
		api.get(`audiotracks/${audiotrack.id}/scores`)
			.then(response => {
				const data = response.data;
				if (response.status === 204 || data.length === 0) {
					setTotalScore(0);
					return;
				}
				let sum = 0;
				data.map(d => sum += d.value);
				setTotalScore(Math.floor(sum / data.length));
			})
			.catch(error => console.log(error))
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