import { useReducer } from 'react';

const initialState = {
  value: '',
  hasError: false,
  errorMessage: '',
};

const validationReducer = (state, action) => {
  if (action.type === 'INPUT') {
    return { ...state, value: action.value, hasError: false };
  } else if (action.type === 'BLUR') {
    const { isValid, errorMessage } = action.validate(state.value);
    return { ...state, hasError: !isValid, errorMessage };
  } else if (action.type === 'FOCUS') {
    return { ...state, hasError: false, errorMessage: '' };
  } else if (action.type === 'RESET') {
  }
};

const useInputValidation = (validate) => {
  const [inputState, inputValidation] = useReducer(
    validationReducer,
    initialState
  );

  //if validate function is not defined the input field  will always be interpreted as having a valid state and no errorMessage will be shown
  if (!validate) {
    validate = () => {
      return { isValid: true, errorMessage: '' };
    };
  }

  const onChangeHandler = (event) => {
    inputValidation({ type: 'INPUT', value: event.target.value });
  };

  const onBlurHandler = (event) => {
    inputValidation({ type: 'BLUR', validate });
  };

  const onFocusHandler = (event) => {
    inputValidation({ type: 'FOCUS' });
  };

  return {
    value : inputState.value,
    hasError: inputState.hasError,
    errorMessage: inputState.errorMessage,
    onChangeHandler,
    onBlurHandler,
    onFocusHandler,
  };
};

export default useInputValidation;
