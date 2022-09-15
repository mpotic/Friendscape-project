import { useCallback, useContext, useEffect, useState } from 'react';
import ClipLoader from 'react-spinners/ClipLoader';

import styles from './Requests.module.css';

import KeyValueTextBox from '../../UI/KeyValueTextBox';
import Card from '../../UI/Card';
import Button from '../../UI/Button';
import useHttp from '../../hooks/use-http';
import swal from 'sweetalert';
import AuthContext from '../../auth/store/auth-context';

const Requests = () => {
  const http = useHttp();
  const authCtx = useContext(AuthContext);
  const [requests, setRequests] = useState([]);

  const { token } = authCtx;
  const { sendRequest, isLoading } = http;

  const getRequestsSuccess = useCallback((data) => {
    setRequests(data.people);
    console.log('RECEIVED NEW REQUESTS! ' + data.people);
  }, []);

  const getRequestsFail = useCallback(() => {
    swal('Oops', 'No requests pending!', 'info');
  }, []);

  const getRequestsHandler = useCallback(() => {
    let requestConfig = {
      url: 'http://localhost:5000/api/person/get-requests',
      headers: { Authorization: `Bearer ${token}` },
      method: 'GET',
    };
    sendRequest(requestConfig, getRequestsSuccess, getRequestsFail);
  }, [sendRequest, token, getRequestsSuccess, getRequestsFail]);

  useEffect(() => {
    getRequestsHandler();
  }, [getRequestsHandler]);

  const approveSuccess = useCallback(
    (data) => {
      swal('Success', 'User approved!', 'success').then(getRequestsHandler());
    },
    [getRequestsHandler]
  );

  const approveFail = useCallback((errorMessage) => {
    swal('Oops', 'Failed to update request status!', 'error');
  }, []);

  const approveHandler = useCallback(
    (email) => {
      let requestConfig = {
        url: `http://localhost:5000/api/person/approve-request?email=${encodeURI(
          email
        )}&status=${encodeURI('USER')}`,
        headers: { Authorization: `Bearer ${token}` },
        method: 'PUT',
      };
      sendRequest(requestConfig, approveSuccess, approveFail);
    },
    [sendRequest, token, approveSuccess, approveFail]
  );

  return (
    <div className={styles.container}>
      <div className={styles.profileInfo}>Requests</div>
      {(!isLoading && requests.length) > 0 ? (
        requests.map((request, index) => (
          <Card additionalStyles={styles.cardStyle} key={index}>
            <KeyValueTextBox name={'Name: '} value={request.name} />
            <KeyValueTextBox name={'Surname: '} value={request.surname ?? ''} />
            <KeyValueTextBox name={'Email: '} value={request.email} />
            <Button
              buttonHandler={() => {
                return approveHandler(request.email);
              }}
              additionalStyles={styles.btnStyle}
            >
              Approve
            </Button>
          </Card>
        ))
      ) : (
        <div style={{ marginTop: '20px', alignSelf: 'center' }}>
          <ClipLoader loading={http.isLoading} color={'#ffffff'} size={150} />
        </div>
      )}
      {requests.length === 0 && (
        <Card additionalStyles={styles.emptyCard}>
          No pending requests!
        </Card>
      )}
    </div>
  );
};

export default Requests;
