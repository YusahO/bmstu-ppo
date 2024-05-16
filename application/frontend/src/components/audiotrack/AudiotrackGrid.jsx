import './AudiotrackGrid.css';
import { useEffect, useState } from 'react';
import AudioPlayer from './AudioPlayer.jsx';
import AudiotrackInfo from './AudiotrackInfo.jsx';
import EditorAudioPlayer from './EditorAudioPlayer.jsx';
import AudiotrackEditor from './AudiotrackEditor.jsx';
import BlurComponent from '../common/BlurComponent.jsx';
import CloseButton from '../common/CloseButton.jsx';
import { useUserContext } from '../../context/UserContext.js';

const AudiotrackEdit = ({ audiotrack, onClose }) => {
  return (
    <BlurComponent>
      <div style={{
        borderRadius: '10px',
        backgroundColor: 'var(--color-primary)',
        position: 'fixed',
        top: '50%', left: '50%',
        transform: 'translate(-50%, -50%)',
        padding: '30px'
      }}>
        <CloseButton onClose={onClose} />
        <AudiotrackEditor audiotrack={audiotrack} onDone={onClose} />
      </div>
    </BlurComponent>
  )
}

const AudiotrackAdd = ({ onClick }) => {
  return (
    <div
      onClick={onClick}
      className='audio-player'
    >
      <label style={{
        fontSize: '100px',
        position: 'absolute',
        zIndex: 1,
        top: '50%',
        left: '50%',
        transform: 'translate(-50%, -50%)'
      }}>
        +
      </label>
    </div>
  );
}

const AudiotrackGrid = ({ audiotracks, onAudiotrackUpdate = () => { }, renderAdd, showActions }) => {
  const { user } = useUserContext();
  const [activeAudio, setActiveAudio] = useState(null);
  const [editedAudiotrack, setEditedAudiotrack] = useState(null);
  const [editing, setEditing] = useState(false);

  function handleInfoClose() {
    setActiveAudio(null);
    onAudiotrackUpdate();
  }

  function handleEditClose() {
    setEditedAudiotrack(null);
    setEditing(false);
    onAudiotrackUpdate();
  }

  useEffect(() => {
    const trackCursor = () => {
      let audioPlayers = document.querySelectorAll('.audio-player');
      audioPlayers.forEach(ap => {
        ap.onmousemove = function (e) {
          let x = e.pageX - ap.offsetLeft;
          let y = e.pageY - ap.offsetTop;

          ap.style.setProperty('--x', x + 'px');
          ap.style.setProperty('--y', y + 'px');
        }
      });
    }
    trackCursor();
  }, [audiotracks]);

  function pickAudiotrackPlayer(audiotrack) {
    if (!user || !user.isAdmin) {
      return <AudioPlayer audiotrack={audiotrack} onInfoClicked={() => setActiveAudio(audiotrack)} />
    } else if (user && user.isAdmin) {
      return (
        <EditorAudioPlayer
          audiotrack={audiotrack}
          needUpdate={onAudiotrackUpdate}
          onEditClicked={() => { setEditedAudiotrack(audiotrack); setEditing(true); }}
          showAdminActions={showActions}
          onInfoClicked={() => setActiveAudio(audiotrack)}
        />
      )
    }
  }

  return (
    <div>
      {activeAudio && <AudiotrackInfo audiotrack={activeAudio} onClose={handleInfoClose} />}
      {editing && <AudiotrackEdit audiotrack={editedAudiotrack} onClose={handleEditClose} />}
      <div className="grid-container">
        {user && user.isAdmin && renderAdd &&
          <AudiotrackAdd onClick={() => { setEditedAudiotrack(null); setEditing(true) }} />}
        {audiotracks.map((audiotrack, index) => (
          <div
            key={index} className="audio-player"
            onDoubleClick={() => setActiveAudio(audiotrack)}
          >
            {pickAudiotrackPlayer(audiotrack)}
          </div>
        ))}
      </div>
    </div>
  );
};

export default AudiotrackGrid;