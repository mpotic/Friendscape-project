import styles from './Button.module.css';

const Button = (props) => {
  return (
    <button
      className={`${styles.button} ${props.additionalStyles ?? ''}`}
      onClick={props.buttonHandler}
      type={props.buttonType ?? 'button'}
    >
      {props.children}
    </button>
  );
};

export default Button;
