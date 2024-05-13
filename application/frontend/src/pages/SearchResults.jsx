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

	const fetchSearchResults = () => {
		if (queryTags.length && queryTags[0] !== '') {
			const tags = queryTags.join('&tags=');
			fetch(`http://localhost:9898/api/SearchResults?tags=${tags}`, {
				mode: 'cors',
				method: 'GET',
			})
				.then((response) => response.json())
				.then((data) => setAudiosTags(data))
				.catch(error => console.log('Nothing found'));
		}

		if (queryTitle !== '') {
			fetch(`http://localhost:9898/api/SearchResults/${queryTitle}`, {
				mode: 'cors',
				method: 'GET',
			})
				.then((response) => response.json())
				.then((data) => setAudiosTitle(data))
				.catch(error => console.log('Nothing found'));
		}
	};

	useEffect(() => {
		setResults([]);
		setQueryTitle(searchParams.get('title'));
		setQueryTags(searchParams.get('tags').split(','));
	}, [location.search]);

	useEffect(() => {	
		fetchSearchResults();
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
			{results.length > 0 && <AudiotrackGrid renderAdd={false} audiotracks={results} />}
		</div>
	)
}

export default SearchResults;