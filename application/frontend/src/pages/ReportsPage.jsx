import './ReportsPage.css';
import { useEffect, useState } from "react";
import { apiAuth } from "../api/mpFetch";
import { AlertTypes, useAlertContext } from "../context/AlertContext";
import ReportElement from '../components/report/ReportElement';
import AllReports from "../components/report/AllReports"

const ReportsPage = () => {
	const { addAlert } = useAlertContext();
	const [reports, setReports] = useState(null);
	const [needUpdate, setNeedUpdate] = useState(false);
	const [activeReport, setActiveReport] = useState(null);

	useEffect(() => {
		apiAuth.get('reports')
			.then(response => { console.log('refresh'); setReports(response.data) })
			.catch(error => {
				addAlert(AlertTypes.error, error.message);
				console.error(error);
			});
	}, [needUpdate]);

	if (!reports) {
		return <div>Loading...</div>;
	}

	function handleDoubleClick(report, username) {
		setActiveReport({ ...report, username });
	}

	function handleStatusUpdate(newStatus) {
		apiAuth.put('reports', {
			id: activeReport.id,
			authorId: activeReport.authorId,
			audiotrackId: activeReport.audiotrackId,
			text: activeReport.text,
			status: newStatus
		})
			.then(() => {
				addAlert(AlertTypes.success, 'Статус обновлен!');
				setNeedUpdate(!needUpdate)
			})
			.catch(error => {
				addAlert(AlertTypes.error, error.message);
				console.error(error);
			})
	}

	return (
		<div className='reports-page'>
			<div className='reports-page-left'>
				<AllReports reports={reports} onDoubleClick={handleDoubleClick} />
			</div>
			<div className='reports-page-right'>
				{activeReport &&
					<ReportElement reportInfo={activeReport} onStatusUpdate={handleStatusUpdate} />}
			</div>
		</div>
	)
}

export default ReportsPage;