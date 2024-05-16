import './AudiotrackEditor.css';
import { useEffect, useState } from "react";
import Audiotrack from '../../models/Audiotrack.js';
import { apiAuth } from '../../api/mpFetch.js';
import { useUserContext } from '../../context/UserContext.js';

const AudiotrackEditor = ({ audiotrack, onDone }) => {
	const [file, setFile] = useState(null);
	const [title, setTitle] = useState('');
	const [path, setPath] = useState('');
	const { user } = useUserContext();

	useEffect(() => {
		if (audiotrack) {
			setTitle(audiotrack.title);
		}
	}, [audiotrack]);

	function handleAudioUpdate() {
		if (!file || title === '' || path === '') {
			return;
		}

		const audiotrackDto = {
			...Audiotrack,
			id: audiotrack.id,
			authorId: user.id,
			title: title,
			filepath: path.endsWith('.mp3') ? path : path + '.mp3',
		};

		let formData = new FormData();
		for (let key in audiotrackDto) {
			formData.append(key, audiotrackDto[key]);
		}
		formData.append('file', file);

		apiAuth.put('audiotracks', formData, {
			headers: { 'Content-Type': 'multipart/form-data' }
		})
			.then(() => onDone())
			.catch(error => console.error(error));
	}

	function handleAudioCreate() {
		if (!file || title === '' || path === '') {
			return;
		}

		const audiotrackDto = {
			...Audiotrack,
			authorId: user.id,
			title: title,
			filepath: path.endsWith('.mp3') ? path : path + '.mp3',
		};

		let formData = new FormData();
		for (let key in audiotrackDto) {
			formData.append(key, audiotrackDto[key]);
		}
		formData.append('file', file);

		apiAuth.post('audiotracks', formData, {
			headers: { 'Content-Type': 'multipart/form-data' }
		})
			.then(() => onDone())
			.catch(error => console.error(error));
	}

	return (
		<div className='audiotrack-editor'>
			<input
				id="file"
				type="file"
				accept=".mp3"
				onChange={(e) => setFile(e.target.files[0])}
				required={true}
			/>
			<label
				htmlFor="file"
				class="audiotrack-editor-upload"
			>
				{!file ? <span>Загрузить файл</span> : <span>{file.name}</span>}
			</label>
			<input
				type="text"
				placeholder="Название"
				value={title}
				required={true}
				onChange={e => setTitle(e.target.value)}
			/>
			<input
				type="text"
				placeholder="Имя файла на сервере"
				value={path}
				required={true}
				onChange={e => setPath(e.target.value)}
			/>
			<button onClick={() => { audiotrack ? handleAudioUpdate() : handleAudioCreate(); }}>
				Сохранить
			</button>
		</div>
	)
}

export default AudiotrackEditor;