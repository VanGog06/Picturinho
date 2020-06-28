import { uniqueId } from 'lodash';
import React, { useCallback, useEffect, useState } from 'react';
import { Spinner } from 'react-bootstrap';
import { useDropzone } from 'react-dropzone';
import { useDispatch } from 'react-redux';
import { useParams } from 'react-router-dom';
import { Dispatch } from 'redux';

import { AlbumModel } from '../../../models/album/AlbumModel';
import { AlbumWithImagesModel } from '../../../models/album/AlbumWithImagesModel';
import { ImageModel } from '../../../models/image/ImageModel';
import { AlbumService } from '../../../services/AlbumService';
import { ImageService } from '../../../services/ImageService';
import { AlertActions, alertError } from '../../../store/alert/AlertActions';
import styles from './AlbumDetails.module.scss';

export const AlbumDetails: React.FC = (): JSX.Element => {
  const { id } = useParams();
  const dispatch = useDispatch<Dispatch<AlertActions>>();

  const [showSpinner, setShowSpinner] = useState<boolean>(false);
  const [album, setAlbum] = useState<AlbumModel>();
  const [images, setImages] = useState<ImageModel[]>([]);

  useEffect(() => {
    (async () => {
      setShowSpinner(true);

      try {
        const retrievedAlbum: AlbumWithImagesModel = await AlbumService.getAlbumById(
          +id
        );

        const imagePromises: Promise<ImageModel>[] = [];
        for (const imageId of retrievedAlbum.imageIds) {
          const imagePromise: Promise<ImageModel> = ImageService.getImageById(
            imageId
          );
          imagePromises.push(imagePromise);
        }

        const alreadyAvailalbeImages: ImageModel[] = await Promise.all(
          imagePromises
        );
        const imagesForState: ImageModel[] = alreadyAvailalbeImages.map((i) => {
          return {
            ...i,
            isUploading: false,
            uniqueId: uniqueId(),
            data: `data:image/png;base64,${i.data}`,
          };
        });

        setAlbum(retrievedAlbum);
        setImages(imagesForState);
        setShowSpinner(false);
      } catch (error) {
        dispatch(alertError(error.message));
        setShowSpinner(false);
      }
    })();
  }, [dispatch, id]);

  const onDrop = useCallback(
    (acceptedFiles: File[]): void => {
      acceptedFiles.forEach(async (file: File) => {
        const uniqId: string = uniqueId();

        setImages((prevImages: ImageModel[]) => [
          ...prevImages,
          {
            id: 0,
            isUploading: true,
            data: URL.createObjectURL(file),
            name: file.name,
            uniqueId: uniqId,
          },
        ]);

        const uploadedImage: ImageModel = await ImageService.addImageToAlbum(
          id,
          file
        );

        const imagesExceptUploaded: ImageModel[] = images.filter(
          (i) => i.uniqueId !== uniqId
        );

        setImages([
          ...imagesExceptUploaded,
          {
            ...uploadedImage,
            isUploading: false,
            uniqueId: uniqId,
            data: `data:image/png;base64,${uploadedImage.data}`,
          },
        ]);
      });
    },
    [images, id, setImages]
  );

  const { acceptedFiles, getRootProps, getInputProps } = useDropzone({
    onDrop,
  });

  return showSpinner && !album ? (
    <Spinner animation="border" variant="dark" role="status">
      <span className="sr-only">Loading album with id = {id}</span>
    </Spinner>
  ) : (
    <div className={styles.details}>
      <h2 className={styles.details__title}>{album?.name}</h2>

      <div className={styles.details__images}>
        {images.map((i) => (
          <img
            className={styles.details__images__image}
            key={i.uniqueId}
            src={i.data}
            alt={i.name}
          />
        ))}
      </div>

      <section className="container">
        <div {...getRootProps({ className: styles.details__dropzone })}>
          <input {...getInputProps()} />
          <p>Drag and drop image(s) here, or click to select some</p>
        </div>
      </section>
    </div>
  );
};
