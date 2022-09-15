import styles from './Header.module.css';

const Header = (props) => {
  return <div className={`${styles.header} ${styles.additionalStyles ?? ''}`}>{props.children}</div>;
};

export default Header;
