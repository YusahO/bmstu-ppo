class AuthService {
  login(email, password) {
    return fetch('http://localhost:9898/api/auth/login', {
      method: 'POST',
      mode: "cors",
      headers: {
        "Content-Type": "application/json; charset=utf-8",
      },
      body: JSON.stringify({ email: email, password: password }),
    })
      .then((response) => {
        if (!response.ok) {
          throw new Error('Login failed');
        }
        return response.json();
      })
      .then((data) => {
        if (data.tokenDto.accessToken) {
          console.log(data.tokenDto.accessToken);
          localStorage.setItem('accessToken', data.tokenDto.accessToken);
        }
        return data;
      });
  }

  logout() {
    localStorage.removeItem('user');
  }

  getCurrentUser() {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : undefined;
  }
}

export default new AuthService();
