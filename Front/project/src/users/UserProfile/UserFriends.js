import { Fragment, useContext, useState, useCallback, useEffect } from 'react';
import ClipLoader from 'react-spinners/ClipLoader';
import swal from 'sweetalert';

import Card from '../../UI/Card';
import KeyValueTextBox from '../../UI/KeyValueTextBox';
import Button from '../../UI/Button';
import useHttp from '../../hooks/use-http';

import styles from './UserFriends.module.css';
import AuthContext from '../../auth/store/auth-context';

const UserFriends = () => {
  const http = useHttp();
  const authCtx = useContext(AuthContext);
  const [friends, setFriends] = useState([]);

	const {isLoading, sendRequest}  = http;
	const {token} = authCtx;

	const loadFriendsSuccess = (data) => {
    setFriends(data.friends);
  };

  const loadFriendsFail = () => {
    swal('Error', "Couldn't retrieve the friend list", 'error');
  };

  const loadFriendsHandler = useCallback(() => {
    let requestConfig = {
      url: 'http://localhost:5000/api/person/all-friends',
      headers: { Authorization: `Bearer ${token}` },
      method: 'GET',
    };
    sendRequest(requestConfig, loadFriendsSuccess, loadFriendsFail);
  }, [sendRequest, token]);

  const removeFriendSuccess = () => {
    swal('Success', 'Your friendship has ended!', 'success');
    loadFriendsHandler();
  };

  const removeFriendFail = () => {
    swal('Oops', 'Failed to remove friend!', 'warning');
  };

  const removeFriendHandler = (email) => {
    let requestConfig = {
      url: `http://localhost:5000/api/person/remove-friend?email=${encodeURI(
        email
      )}`,
      headers: { Authorization: `Bearer ${token}` },
      method: 'DELETE',
    };
    sendRequest(requestConfig, removeFriendSuccess, removeFriendFail);
  };

  useEffect(() => {
    loadFriendsHandler();
  }, [loadFriendsHandler]);

  return (
    <Fragment>
      <div
        style={{
          marginTop: '10px',
          marginBottom: '0px',
          alignSelf: 'center',
        }}
        className={styles.profileInfo}
      >
        Friends
      </div>
      {!isLoading ? (
        friends.map((friend, index) => (
          <Card additionalStyles={styles.cardStyle} key={index}>
            <KeyValueTextBox name={'Name: '} value={friend.friend.name} />
            <KeyValueTextBox name={'Surname: '} value={friend.friend.surname} />
            <KeyValueTextBox name={'Email: '} value={friend.friend.email} />
            <KeyValueTextBox
              name={'Type: '}
              value={friend.type.replace('_', ' ')}
            />
            <Button
              buttonHandler={() => {
                return removeFriendHandler(friend.friend.email);
              }}
              additionalStyles={styles.btnStyle}
            >
              Remove friend
            </Button>
          </Card>
        ))
      ) : (
        <div style={{ marginTop: '20px', alignSelf: 'center' }}>
          <ClipLoader loading={http.isLoading} color={'#ffffff'} size={150} />
        </div>
      )}
      {friends.length === 0 && !isLoading && (
        <div style={{ marginTop: '20px', alignSelf: 'center' }}>
          <Card>Ready to make some friends?</Card>
        </div>
      )}
    </Fragment>
  );
};

export default UserFriends;
