import styles from './KeyValueTextBox.module.css';

const KeyValueTextBox = (props) => {
  return (
    <div className={styles.container}>
      <div className={`${styles.name} ${props.additionalStylesName}`}>
        {props.name}
      </div>
      <div className={`${styles.value} ${props.additionalStylesValue}`}>
        {props.value}
      </div>
    </div>
  );
};

export default KeyValueTextBox;
