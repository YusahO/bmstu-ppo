// import Select from 'react-select';

import { useEffect, useState } from "react";
import TagSelector from "../tag/TagSelector";
import '../tag/TagContainer.css';

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

		console.log(selectedTags);
	}, [selectedTags]);

	return (
		<div style={{
			backgroundColor: 'var(--color-primary)',

			width: '30vw',
			marginTop: '2px',
			height: '500px',

			position: 'fixed',
			zIndex: '2',
			padding: '0px 10px'
		}}>
			<div className="tag-container">
				{allTags.map(t => <TagSelector tag={t} onClick={() => onClickHandle(t)} />)}
			</div>
		</div>
	)
}

export default SearchOptions;