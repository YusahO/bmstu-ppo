// import { useEffect, useRef, useState } from 'react';
import AudioPlaybackControls from './AudioPlaybackControls.jsx';
import StarRatingInteractive from '../score/StarRatingInteractive.jsx';
import './AudiotrackInfo.css';
import { useEffect, useState } from 'react';

function downloadAudio(audiotrackFilename) {
  fetch(`http://localhost:9898/api/audiotracks/${audiotrackFilename}`, { mode: 'cors' })
    .then((response) => response.blob())
    .then((blob) => {
      const url = window.URL.createObjectURL(
        new Blob([blob])
      );
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', `${audiotrackFilename}`);

      document.body.appendChild(link);
      link.click();
      link.parentNode.removeChild(link);
    })
}

const AudiotrackInfo = ({ audiotrackParam, onClose }) => {

  const [score, setScore] = useState(null);

  useEffect(() => {
    fetch(`http://localhost:9898/api/scores/${audiotrackParam.id}`, { mode: 'cors' })
      .then((request) => request.json())
      .then((data) => {
        let stars = 0;
        let len = 0;
        data.map((d) => {
          stars += d.value;
          ++len;
        })
        setScore(Math.round(stars / len));
      })
      .catch((error) => console.log('Error occured: ', error))
  }, [audiotrackParam]);

  if (score === null) {
    return <p>Loading...</p>;
  }

  return (
    <div id='audiotrack-info'>
      <div id='audiotrack-info-content'>
        <div style={{ position: 'absolute', top: '20px', right: '30px' }}>
          <button id='audiotrack-info-close' onClick={onClose}>
            X
          </button>
        </div>
        <div style={{ display: 'flex', gap: '40px', top: '-30px' }}>
          <div style={{ display: 'flex', flexDirection: 'column', gap: '40px' }}>
            <div style={{ display: 'flex', justifyContent: 'space-between' }}>
              <h1>
                {audiotrackParam.title}
              </h1>
              <StarRatingInteractive audiotrackId={audiotrackParam.id} initialStars={score} />
            </div>
            <div>
              <AudioPlaybackControls audiotrackParam={audiotrackParam} />
            </div>

            <div style={{ display: 'flex', gap: '20px' }}>
              <button onClick={() => downloadAudio(audiotrackParam.filepath)}>Скачать</button>
              <button>В Избранное</button>
              <button>Добавить в плейлист</button>
              <button>Пожаловаться</button>
            </div>

            <div>
              <h2>Теги</h2>
              {/* TagsGrid */}
            </div>
          </div>

          <h2>Комментарии</h2>

        </div>
      </div>
    </div>
  )
}

export default AudiotrackInfo;