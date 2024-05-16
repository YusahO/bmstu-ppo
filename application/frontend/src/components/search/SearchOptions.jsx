import '../tag/TagContainer.css';
import './SearchOptions.css';
import { useEffect, useState } from "react";
import TagSelector from "../tag/TagSelector";
import { api } from "../../api/mpFetch";

const SearchOptions = ({ selectedTags, changeSelectedTags }) => {
	const [allTags, setAllTags] = useState([]);

	useEffect(() => {
		api.get('tags')
			.then(response => setAllTags(response.data))
			.catch(error => console.error(error));
	}, []);

	return (
		<div className="search-options">
			<div className="tag-container">
				{allTags.map((t, index) =>
					<div key={index}>
						<TagSelector
							tag={t}
							isSelected={selectedTags.some(tag => tag.id === t.id)}
							onClick={() => changeSelectedTags(t)}
						/>
					</div>)}
			</div>
		</div>
	)
}

export default SearchOptions;