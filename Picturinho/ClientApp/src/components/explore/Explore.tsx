import React, { useCallback, useEffect, useState } from 'react';
import { Spinner } from 'react-bootstrap';
import { useDispatch } from 'react-redux';
import { Dispatch } from 'redux';

import { AlbumModel } from '../../models/album/AlbumModel';
import { AlbumService } from '../../services/AlbumService';
import { AlertActions, alertError } from '../../store/alert/AlertActions';
import { Album } from '../albums/album/Album';
import styles from './Explore.module.scss';

export const Explore: React.FC = (): JSX.Element => {
  const dispatch = useDispatch<Dispatch<AlertActions>>();

  const [showSpinner, setShowSpinner] = useState<boolean>(false);
  const [albums, setAlbums] = useState<AlbumModel[]>([]);

  useEffect(() => {
    (async () => {
      setShowSpinner(true);

      try {
        const albums: AlbumModel[] = await AlbumService.getAlbums();

        setAlbums(albums);
        setShowSpinner(false);
      } catch (err) {
        dispatch(alertError(err.message));
        setShowSpinner(false);
      }
    })();
  }, [dispatch]);

  const handleRemoveAlbum = useCallback(
    (albumId: number) => {
      setAlbums((prevAlbums: AlbumModel[]) =>
        prevAlbums.filter((a) => a.id !== albumId)
      );
    },
    [setAlbums]
  );

  return (
    <div>
      <h2 className={styles.title}>Explore</h2>
      {showSpinner ? (
        <Spinner animation="border" variant="dark" role="status">
          <span className="sr-only">Loading user albums...</span>
        </Spinner>
      ) : (
        <div className={styles.albums}>
          {albums.map((album: AlbumModel) => (
            <Album
              key={album.name}
              album={album}
              handleRemoveAlbum={handleRemoveAlbum}
            />
          ))}
        </div>
      )}
    </div>
  );
};
