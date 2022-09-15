import { useRef } from 'react';
import swal from 'sweetalert';

import Card from '../../UI/Card';
import Input from '../../UI/Input';
import Button from '../../UI/Button';
import useHttp from '../../hooks/use-http';

import styles from './Register.module.css';
import btnStyles from '../../UI/Button.module.css'
import {
  emailValidation,
  passwordValidation,
  regularFieldValidation,
} from '../../misc/inputValidationFunctions';

const Register = () => {
  const firstNameInput = useRef();
  const lastNameInput = useRef();
  const emailInput = useRef();
  const passwordInput = useRef();
  const { isLoading, sendRequest } = useHttp();

  const registerSuccess = (data) => {
    swal('Success', 'Successfully registered a new user!', 'success');
    firstNameInput.current.refValue.current.value = ""
    lastNameInput.current.refValue.current.value = ""
    emailInput.current.refValue.current.value = ""
    passwordInput.current.refValue.current.value = ""
  };

  const registerFail = (errorMessage) => {
    swal('FAILED', errorMessage || 'Failed to register a new user!', 'error');
  };

  const submitHandler = (event) => {
    event.preventDefault();
    if (isLoading) {
      return;
    }

    let requestConfig = {
      url: 'http://localhost:5000/api/person/register',
      body: {
        Name: firstNameInput.current.rawValue,
        Surname: lastNameInput.current.rawValue,
        Email: emailInput.current.rawValue,
        Password: passwordInput.current.rawValue,
      },
      method: 'POST',
      headers: { 'Content-type': 'application/json' },
    };

    sendRequest(requestConfig, registerSuccess, registerFail);
  };

  return (
    <form onSubmit={submitHandler}>
      <Card additionalStyles={styles.registerCard}>
        <Input
          type='text'
          label='First name: '
          ref={firstNameInput}
          validate={regularFieldValidation}
        ></Input>
        <Input
          type='text'
          label='Last name: '
          ref={lastNameInput}
          validate={regularFieldValidation}
        ></Input>
        <Input
          type='text'
          label='Email: '
          ref={emailInput}
          validate={emailValidation}
        ></Input>
        <Input
          type='text'
          label='Password: '
          ref={passwordInput}
          validate={passwordValidation}
        ></Input>
        <Button
          additionalStyles={`${styles.registerButton} ${
            isLoading && btnStyles.registerLoading
          }`}
          buttonType='submit'
        >
          Register
        </Button>
      </Card>
    </form>
  );
};

export default Register;
