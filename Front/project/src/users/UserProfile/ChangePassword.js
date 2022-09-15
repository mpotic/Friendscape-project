import { Fragment, useContext, useRef } from 'react';
import swal from 'sweetalert';

import Card from '../../UI/Card';
import Button from '../../UI/Button';
import Input from '../../UI/Input';

import styles from './ChangePassword.module.css';
import useHttp from '../../hooks/use-http';
import AuthContext from '../../auth/store/auth-context';
import ClipLoader from 'react-spinners/ClipLoader';

const ChangePassword = () => {
  const newPasswordRef = useRef();
  const oldPasswordRef = useRef();
  const http = useHttp();
  const authCtx = useContext(AuthContext);

  const { token } = authCtx;
  const { sendRequest, isLoading } = http;

  const updatePasswordSuccess = () => {
    swal('Success', 'Password has been changed!', 'success');
  };

  const updatePasswordFail = (errorMessage) => {
    swal('Error', errorMessage || 'Unable to update the password!', 'error');
  };

  const updatePasswordHandler = (newPassword, oldPassword) => {
    let requestConfig = {
      url: 'http://localhost:5000/api/person/update-password',
      body: {
        OldPassword: oldPasswordRef.current.refValue.current.value,
        NewPassword: newPasswordRef.current.refValue.current.value,
      },
      headers: {
        Authorization: `Bearer ${token}`,
        'Content-type': 'application/json',
      },
      method: 'PUT',
    };
    sendRequest(requestConfig, updatePasswordSuccess, updatePasswordFail);
  };

  return (
    <Fragment>
      {!isLoading ? (
        <Card>
          <div className={styles.profileInfo}>Change password</div>
          <Input
            additionalStylesDiv={styles.containerStyle}
            additionalStylesLabel={styles.userLabel}
            additionalStylesInput={styles.passwordInput}
            label={'New password: '}
            ref={newPasswordRef}
          ></Input>
          <Input
            additionalStylesDiv={styles.containerStyle}
            additionalStylesLabel={styles.userLabel}
            additionalStylesInput={styles.passwordInput}
            label={'Old password: '}
            ref={oldPasswordRef}
          ></Input>
          <div className={styles.btnContainer}>
            <Button buttonHandler={updatePasswordHandler}>Update</Button>
          </div>
        </Card>
      ) : (
        <div style={{ marginTop: '20px', alignSelf: 'center' }}>
          <ClipLoader loading={http.isLoading} color={'#ffffff'} size={150} />
        </div>
      )}
    </Fragment>
  );
};

export default ChangePassword;
