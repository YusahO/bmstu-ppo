import './UserMinimized.css';
import { Link } from 'react-router-dom';
import { useContext, useEffect, useState } from 'react';
import { UserContext } from '../../App';

const UserMinimized = () => {
  const [text, setText] = useState('Вход');
  const { user } = useContext(UserContext);

  useEffect(() => {
    setText(user ? user.username : 'Вход');
  }, [user]);

  return (
    <div style={{ color: 'var(--accent-color2)' }}>
      <Link className='username-link' to={'/auth'}>
        {text}
      </Link>
    </div>
  )
}

export default UserMinimized;