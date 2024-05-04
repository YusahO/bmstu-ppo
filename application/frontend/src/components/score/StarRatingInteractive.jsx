import React, { useState } from 'react';
import Score from '../../models/Score.js';
import './StarRatingInteractive.css';

const StarRatingInteractive = ({ audiotrackId, initialStars }) => {
  const [selectedStars, setSelectedStars] = useState(initialStars);

  const handleStarClick = (value) => {
    setSelectedStars(value);

    let authorId = JSON.parse(localStorage.getItem('user')).id;
    fetch(`http://localhost:9898/api/scores/`, {
      method: 'PUT',
      mode: 'cors',
      headers: {
        "Content-Type": "application/json; charset=utf-8",
      },
      body: JSON.stringify({ ...Score, audiotrackId: audiotrackId, authorId: authorId }),
    });
  };

  return (
    <div className="rate">
      {[5, 4, 3, 2, 1].map((starValue) => (
        <React.Fragment key={starValue}>
          <input
            type="radio"
            id={`star${starValue}`}
            name="rate"
            value={starValue}
            checked={selectedStars === starValue}
            onChange={() => handleStarClick(starValue)}
          />
          <label htmlFor={`star${starValue}`} title={`${starValue} stars`}>
            {starValue} stars
          </label>
        </React.Fragment>
      ))}
    </div>
  );
};

export default StarRatingInteractive;