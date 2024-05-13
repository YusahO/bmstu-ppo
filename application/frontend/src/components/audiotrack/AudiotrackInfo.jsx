import './AudiotrackInfo.css';

import { useContext, useEffect, useState } from 'react';
import { UserContext } from '../../App.js';

import AudioPlaybackControls from './AudioPlaybackControls.jsx';
import StarRatingInteractive from '../score/StarRatingInteractive.jsx';
import AudiotrackTags from '../tag/AudiotrackTags.jsx';
import AudiotrackCommentaries from '../commentary/AudiotrackCommentaries.jsx';
import AudiotrackPlaylistEditor from '../playlist/AudiotrackPlaylistEditor.jsx';
import BlurComponent from '../common/BlurComponent.jsx';
import ReportForm from '../report/ReportForm.jsx';
import CloseButton from '../common/CloseButton.jsx';
import AudiotrackTagsEditor from '../tag/AudiotrackTagsEditor.jsx';

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

const AudiotrackInfo = ({ audiotrack, onClose }) => {
  const { user } = useContext(UserContext);

  const [score, setScore] = useState(null);
  const [isHoveringDropdown, setIsHoveringDropdown] = useState(false);
  const [showReportForm, setShowReportForm] = useState(false);

  useEffect(() => {
    if (!user) {
      setScore({ value: 0 });
      return;
    }

    fetch(`http://localhost:9898/api/scores/${user.id}/${audiotrack.id}`, {
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
      .then((data) => setScore(data.value))
      .catch((error) => {
        console.error('Error occured: ', error)
      })
  }, [user, audiotrack]);

  if (score === null) {
    return <p>Loading...</p>;
  }

  return (
    <BlurComponent>
      <div id='audiotrack-info-content'>
        <div style={{ position: 'absolute', top: '20px', right: '30px' }}>
          <CloseButton onClose={onClose} />
        </div>
        <div style={{ display: 'flex', justifyContent: 'space-between', height: '100%' }}>
          <div style={{ display: 'flex', flex: '0 0 55%', flexDirection: 'column', justifyContent: 'space-between' }}>
            <div style={{ display: 'flex', justifyContent: 'space-between' }}>
              <h1>
                {audiotrack.title}
              </h1>
              <StarRatingInteractive audiotrackId={audiotrack.id} initialStars={score} />
            </div>
            <div>
              <AudioPlaybackControls audiotrackParam={audiotrack} />
            </div>

            <div style={{ display: 'flex', gap: '20px' }}>
              <button onClick={() => downloadAudio(audiotrack.filepath)}>
                Скачать
              </button>
              <div
                onMouseEnter={() => setIsHoveringDropdown(true)}
                onMouseLeave={() => setIsHoveringDropdown(false)}
              >
                <button>
                  Изменить плейлисты
                </button>
                {isHoveringDropdown &&
                  <AudiotrackPlaylistEditor audiotrackId={audiotrack.id} />}
              </div>
              <div>
                <button onClick={() => setShowReportForm(!showReportForm)}>
                  Пожаловаться
                </button>
                {showReportForm && <ReportForm audiotrack={audiotrack} onClose={() => setShowReportForm(false)} />}
              </div>
            </div>

            <div>
              <h2>Теги</h2>
              {user && (user.isAdmin ? 
                <AudiotrackTagsEditor audiotrackId={audiotrack.id}/>:
                <AudiotrackTags audiotrackId={audiotrack.id} />
              )}
            </div>
          </div>

          <div style={{ flex: '0 0 40%', display: 'flex', flexDirection: 'column' }}>
            <h2>Комментарии</h2>
            <AudiotrackCommentaries audiotrackId={audiotrack.id} />
          </div>

        </div>
      </div>
    </BlurComponent>
  )
}

export default AudiotrackInfo;