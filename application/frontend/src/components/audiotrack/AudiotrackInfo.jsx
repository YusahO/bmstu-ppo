// import { useEffect, useRef, useState } from 'react';
import AudioPlayer from './AudioPlayer.jsx';
import './AudiotrackInfo.css';

const AudiotrackInfo = ({ audiotrackParam }) => {
  return (
    <div id='audiotrack-info'>
      <div id='audiotrack-info-content'>
        <h1>
          {audiotrackParam.title}
        </h1>
        <button id='audiotrack-info-close'>
          X
        </button>
      </div>
    </div>
  )
}

export default AudiotrackInfo;