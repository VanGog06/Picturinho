import React from 'react';
import { Button } from 'react-bootstrap';

import { IDeleteImageProps } from './IDeleteImageProps';

export const DeleteImage: React.FC<IDeleteImageProps> = ({
  selectedImages,
}: IDeleteImageProps): JSX.Element => {
  return selectedImages.length > 0 ? (
    <Button variant="danger">Delete</Button>
  ) : (
    <></>
  );
};
