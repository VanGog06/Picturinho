import React, { useCallback } from 'react';
import { Button } from 'react-bootstrap';
import { useDispatch } from 'react-redux';
import { Dispatch } from 'redux';

import { ImageService } from '../../../services/ImageService';
import { AlertActions, alertError } from '../../../store/alert/AlertActions';
import { IDeleteImageProps } from './IDeleteImageProps';

export const DeleteImage: React.FC<IDeleteImageProps> = ({
  selectedImages,
  handleDeletedImages,
}: IDeleteImageProps): JSX.Element => {
  const dispatch = useDispatch<Dispatch<AlertActions>>();

  const deleteImages = useCallback(async () => {
    const deleteImagesPromises: Promise<number>[] = [];

    try {
      for (const selectedImage of selectedImages) {
        deleteImagesPromises.push(ImageService.deleteImage(selectedImage.id));
      }

      const deletedImagesIds: number[] = await Promise.all(
        deleteImagesPromises
      );
      handleDeletedImages(deletedImagesIds);
    } catch (error) {
      dispatch(alertError(error.message));
    }
  }, [selectedImages]);

  return selectedImages.length > 0 ? (
    <Button onClick={deleteImages} variant="danger">
      Delete
    </Button>
  ) : (
    <></>
  );
};
