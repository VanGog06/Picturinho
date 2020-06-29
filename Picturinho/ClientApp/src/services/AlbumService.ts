import { authHeader } from '../helpers/authHeader';
import { handleResponse } from '../helpers/handleResponse';
import { AlbumModel } from '../models/album/AlbumModel';
import { AlbumWithImagesModel } from '../models/album/AlbumWithImagesModel';

export class AlbumService {
  public static async getAlbums(): Promise<AlbumModel[]> {
    const requestOptions: RequestInit = {
      method: "GET",
    };

    const authenticationHeader = authHeader();
    if (authenticationHeader) {
      requestOptions.headers = authenticationHeader;
    }

    return fetch(`/api/albums`, requestOptions).then(handleResponse);
  }

  public static async getAlbumById(
    albumId: number
  ): Promise<AlbumWithImagesModel> {
    const requestOptions: RequestInit = {
      method: "GET",
    };

    const authenticationHeader = authHeader();
    if (authenticationHeader) {
      requestOptions.headers = authenticationHeader;
    }

    return fetch(`/api/albums/${albumId}`, requestOptions).then(handleResponse);
  }

  public static async getUserAlbums(userId: number): Promise<AlbumModel[]> {
    const requestOptions: RequestInit = {
      method: "GET",
    };

    const authenticationHeader = authHeader();
    if (authenticationHeader) {
      requestOptions.headers = authenticationHeader;
    }

    return fetch(`/api/albums/user/${userId}`, requestOptions).then(
      handleResponse
    );
  }

  public static async createAlbum(model: AlbumModel): Promise<AlbumModel> {
    const requestOptions: RequestInit = {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(model),
    };

    const authenticationHeader = authHeader();
    if (authenticationHeader) {
      requestOptions.headers = {
        ...requestOptions.headers,
        ...authenticationHeader,
      };
    }

    return fetch("/api/albums", requestOptions).then(handleResponse);
  }
}
