import React from 'react';
import { Button } from 'react-bootstrap';

import { IDeleteAlbumProps } from './IDeleteAlbumProps';

export const DeleteAlbum: React.FC<IDeleteAlbumProps> = ({
  selectedImages,
}: IDeleteAlbumProps): JSX.Element => {
  return selectedImages.length > 0 ? (
    <Button variant="danger">Delete</Button>
  ) : (
    <></>
  );
};
