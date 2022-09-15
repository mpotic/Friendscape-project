import { useState, useCallback } from 'react';

const useHttp = () => {
  const [isLoading, setIsLoading] = useState(false);

  const sendRequest = useCallback(
    async (requestConfig, applyData, handleError) => {
      console.log(requestConfig)
      setIsLoading(true);

      try {
        const response = await fetch(requestConfig.url, {
          method: requestConfig.method ? requestConfig.method : 'GET',
          headers: requestConfig.headers ? requestConfig.headers : {},
          body: requestConfig.body ? JSON.stringify(requestConfig.body) : null,
        });

        console.log(response);

        const data = await response.json();
        console.log(data);

        if (!response.ok) {
          handleError(data.message || 'Something went wrong!');
          setIsLoading(false);
          return;
        }

        applyData(data);
      } catch (err) {
        if (err.message === 'Failed to fetch') {
          handleError(err.message);
        }
        console.log(err)
      }
      setIsLoading(false);
    },
    []
  );

  return {
    isLoading,
    sendRequest,
  };
};

export default useHttp;
