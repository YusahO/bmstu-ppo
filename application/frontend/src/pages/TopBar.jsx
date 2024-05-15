import './TopBar.css';
import '../App.css';
import { useNavigate } from "react-router-dom";
import SearchBar from "../components/search/SearchBar";
import UserMinimized from "../components/user/UserMinimized";

const TopBar = ({ onSidebarClick }) => {

	const navigate = useNavigate();
	function handleClick() {
		navigate("/");
	}

	return (
		<div className='topbar'>

			<button
				className={'sidebar-open-button'}
				onClick={onSidebarClick}
			>
				☰
			</button>
			<div style={{ padding: '50px' }}>
				<UserMinimized />
			</div>
			<SearchBar />
			<label className={'mewing-pad-home'} onClick={handleClick}>
				MewingPad
			</label>
		</div>
	)
}

export default TopBar;