import { useRef, useContext } from 'react';

import swal from 'sweetalert';
import Input from '../../UI/Input';
import Button from '../../UI/Button';
import useHttp from '../../hooks/use-http';
import AuthContext from '../../auth/store/auth-context';

import styles from './SearchBar.module.css';

const SearchBar = (props) => {
  const searchRefName = useRef();
  const searchRefSurname = useRef();
  const http = useHttp();
  const authCtx = useContext(AuthContext);
  let { sendRequest } = http;
  let { token } = authCtx;

  const searchSuccess = (data) => {
    if (data.people.length === 0) {
      swal('Oops', 'No data was found!', 'warning');
      return;
    }

    props.searchResultsHandler(data);
  };

  const searchFail = (errorMessage) => {
    swal('Oops', errorMessage || 'No data was found!', 'warning');
  };

  const searchHandler = () => {
    let name = searchRefName.current.refValue.current.value
      .trim()
      .split(' ')[0];
    let surname = searchRefSurname.current.refValue.current.value
      .trim()
      .split(' ')[0];

    let requestConfig = {
      url:
        'http://localhost:5000/api/person/search?' +
        `name=${encodeURI(name ?? '')}&surname=${encodeURI(surname ?? '')}`,
      headers: { Authorization: `Bearer ${token}` },
      method: 'GET',
    };
    sendRequest(requestConfig, searchSuccess, searchFail);
    searchRefName.current.refValue.current.value = '';
    searchRefSurname.current.refValue.current.value = '';
  };

  return (
    <div className={styles.searchDiv}>
      <Input
        ref={searchRefName}
        label={''}
        additionalStylesLabel={styles.searchLabel}
        additionalStylesInput={styles.searchInput}
        placeholder={'Name'}
      ></Input>{' '}
      <Input
        ref={searchRefSurname}
        label={''}
        additionalStylesLabel={styles.searchLabel}
        additionalStylesInput={styles.searchInput}
        placeholder={'Surname'}
      ></Input>
      <Button buttonHandler={searchHandler}>Search</Button>
    </div>
  );
};

export default SearchBar;
