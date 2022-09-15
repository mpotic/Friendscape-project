import { forwardRef, useImperativeHandle, useRef } from 'react';

import useInputValidation from '../hooks/validate-input';

import styles from './Input.module.css';

const Input = forwardRef((props, ref) => {
  const valueRef = useRef();

  const {
    hasError,
    errorMessage,
    onChangeHandler,
    onBlurHandler,
    onFocusHandler,
  } = useInputValidation(props.validate);

  useImperativeHandle(ref, () => {
    return {
      rawValue: valueRef.current.value,
      refValue: valueRef,
    };
  });

  return (
    <div>
      {hasError ? <div className={styles.error}>{errorMessage}</div> : ''}
      <div className={`${styles.container} ${props.additionalStylesDiv}`}>
        <label
          className={`${styles.label} ${props.additionalStylesLabel}`}
          htmlFor={props.id}
        >
          {props.label}
        </label>
        <input
          placeholder={props.placeholder}
          className={`${styles.field} ${props.additionalStylesInput}`}
          ref={valueRef}
          id={props.id}
          type={props.type}
          onChange={onChangeHandler}
          onBlur={onBlurHandler}
          onFocus={onFocusHandler}
        ></input>
      </div>
    </div>
  );
});

export default Input;
