import { History } from 'history';
import { ThunkAction, ThunkDispatch } from 'redux-thunk';

import { RegisterUserModel } from '../../models/user/RegisterUserModel';
import { UserModel } from '../../models/user/UserModel';
import { UserService } from '../../services/UserService';
import { AlertActions, alertError, alertSuccess } from '../alert/AlertActions';

export enum UserActionTypes {
  REGISTER_REQUEST = "USERS_REGISTER_REQUEST",
  REGISTER_SUCCESS = "USERS_REGISTER_SUCCESS",
  REGISTER_FAILURE = "USERS_REGISTER_FAILURE",
  LOGIN_REQUEST = "USERS_LOGIN_REQUEST",
  LOGIN_SUCCESS = "USERS_LOGIN_SUCCESS",
  LOGIN_FAILURE = "USERS_LOGIN_FAILURE",
  LOGOUT = "USERS_LOGOUT",
  DELETE_REQUEST = "USERS_DELETE_REQUEST",
  DELETE_SUCCESS = "USERS_DELETE_SUCCESS",
  DELETE_FAILURE = "USERS_DELETE_FAILURE",
  GETALL_REQUEST = "USERS_GETALL_REQUEST",
  GETALL_SUCCESS = "USERS_GETALL_SUCCESS",
  GETALL_FAILURE = "USERS_GETALL_FAILURE",
}

export type RegisterRequestAction = {
  type: UserActionTypes.REGISTER_REQUEST;
};

export type RegisterSuccessAction = {
  type: UserActionTypes.REGISTER_SUCCESS;
};

export type RegisterFailureActon = {
  type: UserActionTypes.REGISTER_FAILURE;
};

export type LoginRequestAction = {
  type: UserActionTypes.LOGIN_REQUEST;
};

export type LoginSuccessAction = {
  type: UserActionTypes.LOGIN_SUCCESS;
  user: UserModel;
};

export type LoginFailureAction = {
  type: UserActionTypes.LOGIN_FAILURE;
};

export type LogoutAction = {
  type: UserActionTypes.LOGOUT;
};

export type DeleteRequestAction = {
  type: UserActionTypes.DELETE_REQUEST;
  id: number;
};

export type DeleteSuccessAction = {
  type: UserActionTypes.DELETE_SUCCESS;
  id: number;
};

export type DeleteFailureAction = {
  type: UserActionTypes.DELETE_FAILURE;
  id: number;
};

export type UsersGetAllRequestAction = {
  type: UserActionTypes.GETALL_REQUEST;
};

export type UsersGetAllSuccessAction = {
  type: UserActionTypes.GETALL_SUCCESS;
  users: UserModel[];
};

export type UsersGetAllFailureAction = {
  type: UserActionTypes.GETALL_FAILURE;
};

export type UserActions =
  | RegisterRequestAction
  | RegisterSuccessAction
  | RegisterFailureActon
  | LoginRequestAction
  | LoginSuccessAction
  | LoginFailureAction
  | LogoutAction
  | DeleteRequestAction
  | DeleteSuccessAction
  | DeleteFailureAction
  | UsersGetAllRequestAction
  | UsersGetAllSuccessAction
  | UsersGetAllFailureAction;

type ThunkUsersResult<Result> = ThunkAction<
  Result,
  UserModel,
  undefined,
  UserActions
>;

export const login = (
  username: string,
  password: string,
  history: History
): ThunkUsersResult<Promise<void>> => {
  const request = (): LoginRequestAction => {
    return { type: UserActionTypes.LOGIN_REQUEST };
  };
  const success = (user: UserModel): LoginSuccessAction => {
    return { type: UserActionTypes.LOGIN_SUCCESS, user };
  };
  const failure = (): LoginFailureAction => {
    return { type: UserActionTypes.LOGIN_FAILURE };
  };

  return async (
    dispatch: ThunkDispatch<UserModel, boolean, UserActions | AlertActions>
  ): Promise<void> => {
    dispatch(request());

    try {
      const user: UserModel = await UserService.login(username, password);

      dispatch(success(user));
      history.push("/");
    } catch (error) {
      dispatch(failure());
      dispatch(alertError(error.message));
    }
  };
};

export const register = (
  user: RegisterUserModel,
  history: History
): ThunkUsersResult<Promise<void>> => {
  const request = (): RegisterRequestAction => {
    return { type: UserActionTypes.REGISTER_REQUEST };
  };
  const success = (): RegisterSuccessAction => {
    return { type: UserActionTypes.REGISTER_SUCCESS };
  };
  const failure = (): RegisterFailureActon => {
    return { type: UserActionTypes.REGISTER_FAILURE };
  };

  return async (
    dispatch: ThunkDispatch<UserModel, undefined, UserActions | AlertActions>
  ): Promise<void> => {
    dispatch(request());

    try {
      await UserService.register(user);

      dispatch(success());
      history.push("/login");
      dispatch(alertSuccess("User registered successfully"));
    } catch (error) {
      dispatch(failure());
      dispatch(alertError(error.message));
    }
  };
};

export const logout = (): LogoutAction => {
  UserService.logout();
  return { type: UserActionTypes.LOGOUT };
};

export const getAll = (): ThunkUsersResult<Promise<void>> => {
  const request = (): UsersGetAllRequestAction => {
    return { type: UserActionTypes.GETALL_REQUEST };
  };
  const success = (users: UserModel[]): UsersGetAllSuccessAction => {
    return { type: UserActionTypes.GETALL_SUCCESS, users };
  };
  const failure = (): UsersGetAllFailureAction => {
    return { type: UserActionTypes.GETALL_FAILURE };
  };

  return async (
    dispatch: ThunkDispatch<UserModel, null, UserActions | AlertActions>
  ): Promise<void> => {
    dispatch(request());

    try {
      const users: UserModel[] = await UserService.getAll();

      dispatch(success(users));
    } catch (error) {
      dispatch(failure());
      dispatch(alertError(error.message));
    }
  };
};

export const deleteUser = (id: number): ThunkUsersResult<Promise<void>> => {
  const request = (id: number): DeleteRequestAction => {
    return { type: UserActionTypes.DELETE_REQUEST, id };
  };
  const success = (id: number): DeleteSuccessAction => {
    return { type: UserActionTypes.DELETE_SUCCESS, id };
  };
  const failure = (id: number): DeleteFailureAction => {
    return { type: UserActionTypes.DELETE_FAILURE, id };
  };

  return async (
    dispatch: ThunkDispatch<UserModel, undefined, UserActions | AlertActions>
  ): Promise<void> => {
    dispatch(request(id));

    try {
      await UserService.deleteUser(id);

      dispatch(success(id));
    } catch (error) {
      dispatch(failure(id));
      dispatch(alertError(error.message));
    }
  };
};
