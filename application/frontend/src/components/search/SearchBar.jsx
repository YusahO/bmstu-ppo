import { useState } from "react";
import SearchOptions from "./SearchOptions";
import { useNavigate } from "react-router-dom";

const SearchBar = () => {
  const navigate = useNavigate();

  const [inputFocused, setInputFocused] = useState(false);
  const [isHoveringDropdown, setIsHoveringDropdown] = useState(false);

  const [title, setTitle] = useState("");
	const [selectedTags, setSelectedTags] = useState([]);

  function showElement() {
    if (inputFocused || isHoveringDropdown) {
      return <SearchOptions selectedTags={selectedTags} changeSelectedTags={changeSelectedTags} />;
    }
  }

	const changeSelectedTags = (tag) => {
    const tagPresent = selectedTags.some(t => t.id === tag.id);
		setSelectedTags(prevSelectedTags => {
			if (tagPresent) {
				return prevSelectedTags.filter(t => t.id !== tag.id);
			} else {
				return [...prevSelectedTags, tag];
			}
		});
    return tagPresent;
	}

  function handleSearch(e) {
    e.preventDefault();
    navigate(`/search?title=${title}&tags=${selectedTags.map(t => t.id).join(',')}`);

    setTitle('');
    setSelectedTags([]);
  }

  return (
    <div>
      <div
        onMouseEnter={() => setIsHoveringDropdown(true)}
        onMouseLeave={() => setIsHoveringDropdown(false)}
      >
        <input
          style={{ width: '30vw' }}
          className="search-bar"
          type="text"
          placeholder="Поиск по названию"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          onFocus={() => setInputFocused(true)}
          onBlur={() => setInputFocused(false)}
        />
        <button onClick={handleSearch}>⌕</button>
        {showElement()}
      </div>
    </div>
  );
}

export default SearchBar;