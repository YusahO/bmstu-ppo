import { useEffect, useState } from "react";
import AudioPlayer from "../audiotrack/AudioPlayer"
import { api } from "../../api/mpFetch";
import { AlertTypes, useAlertContext } from "../../context/AlertContext";
import { ReportStatus } from "../../models/enums/ReportStatus";

const ReportElement = ({ reportInfo, onStatusUpdate }) => {
	const { addAlert } = useAlertContext();
	const [audiotrack, setAudiotrack] = useState(null);
	const [selectedStatus, setSelectedStatus] = useState(reportInfo.status);

	useEffect(() => {
		api.get(`audiotracks/${reportInfo.audiotrackId}`)
			.then(response => setAudiotrack(response.data))
			.catch(error => {
				addAlert(AlertTypes.error, error.message);
				console.error(error);
			});
	}, [reportInfo.audiotrackId]);

	useEffect(() => {
		setSelectedStatus(reportInfo.status);
	}, [reportInfo.status]);

	function handleReportStatusChange(newStatus) {
		if (newStatus !== reportInfo.status) {
			switch (newStatus) {
				case '0':
					onStatusUpdate(ReportStatus.NotViewed);
					break;
				case '1':
					onStatusUpdate(ReportStatus.Viewed);
					break;
				case '2':
					onStatusUpdate(ReportStatus.Accepted);
					break;
				case '3':
					onStatusUpdate(ReportStatus.Declined);
					break;
			}
		}
		reportInfo.status = newStatus;
	}

	if (!audiotrack) {
		return <div>Loading...</div>
	}

	return (
		<div style={{ padding: '20px', width: '90%' }}>
			<div style={{
				display: 'flex',
				justifyContent: 'space-between',
				width: '100%',
				alignItems: 'center'
			}}>
				<div style={{ flex: '0 0 70%' }}>
					<AudioPlayer audiotrack={audiotrack} />
				</div>
				<select
					value={selectedStatus}
					onChange={e => handleReportStatusChange(e.target.value)}
					style={{ flex: '0 0 5%', color: 'var(--font-color)' }}
				>
					<option value="0">Не просмотрено</option>
					<option value="1">Просмотрено</option>
					<option value="2">Принято</option>
					<option value="3">Отклонено</option>
				</select>
			</div>
			<div style={{
				marginTop: '50px',
				padding: '20px 0',
				borderTop: '1px solid var(--accent-color2)',
				width: '100%'
			}}>
				<h2 style={{ marginBottom: '10px' }}>{reportInfo.username}</h2>
				<label>{reportInfo.text}</label>
			</div>
		</div>
	)
}

export default ReportElement;