import { Link } from 'react-router-dom';
import { useContext, useState, useCallback, useEffect } from 'react';
import swal from 'sweetalert';

import AuthContext from '../auth/store/auth-context';
import Header from '../UI/Header';
import Button from '../UI/Button';
import styles from './Navigation.module.css';
import useHttp from '../hooks/use-http';

const Navigation = () => {
  const authCtx = useContext(AuthContext);
  const http = useHttp();
  const [user, setUser] = useState({});

  const onLogout = () => {
    authCtx.logout();
  };

  const fetchSuccess = (data) => {
    setUser(data);
  };

  const fetchFail = (errorMessage) => {
    swal('Oops', errorMessage || 'Failed to fetch profile info!', 'error');
  };

  const { sendRequest } = http;
  const { token } = authCtx;

  const fetchProfile = useCallback(() => {
    let requestConfig = {
      url: 'http://localhost:5000/api/person/get-person-by-email',
      headers: { Authorization: `Bearer ${token}` },
      method: 'GET',
    };
    sendRequest(requestConfig, fetchSuccess, fetchFail);
  }, [sendRequest, token]);

  const {isLoggedIn} = authCtx;
  useEffect(() => {
    if (isLoggedIn) {
      fetchProfile();
    }
  }, [fetchProfile, isLoggedIn]);

  return (
    <Header>
      <Link to='/login' style={{ textDecoration: 'none', marginRight: 'auto' }}>
        <h1 className={styles.title}>Friendscape</h1>
      </Link>
      {authCtx.isLoggedIn && user.role === 'ADMIN' ? (
        <Link to='/requests' style={{ textDecoration: 'none' }}>
          <Button>Requests</Button>
        </Link>
      ) : (
        ''
      )}
      {authCtx.isLoggedIn ? (
        <Link to='/profile' style={{ textDecoration: 'none' }}>
          <Button>Profile</Button>
        </Link>
      ) : (
        ''
      )}
      {authCtx.isLoggedIn ? (
        <Link to='/login' style={{ textDecoration: 'none' }}>
          <Button buttonHandler={onLogout}>Log out</Button>
        </Link>
      ) : (
        <Link to='/login' style={{ textDecoration: 'none' }}>
          <Button>Log in</Button>
        </Link>
      )}
    </Header>
  );
};

export default Navigation;
