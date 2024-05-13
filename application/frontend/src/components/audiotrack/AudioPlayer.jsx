import AudioPlaybackControls from './AudioPlaybackControls.jsx';
import { useEffect, useState } from 'react';
import './AudioPlayer.css';

const AudioPlayer = ({ audiotrack }) => {

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
    <div style={{ display: 'flex', flexDirection: 'column' }}>
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

export default AudioPlayer;