import { useEffect, useState } from "react";
import SearchOptions from "./SearchOptions";
import './SearchBar.css';

const SearchBar = () => {

  const [title, setTitle] = useState("");
  const [audiotracks, setAudiotracks] = useState([]);

  const fetchAudiotracks = async (title) => {
    try {
      const response = await fetch(`http://localhost:9898/api/search/${title}`, {
        mode: 'cors',
        method: 'GET'
      });
      const data = await response.json();
      setAudiotracks(data);
    }
    catch (error) {
      return console.log(error);
    }
  }

  useEffect(() => {
    let isMounted = true;
    if (title && isMounted) {
      fetchAudiotracks(title);
    }
    return () => { isMounted = false };
  }, [title]);


  const handleInputChange = (e) => {
    setTitle(e.target.value);
  }

  const [inputFocused, setInputFocused] = useState(false);
  const [isHoveringDropdown, setIsHoveringDropdown] = useState(false);

  const handleMouseEnterDropdown = () => {
    console.log('is hovering')
    setIsHoveringDropdown(true);
  }

  const handleMouseLeaveDropdown = () => {
    setIsHoveringDropdown(false);
  }

  const handleFocusIn = (e) => {
    setInputFocused(true);
  }

  const handleFocusOut = (e) => {
    setInputFocused(false);
  }

  function showElement() {
    if (inputFocused || isHoveringDropdown) {
      return <SearchOptions />;
    }
  }

  return (
    <div>
      <input
        className="search-bar"
        type="text"
        placeholder="Поиск по названию"
        value={title}
        onChange={handleInputChange}
        onFocus={handleFocusIn}
        onBlur={handleFocusOut}
      />
      <div
        onMouseEnter={handleMouseEnterDropdown}
        onMouseLeave={handleMouseLeaveDropdown}
      >
        {showElement()}
      </div>
    </div>
  );
}

export default SearchBar;