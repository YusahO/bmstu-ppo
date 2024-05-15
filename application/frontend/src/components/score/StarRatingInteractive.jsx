import './StarRatingInteractive.css';
import React, { useState } from 'react';
import Score from '../../models/Score.js';
import { apiAuth } from '../../api/mpFetch.js';

const StarRatingInteractive = ({ audiotrackId, initialStars }) => {

  const [selectedStars, setSelectedStars] = useState(initialStars);

  const handleStarClick = (value) => {
    apiAuth.put('scores', { ...Score, value: value, audiotrackId: audiotrackId })
      .then(() => setSelectedStars(value))
      .catch(error => console.error(error));
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