export enum AlertActionTypes {
  SUCCESS = "ALERT_SUCCESS",
  ERROR = "ALERT_ERROR",
  CLEAR = "ALERT_CLEAR",
}

export interface SuccessAction {
  type: AlertActionTypes.SUCCESS;
  payload: { message: string };
}

export const alertSuccess = (message: string): SuccessAction => {
  return {
    type: AlertActionTypes.SUCCESS,
    payload: { message },
  };
};

export interface ErrorAction {
  type: AlertActionTypes.ERROR;
  payload: { message: string };
}

export const alertError = (message: string): ErrorAction => {
  return {
    type: AlertActionTypes.ERROR,
    payload: { message },
  };
};

export interface ClearAction {
  type: AlertActionTypes.CLEAR;
}

export const alertClear = (): ClearAction => {
  return {
    type: AlertActionTypes.CLEAR,
  };
};

export type AlertActions = SuccessAction | ErrorAction | ClearAction;
