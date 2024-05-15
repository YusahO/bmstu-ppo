import './AudioPlayer.css';
import AudioPlaybackControls from './AudioPlaybackControls.jsx';
import { useEffect, useState } from 'react';
import { api } from '../../api/mpFetch.js';

const AudioPlayer = ({ audiotrack }) => {

  const [totalScore, setTotalScore] = useState(0);

  useEffect(() => {
    api.get(`audiotracks/${audiotrack.id}/scores`)
      .then(response => {
        const data = response.data;
        console.log(data);
        if (response.status === 204 || data.length === 0) {
          setTotalScore(0);
          return;
        }
        let sum = 0;
        data.map(d => sum += d.value);
        setTotalScore(Math.floor(sum / data.length));
      })
      .catch(error => console.error(error));
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