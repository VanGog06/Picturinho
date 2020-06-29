import { uniqueId } from 'lodash';
import { MDBIcon } from 'mdbreact';
import React, { useCallback, useEffect, useState } from 'react';
import { ProgressBar, Spinner } from 'react-bootstrap';
import { useDispatch } from 'react-redux';
import { useParams } from 'react-router-dom';
import { Dispatch } from 'redux';

import { AlbumModel } from '../../../models/album/AlbumModel';
import { AlbumWithImagesModel } from '../../../models/album/AlbumWithImagesModel';
import { ImageModel } from '../../../models/image/ImageModel';
import { AlbumService } from '../../../services/AlbumService';
import { ImageService } from '../../../services/ImageService';
import { AlertActions, alertError } from '../../../store/alert/AlertActions';
import { Reaction } from '../reaction/Reaction';
import { Upload } from '../upload/Upload';
import styles from './AlbumDetails.module.scss';

export const AlbumDetails: React.FC = (): JSX.Element => {
  const { id } = useParams();
  const dispatch = useDispatch<Dispatch<AlertActions>>();

  const [showSpinner, setShowSpinner] = useState<boolean>(false);
  const [album, setAlbum] = useState<AlbumModel>();
  const [images, setImages] = useState<ImageModel[]>([]);
  const [selectedImages, setSelectedImages] = useState<string[]>([]);

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
    async (acceptedFiles: File[]): Promise<void> => {
      for (const file of acceptedFiles) {
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

        setImages((prevImages: ImageModel[]) => {
          return [
            ...prevImages.filter((pi) => pi.uniqueId !== uniqId),
            {
              ...uploadedImage,
              isUploading: false,
              uniqueId: uniqId,
              data: `data:image/png;base64,${uploadedImage.data}`,
            },
          ];
        });
      }
    },
    [id, setImages]
  );

  const addSelectedImage = useCallback(
    (imageUniqueId: string): void => {
      setSelectedImages((prevSelectedImages: string[]) => [
        ...prevSelectedImages,
        imageUniqueId,
      ]);
    },
    [setSelectedImages]
  );

  const removeSelectedImage = useCallback(
    (imageUniqueId: string): void => {
      const newSelectedImages: string[] = selectedImages.filter(
        (si) => si !== imageUniqueId
      );
      setSelectedImages(newSelectedImages);
    },
    [selectedImages, setSelectedImages]
  );

  return showSpinner && !album ? (
    <Spinner animation="border" variant="dark" role="status">
      <span className="sr-only">Loading album with id = {id}</span>
    </Spinner>
  ) : (
    <div className={styles.details}>
      <h2 className={styles.details__title}>{album?.name}</h2>

      <div className={styles.details__images}>
        {images.map((i) => (
          <div key={i.uniqueId} className={styles.details__images__container}>
            <img
              className={styles.details__images__container__image}
              src={i.data}
              alt={i.name}
              onClick={() => addSelectedImage(i.uniqueId)}
            />

            {selectedImages.indexOf(i.uniqueId) !== -1 && (
              <div
                onClick={() => removeSelectedImage(i.uniqueId)}
                className={styles.selectedImage}
              >
                <MDBIcon icon="check" />
              </div>
            )}

            {i.isUploading && <ProgressBar animated now={100} />}
          </div>
        ))}
      </div>

      <Reaction />
      <Upload onDrop={onDrop} />
    </div>
  );
};
