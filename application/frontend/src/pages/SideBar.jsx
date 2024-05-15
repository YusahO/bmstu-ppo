import './SideBar.css';
import { useUserContext } from '../context/UserContext';

const SideBar = () => {
	const { user } = useUserContext();
	return (
		<div id='sidebarComp' className='sidepanel'>
			<a href='/playlists'>Мои плейлисты</a>
			{user && user.isAdmin && 
				<div>
					<a href='/reports'>Жалобы</a>
					<a href='/tags'>Теги</a>
				</div>
			}
		</div>
	);
}

export default SideBar;