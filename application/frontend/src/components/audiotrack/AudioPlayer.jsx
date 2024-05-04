import AudioPlaybackControls from './AudioPlaybackControls.jsx';
import './AudioPlayer.css';

const AudioPlayer = ({ audiotrack }) => {
  return (
    <div style={{ display: 'flex', flexDirection: 'column' }}>
      <AudioPlaybackControls audiotrackParam={audiotrack} />
      <label id='audio-title-label'>{audiotrack.title}</label>
    </div>
  )
}

export default AudioPlayer;