import './AudioPlayer.css';
import AudioPlaybackControls from './AudioPlaybackControls.jsx';
import { useEffect, useState } from 'react';
import { api, apiAuth } from '../../api/mpFetch.js';
import AudiotrackActions from './AudiotrackActions.jsx';

// const AudiotrackActions = ({ audiotrack, onEditClicked, needUpdate, showAdminActions }) => {
// 	const [showDeletePrompt, setShowDeletePrompt] = useState(false);
// 	const { addAlert } = useAlertContext();

// 	function handleAudiotrackDelete() {
// 		apiAuth.delete(`audiotracks/${audiotrack.id}`)
// 			.then(() => {
// 				addAlert(AlertTypes.info, 'Аудиотрек удален');
// 				setShowDeletePrompt(false); needUpdate();
// 			})
// 			.catch(error => {
// 				addAlert(AlertTypes.error, 'Ошибки при удалении аудиотрека');
// 				console.error(error)
// 			});
// 	}

// 	return (
// 		<div className='audio-actions'>
// 			<button className='audio-actions-button'>i</button>
// 			{showAdminActions &&
// 				<div>
// 					<button className='audio-actions-button' onClick={onEditClicked}>
// 						&#9998;
// 					</button>
// 					<button className='audio-actions-button' onClick={() => setShowDeletePrompt(true)}>
// 						&#215;
// 					</button>
// 					{showDeletePrompt &&
// 						<div style={{ position: 'relative', top: '-50px', left: '-190px' }}>
// 							<DeletePrompt
// 								onAccept={handleAudiotrackDelete}
// 								onClose={() => { setShowDeletePrompt(false); needUpdate(); }}
// 							/>
// 						</div>
// 					}
// 				</div>
// 			}
// 		</div>
// 	);
// }

const EditorAudioPlayer = ({ audiotrack, onInfoClicked, onEditClicked, needUpdate, showAdminActions = false }) => {
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
			<AudiotrackActions
				audiotrack={audiotrack}
				needUpdate={needUpdate}
				onEditClicked={onEditClicked}
				showAdminActions={showAdminActions}
				onInfoClicked={onInfoClicked}
			/>
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