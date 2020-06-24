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
}