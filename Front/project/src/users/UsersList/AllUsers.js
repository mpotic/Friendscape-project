import { useState } from 'react';
import useHttp from '../../hooks/use-http';

import SearchBar from './SearchBar';
import UserAddFriend from './UserAddFriend';

import styles from './AllUsers.module.css';

const AllUsers = () => {
  const http = useHttp();
  const [results, setResults] = useState(undefined);

  const searchResults = (data) => {
    if (data.people.length > 0) {
      setResults(data.people);
    } else {
      setResults(undefined);
    }
  };

  return (
    <div className={styles.container}>
      <SearchBar searchResultsHandler={searchResults} http={http}></SearchBar>
      <UserAddFriend searchResultsHandler={searchResults} results={results} />
    </div>
  );
};

export default AllUsers;
