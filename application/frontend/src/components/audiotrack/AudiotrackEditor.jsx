import './AudiotrackEditor.css';

import { useContext, useEffect, useState } from "react";
import Audiotrack from '../../models/Audiotrack.js';
import { UserContext } from "../../App";
import { useNavigate } from 'react-router-dom';

const AudiotrackEditor = ({ audiotrack, onDone }) => {

	const navigate = useNavigate();
	const [file, setFile] = useState(null);
	const [title, setTitle] = useState('');
	const [path, setPath] = useState('');
	const { user } = useContext(UserContext);

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

		fetch(`http://localhost:9898/api/audiotracks`, {
			mode: 'cors',
			method: 'PUT',
			headers: {
				'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
			},
			body: formData
		})
			.then(response => {
				if (response.status === 401) {
					navigate('/auth')
				}
				onDone();
			})
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

		fetch(`http://localhost:9898/api/audiotracks`, {
			mode: 'cors',
			method: 'POST',
			headers: {
				'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
			},
			body: formData
		})
			.then(response => {
				if (response.status === 401) {
					navigate('/auth')
				}
				onDone();
			})
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