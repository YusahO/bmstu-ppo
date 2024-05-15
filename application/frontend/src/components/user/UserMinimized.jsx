import './UserMinimized.css';
import { useNavigate } from 'react-router-dom';
import { useUserContext } from '../../context/UserContext';
import { useCookies } from 'react-cookie';
import { AlertTypes, useAlertContext } from '../../context/AlertContext';

const UserMinimized = () => {
  const { user, setUser } = useUserContext();
  const { addAlert } = useAlertContext();
  const [cookies, setCookie, removeCookie] = useCookies();
  const navigate = useNavigate();

  function handleExit() {
    removeCookie('token');
    setUser(null);
    addAlert(AlertTypes.info, 'Вы вышли из аккаунта');
    navigate('/');
  }

  return (
    <div className='user-minimized'>
      {user ?
        <div className='user-options'>
          <label onClick={() => navigate('/auth')}>{user.username}</label>
          <label onClick={handleExit}>Выход</label>
        </div> :
        <label onClick={() => navigate('/auth')}>Вход</label>
      }
    </div>
  )
}

export default UserMinimized;