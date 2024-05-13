import { useContext } from 'react';
import './SideBar.css';
import { UserContext } from '../App';

const SideBar = () => {
	const { user } = useContext(UserContext);
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