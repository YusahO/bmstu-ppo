import { useEffect, useState } from "react";
import useSound from "use-sound";
import { AiFillPlayCircle, AiFillPauseCircle } from "react-icons/ai";
import { IconContext } from "react-icons";
import './AudioPlayer.css';

const AudioPlayer = ({ audiotrackId }) => {
  const [isPlaying, setIsPlaying] = useState(false);

  fetch(`http://localhost:9898/api/audiotracks/${audiotrackId}`, { mode: 'cors' })
    .then((response) => response.json())
    .then((data) => {
      let audiosList = [];
      data.map((audiotrack) => {
        audiosList.push({
          ...Audiotrack,
          title: audiotrack.title,
          filepath: audiotrack.filepath
        });
      });
      setAudiotracks(audiosList);
    })
    .catch(error => console.error('Error fetching users:', error));

  const [play, { pause, duration, sound }] = useSound();

  const playingButton = () => {
    if (isPlaying) {
      pause(); // приостанавливаем воспроизведение звука
      setIsPlaying(false);
    } else {
      play(); // воспроизводим аудиозапись
      setIsPlaying(true);
    }
  };

  return (
    <div className="audio-player-component">
      <h2>Playing Now</h2>
      <img
        className="musicCover"
        src="https://picsum.photos/200/200"
      />
      <div>
        <h3 className="title">Aboba</h3>
        <p className="subTitle">Amogus</p>
      </div>
      <div>
        {!isPlaying ? (
          <button className="playButton" onClick={playingButton}>
            <IconContext.Provider value={{ size: "3em", color: "#27AE60" }}>
              <AiFillPlayCircle />
            </IconContext.Provider>
          </button>
        ) : (
          <button className="playButton" onClick={playingButton}>
            <IconContext.Provider value={{ size: "3em", color: "#27AE60" }}>
              <AiFillPauseCircle />
            </IconContext.Provider>
          </button>
        )}
      </div>
    </div>
  );
}

export default AudioPlayer;