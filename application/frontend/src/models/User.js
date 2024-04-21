import { v4 as uuidv4 } from 'uuid';

const User = {
    id: uuidv4(),
    favouritesId: uuidv4(),
    username: "",
    email: "",
    password: "",
    isAdmin: false
};

export default User;
