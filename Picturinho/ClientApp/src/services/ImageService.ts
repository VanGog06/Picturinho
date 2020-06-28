import { authHeader } from '../helpers/authHeader';
import { handleResponse } from '../helpers/handleResponse';
import { ImageModel } from '../models/image/ImageModel';

export class ImageService {
  public static async getImageById(imageId: number): Promise<ImageModel> {
    const requestOptions: RequestInit = {
      method: "GET",
    };

    const authenticationHeader = authHeader();
    if (authenticationHeader) {
      requestOptions.headers = authenticationHeader;
    }

    return fetch(`/api/images/${imageId}`, requestOptions).then(handleResponse);
  }

  public static async addImageToAlbum(
    albumId: number,
    image: File
  ): Promise<ImageModel> {
    const formData: FormData = new FormData();
    formData.append("albumId", albumId.toString());
    formData.append("image", image);

    const requestOptions: RequestInit = {
      method: "POST",
      body: formData,
    };

    const authenticationHeader = authHeader();
    if (authenticationHeader) {
      requestOptions.headers = authenticationHeader;
    }

    return fetch(`/api/images`, requestOptions).then(handleResponse);
  }
}
