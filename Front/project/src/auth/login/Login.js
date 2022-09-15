import { useContext, useRef } from 'react';
import { Link } from 'react-router-dom';
import swal from 'sweetalert';

import Card from '../../UI/Card';
import Input from '../../UI/Input';
import Button from '../../UI/Button';
import {
  emailValidation,
  passwordValidation,
} from '../../misc/inputValidationFunctions';
import AuthContext from '../store/auth-context';
import useHttp from '../../hooks/use-http';

import styles from './Login.module.css';
import btnStyles from '../../UI/Button.module.css';

const Login = () => {
  const emailRef = useRef();
  const passwordRef = useRef();
  const authCtx = useContext(AuthContext);
  console.log(authCtx)
  const { isLoading, sendRequest } = useHttp();

  const loginSuccess = (data) => {
    swal('SUCCESS', 'Successfully logged in!', 'success');

    authCtx.login(
      data.token,
      new Date(new Date().getTime() + 1 * 60 * 60 * 1000).toISOString()
    );

    emailRef.current.refValue.current.value = '';
    passwordRef.current.refValue.current.value = '';
  };

  const loginFail = (errorMessage) => {
    swal('FAILED', errorMessage || 'Failed to login!', 'error');
  };

  const submitHandler = (event) => {
    event.preventDefault();

    if (authCtx.isLoggedIn) {
      swal('Oops', 'Already logged in!', 'warning');
      return;
    }

    let requestConfig = {
      url: 'http://localhost:5000/api/person/login',
      body: {
        Email: emailRef.current.rawValue,
        Password: passwordRef.current.rawValue,
      },
      method: 'POST',
      headers: { 'Content-type': 'application/json' },
    };

    sendRequest(requestConfig, loginSuccess, loginFail);
  };

  return (
    <form onSubmit={submitHandler}>
      <Card additionalStyles={styles.loginCard} center={true}>
        <Input
          type='text'
          label='Email: '
          validate={emailValidation}
          ref={emailRef}
        ></Input>
        <Input
          type='text'
          label='Password: '
          validate={passwordValidation}
          ref={passwordRef}
        ></Input>
        <Button
          additionalStyles={`${styles.loginButton} ${
            isLoading && btnStyles.registerLoading
          }`}
          buttonType='submit'
        >
          Log in
        </Button>
        <Link className={styles.registerNow} to='/register'>
          Don't have an account? <br />
          Register now!
        </Link>
      </Card>
    </form>
  );
};

export default Login;
