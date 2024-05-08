import { useNavigate } from "react-router-dom";
import SearchBar from "../components/search/SearchBar";
import UserMinimized from "../components/user/UserMinimized";

import './UpperPanel.css';

const UpperPanel = ({ displayFunctional }) => {
	const navigate = useNavigate();

	console.log(displayFunctional);

	function handleClick() {
		navigate("/");
	}

	function displayComponents() {
		if (displayFunctional) {
			return (
				<>
					<UserMinimized />
					<SearchBar />
				</>
			);
		} else {
			return <div></div>;
		}
	}

	return (
		<div className="top-panel">
			{
				displayComponents()
			}

			<label style={{
				fontSize: '30px',
				fontStyle: 'italic'
			}} onClick={handleClick}>
				MewingPad
			</label>

		</div>
	)
}

export default UpperPanel;