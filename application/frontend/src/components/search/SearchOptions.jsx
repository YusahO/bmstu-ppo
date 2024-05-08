import { useEffect, useState } from "react";
import TagSelector from "../tag/TagSelector";

import '../tag/TagContainer.css';
import './SearchOptions.css';

const SearchOptions = () => {

	const [allTags, setAllTags] = useState([]);
	const [selectedTags, setSelectedTags] = useState([]);

	function onClickHandle(tag) {
		setSelectedTags(prevSelectedTags => {
			if (prevSelectedTags.some(t => t.id === tag.id)) {
				return prevSelectedTags.filter(t => t.id !== tag.id);
			} else {
				return [...prevSelectedTags, tag];
			}
		});
	}

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

		console.log(selectedTags);
	}, [selectedTags]);

	return (
		<div className="search-options">
			<div className="tag-container">
				{allTags.map(t => <TagSelector tag={t} onClick={() => onClickHandle(t)} />)}
			</div>
		</div>
	)
}

export default SearchOptions;