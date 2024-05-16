import './Reports.css';
import { useEffect, useState } from 'react';
import { apiAuth } from '../../api/mpFetch';

const ReportMinimized = ({ report, onDoubleClick }) => {
	const [author, setAuthor] = useState(null);

	useEffect(() => {
		apiAuth.get(`users/${report.authorId}`)
			.then(response => setAuthor(response.data))
			.catch(error => {
				console.error(error);
			})
	}, [report]);

	if (!author) {
		return <div>Loading...</div>;
	}

	function shouldColor() {
		if (report.status === 2)
			return ' accepted';
		else if (report.status === 3)
			return ' declined';
		return ''
	}

	return (
		<div
			className={'report-minimized' + shouldColor()}
			onDoubleClick={() => onDoubleClick(report, author.username)}
		>
			{report.status === 0 &&
				<svg
					viewBox='0 0 10 10'
					className='unread'
					xmlns="http://www.w3.org/2000/svg"
				>
					<circle cx="5" cy="5" r="5" />
				</svg>}
			<h3>От:&nbsp;{author.username}</h3>
			<label>{report.text}</label>
		</div>
	)
}

const AllReports = ({ reports, onDoubleClick }) => {
	return (
		<div className="report-minimized-container">
			{reports.map((report, index) =>
				<div key={index}>
					<ReportMinimized report={report} onDoubleClick={onDoubleClick} />
				</div>
			)}
		</div>
	)
}

export default AllReports;