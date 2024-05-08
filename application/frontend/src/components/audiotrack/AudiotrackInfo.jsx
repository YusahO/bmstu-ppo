import { useEffect, useState } from 'react';
import { parseJwt } from '../../Globals.js';
import './AudiotrackInfo.css';

import AudioPlaybackControls from './AudioPlaybackControls.jsx';
import StarRatingInteractive from '../score/StarRatingInteractive.jsx';
import AudiotrackTags from '../tag/AudiotrackTags.jsx';
import AudiotrackCommentaries from '../commentary/AudiotrackCommentaries.jsx';

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
    const userId = parseJwt(localStorage.getItem('accessToken')).id;

    fetch(`http://localhost:9898/api/scores/${userId}/${audiotrackParam.id}`, {
      mode: 'cors',
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
      }
    })
      .then((request) => {
        if (request.status === 401 || request.status === 204) {
          return JSON.stringify({ value: 0 });
        }
        return request.json();
      })
      .then((data) => {
        setScore(data.value);
      })
      .catch((error) => {
        console.error('Error occured: ', error)
      })
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
        <div style={{ display: 'flex', justifyContent: 'space-between' }}>
          <div style={{ display: 'flex', flex: '0 0 55%', flexDirection: 'column', gap: '40px' }}>
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
              <AudiotrackTags audiotrackId={audiotrackParam.id} />
            </div>
          </div>

          <div style={{ flex: '0 0 40%' }}>
            <h2>Комментарии</h2>
            <AudiotrackCommentaries audiotrackId={audiotrackParam.id}/>
          </div>

        </div>
      </div>
    </div>
  )
}

export default AudiotrackInfo;