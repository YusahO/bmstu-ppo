import './ReportForm.css';

import { useContext, useState } from 'react';
import { UserContext } from '../../App';
import { useNavigate } from 'react-router-dom';

import Report from '../../models/Report.js';
import BlurComponent from "../common/BlurComponent";
import CloseButton from '../common/CloseButton';
import { ReportStatus } from '../../models/enums/ReportStatus';

const ReportForm = ({ audiotrack, onClose }) => {
	const { user } = useContext(UserContext);
	const [text, setText] = useState('');
	const navigate = useNavigate();

	function handleSendReport() {
		if (!user) {
			navigate('/auth');
			return;
		}
		fetch(`http://localhost:9898/api/reports`, {
			mode: 'cors',
			method: 'POST',
			headers: {
				'Authorization': `Bearer ${localStorage.getItem('accessToken')}`,
				'Content-Type': 'application/json; charset=utf-8'
			},
			body: JSON.stringify({
				...Report,
				authorId: user.id,
				audiotrackId: audiotrack.id,
				text: text
			})
		})
			.then(response => {
				if (response.status === 401) {
					navigate('/auth');
				}
				onClose();
			})
			.catch(error => console.error(error));
	}

	return (
		<BlurComponent style={{
			left: '50%',
			top: '50%',
			transform: 'translate(-50%, -50%)'
		}}>
			<div id="report-window">
				<h2>Отправка жалобы</h2>
				<textarea
					type='text'
					placeholder='Текст жалобы'
					value={text}
					multiline={true}
					onChange={e => setText(e.target.value)}
					style={{
						width: '100%', height: '100%'
					}}
				/>
				<button onClick={handleSendReport}>Отправить</button>
				<CloseButton onClose={onClose} />
			</div>
		</BlurComponent>
	)
}

export default ReportForm;