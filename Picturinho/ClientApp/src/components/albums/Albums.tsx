import React, { Dispatch, useEffect, useState } from 'react';
import { Spinner } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';

import { AlbumModel } from '../../models/album/AlbumModel';
import { UserModel } from '../../models/user/UserModel';
import { AlbumService } from '../../services/AlbumService';
import { AlertActions, alertError } from '../../store/alert/AlertActions';
import { Album } from './album/Album';
import styles from './Albums.module.scss';

export const Albums: React.FC = (): JSX.Element => {
  const dispatch = useDispatch<Dispatch<AlertActions>>();
  const user: UserModel | undefined = useSelector(
    (state: any) => state.authentication?.user
  );

  const [showSpinner, setShowSpinner] = useState<boolean>(false);
  const [albums, setAlbums] = useState<AlbumModel[]>([]);

  useEffect(() => {
    (async () => {
      setShowSpinner(true);

      try {
        if (user) {
          const userAlbums: AlbumModel[] = await AlbumService.getUserAlbums(
            user.id
          );

          setAlbums(userAlbums);
          setShowSpinner(false);
        }
      } catch (err) {
        dispatch(alertError(err.message));
        setShowSpinner(false);
      }
    })();
  }, []);

  return (
    <>
      <h2 className={styles.title}>My albums</h2>
      {showSpinner ? (
        <Spinner animation="border" variant="dark" role="status">
          <span className="sr-only">Loading user albums...</span>
        </Spinner>
      ) : (
        <div className={styles.albums}>
          {albums.map((album: AlbumModel) => (
            <Album
              key={album.name}
              name={album.name}
              description={album.description}
            />
          ))}
        </div>
      )}
    </>
  );
};
