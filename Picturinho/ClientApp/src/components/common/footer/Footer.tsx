import React from 'react';

import styles from './Footer.module.scss';

export const Footer: React.FC = (): JSX.Element => (
  <div className={styles.footer}>
    Made with <span className={styles.footer__heart}>❤</span> by{" "}
    <a
      className={styles.footer__link}
      href="https://github.com/VanGog06"
      target="_blank"
      rel="noopener noreferrer"
    >
      Georgi Stankov
    </a>
  </div>
);
