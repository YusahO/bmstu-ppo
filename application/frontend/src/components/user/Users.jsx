import React, { useEffect, useState } from 'react';
import User from "../../models/User";


function Users() {
    const [users, setUsers] = useState([]);

    const fetchUsers = () => {
        fetch('http://localhost:9898/api/users', { mode: 'cors' })
            .then((response) => response.json())
            .then((data) => {
                let userList = [];
                data.map((user) => {
                    userList.push({
                        ...User,
                        id: user.id,
                        username: user.username,
                        email: user.email,
                        isAdmin: user.isAdmin
                    });
                });
                setUsers(userList);
            })
            .catch(error => console.error('Error fetching users:', error));
    }

    useEffect(() => {
        fetchUsers()
    }, []);

    return (
        <div>
            <h2>User List</h2>
            <ul>
                {users.map(user => (
                    <li key={user.id}>
                        <strong>Id:</strong> {user.id}<br />
                        <strong>Username:</strong> {user.username}<br />
                        <strong>Email:</strong> {user.email}<br />
                        <strong>Password:</strong> {user.password}<br />
                        <strong>isAdmin:</strong> {user.isAdmin ? 'Yes' : 'No'}
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default Users;