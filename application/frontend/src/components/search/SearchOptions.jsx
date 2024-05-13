import { useEffect, useState } from "react";
import TagSelector from "../tag/TagSelector";

import '../tag/TagContainer.css';
import './SearchOptions.css';

const SearchOptions = ({ selectedTags, changeSelectedTags }) => {

	const [allTags, setAllTags] = useState([]);

	useEffect(() => {
		fetch(`http://localhost:9898/api/tags/`, {
			mode: 'cors',
			method: 'GET'
		})
			.then((response) => response.json())
			.then((data) => {
				setAllTags(data);
			})
			.catch(error => console.log(error));
	}, []);

	return (
		<div className="search-options">
			<div className="tag-container">
				{allTags.map(t =>
					<TagSelector
						tag={t}
						isSelected={selectedTags.some(tag => tag.id === t.id)}
						onClick={() => changeSelectedTags(t)}
					/>)}
			</div>
		</div>
	)
}

export default SearchOptions;