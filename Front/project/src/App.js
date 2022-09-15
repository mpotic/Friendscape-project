import { Fragment, useContext } from 'react';
import { Redirect, Route, Switch } from 'react-router-dom';

import Login from './auth/login/Login';
import Register from './auth/register/Register';
import Navigation from './menu/Navigation';
import AuthContext from './auth/store/auth-context';
import AllUsers from './users/UsersList/AllUsers';
import Profile from './users/UserProfile/Profile';
import Requests from './users/UserRequests/Requests';

const App = () => {
  const authCtx = useContext(AuthContext);

  return (
    <Fragment>
      <Navigation />
      <Switch>
        <Route path='/requests'>
          {authCtx.isLoggedIn && <Requests />}
          {!authCtx.isLoggedIn && <Redirect path='/requests' to='/login' />}
        </Route>
        <Route path='/profile'>
          {authCtx.isLoggedIn && <Profile />}
          {!authCtx.isLoggedIn && <Redirect path='/profile' to='/login' />}
        </Route>
        <Route path='/all-users'>
          <AllUsers />
        </Route>
        <Route path='/login'>
          {!authCtx.isLoggedIn && <Login />}
          {authCtx.isLoggedIn && <Redirect path='/login' to='/all-users' />}
        </Route>
        <Route path='/register'>
          <Register />
        </Route>
        <Route path='/'>
          {!authCtx.isLoggedIn && <Redirect path='/' to='/login' />}
          {authCtx.isLoggedIn && <Redirect path='/' to='/all-users' />}
        </Route>
      </Switch>
    </Fragment>
  );
};

export default App;
