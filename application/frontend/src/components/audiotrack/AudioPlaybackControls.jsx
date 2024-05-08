import { useEffect, useRef, useState } from 'react';
import { useWavesurfer } from '@wavesurfer/react'
import './AudioPlaybackControls.css';


const Pause = ({ onPlayPause }) => {
  return (
    <button onClick={onPlayPause}>
      <svg viewBox="0 0 60 60">
        <polygon points="0,0 20,0 20,60 0,60" />
        <polygon points="40,0 60,0 60,60, 40,60" />
      </svg>
    </button>
  )
}

const Play = ({ onPlayPause }) => {
  return (
    <button onClick={onPlayPause}>
      <svg viewBox="0 0 60 60">
        <polygon points="0,0 60,30 0,60" />
      </svg>
    </button>
  )
}

const formatTime = (seconds) => {
  const minutes = Math.floor(seconds / 60)
  const secondsRemainder = Math.round(seconds) % 60
  const paddedSeconds = `0${secondsRemainder}`.slice(-2)
  return `${minutes}:${paddedSeconds}`
}

export default function AudioPlaybackControls({ audiotrackParam }) {

  const containerRef = useRef();
  const [duration, setDuration] = useState(0);

  const { wavesurfer, isPlaying } = useWavesurfer({
    container: containerRef,
    url: `http://localhost:9898/api/audiotracks/${audiotrackParam.filepath}`,
    waveColor: '#ccc',
    progressColor: '#0178ff',
    cursorColor: 'transparent',
    responsive: true,
    height: 80,
    normalize: true,
    barWidth: 2,
    barGap: 3,
    fillParent: true,
  })

  const onPlayPause = () => {
    wavesurfer && wavesurfer.playPause();
  }

  useEffect(() => {
    if (wavesurfer) {
      wavesurfer.on('decode', (duration) => setDuration(duration))
      wavesurfer.on('finish', () => wavesurfer.seekTo(0));
    }
  }, [wavesurfer]);

  return (
    <div className='audio-playback-controls'>
      <div className='controls'>
        {isPlaying ? <Pause onPlayPause={onPlayPause} /> : <Play onPlayPause={onPlayPause} />}
      </div>
      <div id='waveform' ref={containerRef} />
      <span id='audio-duration-label'>{formatTime(duration)}</span>
    </div>
  )
}
