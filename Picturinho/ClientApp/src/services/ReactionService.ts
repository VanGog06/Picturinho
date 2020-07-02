import { authHeader } from '../helpers/authHeader';
import { handleResponse } from '../helpers/handleResponse';

export class ReactionService {
  public static async getLikes(albumId: number): Promise<number> {
    const requestOptions: RequestInit = {
      method: "GET",
    };

    const authenticationHeader = authHeader();
    if (authenticationHeader) {
      requestOptions.headers = authenticationHeader;
    }

    return fetch(`/api/reactions/likes/${albumId}`, requestOptions).then(
      handleResponse
    );
  }

  public static async like(albumId: number): Promise<number> {
    const requestOptions: RequestInit = {
      method: "POST",
    };

    const authenticationHeader = authHeader();
    if (authenticationHeader) {
      requestOptions.headers = authenticationHeader;
    }

    return fetch(`/api/reactions/like/${albumId}`, requestOptions).then(
      handleResponse
    );
  }
}
