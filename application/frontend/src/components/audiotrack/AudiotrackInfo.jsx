import './AudiotrackInfo.css';
import { useEffect, useState } from 'react';
import AudioPlaybackControls from './AudioPlaybackControls.jsx';
import StarRatingInteractive from '../score/StarRatingInteractive.jsx';
import AudiotrackTags from '../tag/AudiotrackTags.jsx';
import AudiotrackCommentaries from '../commentary/AudiotrackCommentaries.jsx';
import AudiotrackPlaylistEditor from '../playlist/AudiotrackPlaylistEditor.jsx';
import BlurComponent from '../common/BlurComponent.jsx';
import ReportForm from '../report/ReportForm.jsx';
import CloseButton from '../common/CloseButton.jsx';
import AudiotrackTagsEditor from '../tag/AudiotrackTagsEditor.jsx';
import { api, apiAuth } from '../../api/mpFetch.js';
import { useUserContext } from '../../context/UserContext.js';

function downloadAudio(audiotrackFilename) {
  api.get(`audiotracks/${audiotrackFilename}`, { responseType: 'blob' })
    .then(response => {
      const url = window.URL.createObjectURL(
        new Blob([response.data])
      );
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', `${audiotrackFilename}`);

      document.body.appendChild(link);
      link.click();
      link.parentNode.removeChild(link);
    })
    .catch(error => console.error(error));
}

const AudiotrackInfo = ({ audiotrack, onClose }) => {
  const { user } = useUserContext();

  const [score, setScore] = useState(null);
  const [isHoveringDropdown, setIsHoveringDropdown] = useState(false);
  const [showReportForm, setShowReportForm] = useState(false);

  useEffect(() => {
    if (!user) {
      setScore({ value: 0 });
      return;
    }

    apiAuth.get('scores', {
      params: {
        authorId: user.id,
        audiotrackId: audiotrack.id
      }
    })
      .then(response => {
        if (response.status === 204) {
          setScore({ value: 0 });
          return;
        }
        setScore(response.data.value);
      })
      .catch(error => console.error(error));
  }, [user, audiotrack]);

  if (score === null) {
    return <p>Loading...</p>;
  }

  function selectTagsElement() {
    if (!user || !user.isAdmin)
      return <AudiotrackTags audiotrackId={audiotrack.id} />
    else if (user && user.isAdmin)
      return <AudiotrackTagsEditor audiotrackId={audiotrack.id} />
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
              {user &&
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
              }
              {user &&
                <div>
                  <button onClick={() => setShowReportForm(!showReportForm)}>
                    Пожаловаться
                  </button>
                  {showReportForm && <ReportForm audiotrack={audiotrack} onClose={() => setShowReportForm(false)} />}
                </div>
              }
            </div>

            <div>
              <h2>Теги</h2>
              {selectTagsElement()}
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