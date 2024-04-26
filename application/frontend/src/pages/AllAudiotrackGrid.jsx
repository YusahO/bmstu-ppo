import './AllAudiotrackGrid.css';
import AudioPlayer from '../components/audiotrack/AudioPlayer.jsx';

const AllAudiotrackGrid = ({ audiotracks }) => {
  return (
    <div className="grid-container">
      {audiotracks.map((audiotrack, index) => (
        <div key={index} className="grid-item">
          <AudioPlayer></AudioPlayer>
        </div>
      ))}
    </div>
  );
};

export default AllAudiotrackGrid;