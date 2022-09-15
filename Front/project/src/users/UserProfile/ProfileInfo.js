import {
  useRef,
  useState,
  useContext,
  useCallback,
  useEffect,
  Fragment,
} from 'react';
import swal from 'sweetalert';
import ClipLoader from 'react-spinners/ClipLoader';

import useHttp from '../../hooks/use-http';
import AuthContext from '../../auth/store/auth-context';
import Card from '../../UI/Card';
import Input from '../../UI/Input';
import Button from '../../UI/Button';
import KeyValueTextBox from '../../UI/KeyValueTextBox';

import styles from './ProfileInfo.module.css';

const ProfileInfo = () => {
  const nameRef = useRef();
  const surnameRef = useRef();
  const authCtx = useContext(AuthContext);
  const [user, setUser] = useState({ name: '', surname: '' });
  const http = useHttp();

  const fetchSuccess = (data) => {
    setUser(data);
  };

  const fetchFail = (errorMessage) => {
    swal('Oops', errorMessage || 'Failed to fetch profile info', 'warning');
  };

  const { sendRequest, isLoading } = http;
  const { token } = authCtx;

  const fetchProfile = useCallback(() => {
    let requestConfig = {
      url: 'http://localhost:5000/api/person/get-person-by-email',
      headers: { Authorization: `Bearer ${token}` },
      method: 'GET',
    };
    sendRequest(requestConfig, fetchSuccess, fetchFail);
  }, [sendRequest, token]);

  const changeSuccess = () => {
    swal('Success', 'Successfully changed profile information!', 'success');
    fetchProfile();
  };

  const changeFail = (message) => {
    swal('Oops', message || 'Failed to change profile info!', 'error');
    fetchProfile();
  };

  const changeProfile = () => {
    let requestConfig = {
      url:
        'http://localhost:5000/api/person/update-person?name=' +
        `${encodeURI(
          nameRef.current.refValue.current.value
        )}&surname=${encodeURI(surnameRef.current.refValue.current.value)}`,
      headers: { Authorization: `Bearer ${token}` },
      method: 'PUT',
    };
    sendRequest(requestConfig, changeSuccess, changeFail);
  };

  useEffect(() => {
    fetchProfile();
  }, [fetchProfile]);

  useEffect(() => {
    nameRef.current.refValue.current.value = user.name;
    surnameRef.current.refValue.current.value = user.surname;
  }, [user.name, user.surname]);

  const deleteSuccess = () => {
    swal('Success', 'Account deleted!', 'success').then(() => {
      authCtx.logout();
    });
  };

  const deleteFail = () => {
    swal('Error', "Couldn't delete the account!", 'error');
  };

  const onDeleteHandler = () => {
    let requestConfig = {
      url: 'http://localhost:5000/api/person/delete-account',
      headers: { Authorization: `Bearer ${token}` },
      method: 'DELETE',
    };
    sendRequest(requestConfig, deleteSuccess, deleteFail);
  };

  return (
    <Fragment>
      {!isLoading ? (
        <Card>
          <div className={styles.profileInfo}>Profile info</div>
          <Input
            additionalStylesDiv={styles.containerStyle}
            additionalStylesLabel={styles.userLabel}
            additionalStylesInput={styles.userInput}
            label={'Name: '}
            ref={nameRef}
          ></Input>
          <Input
            additionalStylesDiv={styles.containerStyle}
            additionalStylesInput={styles.userInput}
            additionalStylesLabel={styles.userLabel}
            label={'Surname: '}
            ref={surnameRef}
          ></Input>
          <KeyValueTextBox
            name='Email: '
            value={user.email}
            additionalStylesValue={styles.valueStyle}
          />
          <div className={styles.btnContainer}>
            <Button buttonHandler={onDeleteHandler}>Delete</Button>
            <Button buttonHandler={changeProfile}>Change</Button>
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

export default ProfileInfo;
