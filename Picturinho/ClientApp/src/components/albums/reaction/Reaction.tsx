import { MDBIcon } from 'mdbreact';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import { Dispatch } from 'redux';

import { ReactionService } from '../../../services/ReactionService';
import { AlertActions, alertError } from '../../../store/alert/AlertActions';
import { IReactionProps } from './IReactionProps';
import styles from './Reaction.module.scss';

export const Reaction: React.FC<IReactionProps> = ({
  albumId,
}: IReactionProps): JSX.Element => {
  const dispatch = useDispatch<Dispatch<AlertActions>>();

  const [likesCount, setLikesCount] = useState<number>(0);

  useEffect(() => {
    (async () => {
      try {
        const newLikesCount: number = await ReactionService.getLikes(albumId);
        setLikesCount(newLikesCount);
      } catch (error) {
        dispatch(alertError(error.message));
      }
    })();
  }, []);

  const like = useCallback(async () => {
    try {
      const newLikesCount: number = await ReactionService.like(albumId);
      setLikesCount(newLikesCount);
    } catch (error) {
      dispatch(alertError(error.message));
    }
  }, [albumId, setLikesCount]);

  return (
    <div className={styles.reaction}>
      <MDBIcon
        className={`${styles.reaction__icon} ${styles.reaction__like}`}
        icon="thumbs-up"
        onClick={like}
      />
      {likesCount > 0 && (
        <span className={styles.reaction__count}>{likesCount}</span>
      )}
      <MDBIcon
        className={`${styles.reaction__icon} ${styles.reaction__love}`}
        icon="heart"
      />
    </div>
  );
};
