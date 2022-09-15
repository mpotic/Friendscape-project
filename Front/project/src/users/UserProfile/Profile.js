import UserFriends from './UserFriends';
import ProfileInfo from './ProfileInfo';
import ChangePassword from './ChangePassword';

import styles from './Profile.module.css';

const Profile = () => {
  return (
    <div className={styles.container}>
      <ProfileInfo />
      <ChangePassword />
      <UserFriends />
    </div>
  );
};

export default Profile;
