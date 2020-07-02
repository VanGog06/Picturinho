import { ImageModel } from '../../../models/image/ImageModel';

export interface IDeleteImageProps {
  selectedImages: ImageModel[];
  handleDeletedImages(deletedImagesIds: number[]): void;
}
