import './AlertNotify.css';
import { useAlertContext } from '../../context/AlertContext';

const AlertNotifies = () => {
	const { alerts, addAlert, removeAlert } = useAlertContext();

	const closeAlert = (id) => {
		removeAlert(id);
	};

	return (
		<div className='alert-container'>
			{alerts.map((alert) => (
				<div key={alert.id} id={alert.id} className={`alert ${alert.type}`}>
					<span className="closebtn" onClick={() => closeAlert(alert.id)}>
						&times;
					</span>
					{alert.message}
				</div>
			))}
		</div>
	);
};

export default AlertNotifies;