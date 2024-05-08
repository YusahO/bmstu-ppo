import { useEffect, useState } from 'react';
import './AudiotrackGrid.css';
import AudioPlayer from '../components/audiotrack/AudioPlayer.jsx';
import AudiotrackInfo from '../components/audiotrack/AudiotrackInfo.jsx';

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

const AudiotrackGrid = ({ audiotracks }) => {

  const [activeAudio, setActiveAudio] = useState(null);

  useEffect(() => {
    trackCursor();
    const closeBtn = document.getElementById('audiotrack-info-close');
    if (closeBtn) {
      closeBtn.addEventListener('click', () => {
        setActiveAudio(null);
      });
    }
  }, [activeAudio]);

  return (
    <>
      <div>
        {activeAudio !== null && <AudiotrackInfo audiotrackParam={activeAudio} onClose={() => setActiveAudio(null)} />}
      </div>
      <div className="grid-container">
        {audiotracks.map((audiotrack, index) => (
          <div key={index} className="audio-player" onDoubleClick={() => setActiveAudio(audiotrack)}>
            <AudioPlayer audiotrack={audiotrack}></AudioPlayer>
          </div>
        ))}
      </div>
    </>
  );
};

export default AudiotrackGrid;