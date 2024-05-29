import './AudioPlayer.css';
import { useState } from 'react';
import DeletePrompt from '../common/DeletePrompt.jsx';
import { apiAuth } from '../../api/mpFetch.js';
import { AlertTypes, useAlertContext } from '../../context/AlertContext.js';

const AudiotrackActions = ({ audiotrack, onInfoClicked = null, onEditClicked, needUpdate, showAdminActions }) => {
  const [showDeletePrompt, setShowDeletePrompt] = useState(false);
  const { addAlert } = useAlertContext();

  function handleAudiotrackDelete() {
    apiAuth.delete(`audiotracks/${audiotrack.id}`)
      .then(() => {
        addAlert(AlertTypes.info, 'Аудиотрек удален');
        setShowDeletePrompt(false); needUpdate();
      })
      .catch(error => {
        addAlert(AlertTypes.error, 'Ошибки при удалении аудиотрека');
        console.error(error)
      });
  }

  return (
    <div className='audio-actions'>
      {onInfoClicked !== null &&
        <button
          className='audio-actions-button'
          onClick={onInfoClicked}
        >
          i
        </button>
      }
      {showAdminActions &&
        <div style={{ display: 'flex', gap: '5px', flexDirection: 'row-reverse' }}>
          <button className='audio-actions-button' onClick={onEditClicked}>
            &#9998;
          </button>
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
      }
    </div>
  );
}

export default AudiotrackActions;