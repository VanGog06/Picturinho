import { MDBIcon } from 'mdbreact';
import React from 'react';

import styles from './Reaction.module.scss';

export const Reaction: React.FC = (): JSX.Element => {
  return (
    <div className={styles.reaction}>
      <MDBIcon
        className={`${styles.reaction__icon} ${styles.reaction__like}`}
        icon="thumbs-up"
      />
      <MDBIcon
        className={`${styles.reaction__icon} ${styles.reaction__love}`}
        icon="heart"
      />
    </div>
  );
};
