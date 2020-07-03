import { authHeader } from '../helpers/authHeader';
import { handleResponse } from '../helpers/handleResponse';
import { TotalReactionsModel } from '../models/reaction/TotalReactionsModel';

export class ReactionService {
  public static async getReactions(
    albumId: number
  ): Promise<TotalReactionsModel> {
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

  public static async like(albumId: number): Promise<TotalReactionsModel> {
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

  public static async love(albumId: number): Promise<TotalReactionsModel> {
    const requestOptions: RequestInit = {
      method: "POST",
    };

    const authenticationHeader = authHeader();
    if (authenticationHeader) {
      requestOptions.headers = authenticationHeader;
    }

    return fetch(`/api/reactions/love/${albumId}`, requestOptions).then(
      handleResponse
    );
  }
}
