import { useEffect, useState } from 'react';
import './AllAudiotrackGrid.css';
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

const AllAudiotrackGrid = ({ audiotracks }) => {

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
        {activeAudio !== null && <AudiotrackInfo audiotrackParam={activeAudio} />}
      </div>
      <div className="grid-container">
        {audiotracks.map((audiotrack, index) => (
          <div key={index} className="grid-item" onDoubleClick={() => {
            setActiveAudio(audiotrack);
          }}>
            <AudioPlayer audiotrackParam={audiotrack}></AudioPlayer>
          </div>
        ))}
      </div>
    </>
  );
};

export default AllAudiotrackGrid;