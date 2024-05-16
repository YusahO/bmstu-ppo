import { api } from "../api/mpFetch";
import { useLocation } from "react-router-dom";
import { useEffect, useState } from "react";
import AudiotrackGrid from "../components/audiotrack/AudiotrackGrid";

const SearchResults = () => {

	const location = useLocation();
	const searchParams = new URLSearchParams(location.search);

	const [queryTags, setQueryTags] = useState(searchParams.get('tags').split(','));
	const [queryTitle, setQueryTitle] = useState(searchParams.get('title'));

	const [audiosTags, setAudiosTags] = useState([]);
	const [audiosTitle, setAudiosTitle] = useState([]);
	const [results, setResults] = useState([]);

	useEffect(() => {
		setResults([]);
		setQueryTitle(searchParams.get('title'));
		setQueryTags(searchParams.get('tags').split(','));
	}, [location.search]);

	useEffect(() => {
		if (queryTags.length && queryTags[0] !== '') {
			api.post('searchResults', queryTags)
				.then(response => setAudiosTags(response.data))
				.catch(() => console.log('Nothing found'));
		}
		if (queryTitle !== '') {
			api.get(`searchResults/${queryTitle}`)
				.then(response => setAudiosTitle(response.data))
				.catch(() => console.log('Nothing found'));
		}
	}, [queryTags, queryTitle]);


	useEffect(() => {
		if (queryTags.length && queryTags[0] !== '' && queryTitle !== '') {
			const combinedResults = audiosTags.filter(v => audiosTitle.some(a => v.id === a.id));
			setResults(combinedResults);
		} else if (queryTags.length && queryTags[0] === '') {
			setResults(audiosTitle);
		} else if (queryTitle === '') {
			setResults(audiosTags);
		} else {
			setResults([]);
		}
	}, [audiosTags, audiosTitle]);

	return (
		<div>
			<h2>Результаты поиска</h2>
			{results.length > 0 ?
				<AudiotrackGrid renderAdd={false} audiotracks={results} /> :
				<h3>Ничего не найдено</h3>
			}
		</div>
	)
}

export default SearchResults;