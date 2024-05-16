import './ReportForm.css';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { apiAuth } from '../../api/mpFetch.js';
import Report from '../../models/Report.js';
import BlurComponent from "../common/BlurComponent";
import CloseButton from '../common/CloseButton';
import { useUserContext } from '../../context/UserContext.js';
import { AlertTypes, useAlertContext } from '../../context/AlertContext.js';

const ReportForm = ({ audiotrack, onClose }) => {
	const { user } = useUserContext();
	const { addAlert } = useAlertContext();
	const [text, setText] = useState('');
	const navigate = useNavigate();

	function handleSendReport() {
		if (!user) {
			navigate('/auth');
			return;
		}
		apiAuth.post('reports', {
			...Report,
			authorId: user.id,
			audiotrackId: audiotrack.id,
			text: text
		})
			.then(() => {
				addAlert(AlertTypes.info, 'Жалоба отправлена');
				onClose()
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