import './TopBar.css';
import '../App.css';
import { useNavigate } from "react-router-dom";
import SearchBar from "../components/search/SearchBar";
import UserMinimized from "../components/user/UserMinimized";
import { useUserContext } from '../context/UserContext';

const TopBar = ({ onSidebarClick }) => {
	const navigate = useNavigate();
	const { user } = useUserContext();

	return (
		<div className='topbar'>

			{user &&
				<button
					className='sidebar-open-button'
					onClick={onSidebarClick}
				>
					☰
				</button>
			}
			<div style={{ padding: '50px' }}>
				<UserMinimized />
			</div>
			<SearchBar />
			<label className={'mewing-pad-home'} onClick={() => navigate("/")}>
				MewingPad
			</label>
		</div>
	)
}

export default TopBar;