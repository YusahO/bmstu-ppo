import { useEffect, useState } from "react";
import TagsEditor from "../components/tag/TagsEditor";
import { api } from "../api/mpFetch";

const TagsPage = () => {
	const [tags, setTags] = useState(null);
	const [needUpdate, setNeedUpdate] = useState(false);

	useEffect(() => {
		api.get('tags')
			.then(response => setTags(response.data))
			.catch(error => console.error(error));
	}, [needUpdate]);

	if (!tags) {
		return <div>Loading...</div>
	}

	return (
		<div>
			<h2>Все теги</h2>
			<TagsEditor tags={tags} onClose={() => setNeedUpdate(!needUpdate)} />
		</div>
	);
}

export default TagsPage;