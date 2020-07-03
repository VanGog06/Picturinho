import { MDBIcon } from 'mdbreact';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import { Dispatch } from 'redux';

import { TotalReactionsModel } from '../../../models/reaction/TotalReactionsModel';
import { ReactionService } from '../../../services/ReactionService';
import { AlertActions, alertError } from '../../../store/alert/AlertActions';
import { IReactionProps } from './IReactionProps';
import styles from './Reaction.module.scss';

export const Reaction: React.FC<IReactionProps> = ({
  albumId,
}: IReactionProps): JSX.Element => {
  const dispatch = useDispatch<Dispatch<AlertActions>>();

  const [reactions, setReactions] = useState<TotalReactionsModel>({
    likes: 0,
    loves: 0,
  });

  useEffect(() => {
    (async () => {
      try {
        const newReactionsCount: TotalReactionsModel = await ReactionService.getReactions(
          albumId
        );
        setReactions(newReactionsCount);
      } catch (error) {
        dispatch(alertError(error.message));
      }
    })();
  }, []);

  const like = useCallback(async () => {
    try {
      const newReactionsCount: TotalReactionsModel = await ReactionService.like(
        albumId
      );
      setReactions(newReactionsCount);
    } catch (error) {
      dispatch(alertError(error.message));
    }
  }, [albumId, setReactions]);

  const love = useCallback(async () => {
    try {
      const newReactionsCount: TotalReactionsModel = await ReactionService.love(
        albumId
      );
      setReactions(newReactionsCount);
    } catch (error) {
      dispatch(alertError(error.message));
    }
  }, [albumId, setReactions]);

  return (
    <div className={styles.reaction}>
      <MDBIcon
        className={`${styles.reaction__icon} ${styles.reaction__like}`}
        icon="thumbs-up"
        onClick={like}
      />
      <span className={styles.reaction__count}>
        {reactions.likes > 0 ? reactions.likes : ""}
      </span>
      <MDBIcon
        className={`${styles.reaction__icon} ${styles.reaction__love}`}
        icon="heart"
        onClick={love}
      />
      <span className={styles.reaction__count}>
        {reactions.loves > 0 ? reactions.loves : ""}
      </span>
    </div>
  );
};
