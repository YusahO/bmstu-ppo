import { Link } from 'react-router-dom';
import './UserMinimized.css';

const UserMinimized = () => {

  // const fetchUser = () => {

  // }

  // useEffect(() => {
  //   getUsername();
  // }, [username]);

  return (
    <div style={{ color: 'var(--accent-color2)' }}>
      <Link className='username-link' to={'/login'}>aboba</Link>
    </div>
  )
}

export default UserMinimized;