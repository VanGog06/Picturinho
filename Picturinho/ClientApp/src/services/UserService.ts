import { authHeader } from '../helpers/authHeader';
import { handleResponse } from '../helpers/handleResponse';
import { RegisterUserModel } from '../models/user/RegisterUserModel';
import { UserModel } from '../models/user/UserModel';

export class UserService {
  public static async login(
    username: string,
    password: string
  ): Promise<UserModel> {
    const requestOptions: RequestInit = {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ username, password }),
    };

    return fetch("/users/authenticate", requestOptions)
      .then(handleResponse)
      .then((user) => {
        // store user details and jwt token in local storage to keep user logged in between page refreshes
        localStorage.setItem("user", JSON.stringify(user));

        return user;
      });
  }

  public static async register(user: RegisterUserModel): Promise<void> {
    const requestOptions: RequestInit = {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(user),
    };

    return fetch("/users/register", requestOptions).then(handleResponse);
  }

  public static logout(): void {
    localStorage.removeItem("user");
  }

  public static async getAll(): Promise<UserModel[]> {
    const requestOptions: RequestInit = {
      method: "GET",
    };

    const authenticationHeader = authHeader();
    if (authenticationHeader) {
      requestOptions.headers = authenticationHeader;
    }

    return fetch("/users", requestOptions).then(handleResponse);
  }

  public static async deleteUser(id: number): Promise<void> {
    const requestOptions: RequestInit = {
      method: "DELETE",
    };

    const authenticationHeader = authHeader();
    if (authenticationHeader) {
      requestOptions.headers = authenticationHeader;
    }

    return fetch(`/users/${id}`, requestOptions).then(handleResponse);
  }
}
