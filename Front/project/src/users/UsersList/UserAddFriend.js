import { useContext, useState, useCallback, useEffect } from 'react';
import { useHistory } from 'react-router-dom';
import swal from 'sweetalert';
import ClipLoader from 'react-spinners/ClipLoader';

import Card from '../../UI/Card';
import KeyValueTextBox from '../../UI/KeyValueTextBox';
import Button from '../../UI/Button';
import useHttp from '../../hooks/use-http';
import AuthContext from '../../auth/store/auth-context';

import styles from './UserAddFriend.module.css';

const UserAddFriend = (props) => {
  const http = useHttp();
  const authCtx = useContext(AuthContext);
  const [selectState, setSelectState] = useState('ACQUAINTANCE');
  const [results, setResults] = useState([]);
  const history = useHistory();

  let { sendRequest } = http;
  let { token, isLoggedIn } = authCtx;

  const fetchSuccess = (data) => {
    setResults(data.people);
  };

  const fetchFail = (errorMessage) => {
    swal('Oops', errorMessage || 'Failed to load all the users!', 'warning');
  };

  const fetchAllPeople = useCallback(() => {
    let requestConfig = {
      url: 'http://localhost:5000/api/person/get-non-friends',
      headers: { Authorization: `Bearer ${token}` },
      method: 'GET',
    };
    sendRequest(requestConfig, fetchSuccess, fetchFail);
  }, [sendRequest, token]);

  const searchResults = props.results;

  useEffect(() => {
    if (isLoggedIn) {
      if (searchResults !== undefined) {
        setResults(searchResults);
      } else {
        fetchAllPeople();
      }
    } else {
      history.replace('/login');
    }
  }, [searchResults, fetchAllPeople, isLoggedIn, history]);

  const addFriendSuccess = (data) => {
    swal(
      'Success',
      data.message || 'Successfully added a new friend!',
      'success'
    );
    fetchAllPeople();
  };

  const addFriendFail = (errorMessage) => {
    swal('Oops', errorMessage || 'Failed to add a new friend!', 'warning');
  };

  const addFriendHandler = (email) => {
    let requestConfig = {
      url: `http://localhost:5000/api/person/add-friend?email=${encodeURI(
        email
      )}&type=${encodeURI(selectState)}`,
      headers: { Authorization: `Bearer ${token}` },
      method: 'POST',
    };
    sendRequest(requestConfig, addFriendSuccess, addFriendFail);
  };

  const onChangeSelectHandler = (event) => {
    setSelectState(event.target.value);
    console.log(event.target.value);
  };

  return (
    <div className={styles.container}>
      {!http.isLoading ? (
        results.map((element, index) => (
          <Card additionalStyles={styles.cardStyle} key={index}>
            <KeyValueTextBox name={'Name: '} value={element.name} />
            <KeyValueTextBox name={'Surname: '} value={element.surname} />
            <KeyValueTextBox name={'Email: '} value={element.email} />
            <select
              name='type'
              id='type'
              className={styles.select}
              onChange={onChangeSelectHandler}
              value={selectState}
            >
              <option value='ACQUAINTANCE'>Acquaintance</option>
              <option value='FRIEND'>Friend</option>
              <option value='BEST_FRIEND'>Best friend</option>
              <option value='GOOD_FRIEND'>Good friend</option>
            </select>
            <Button
              buttonHandler={() => {
                return addFriendHandler(element.email);
              }}
              additionalStyles={styles.btnStyle}
            >
              Add friend
            </Button>
          </Card>
        ))
      ) : (
        <div style={{ marginTop: '100px' }}>
          <ClipLoader loading={http.isLoading} color={'#ffffff'} size={150} />
        </div>
      )}
      {results.length === 0 && !http.isLoading && (
        <div style={{ marginTop: '100px' }}>
          <Card>No one left to make friends with :(</Card>
        </div>
      )}
    </div>
  );
};

export default UserAddFriend;
