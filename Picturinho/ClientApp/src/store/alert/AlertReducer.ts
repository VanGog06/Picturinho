import { Alert } from './Alert';
import { AlertActions, AlertActionTypes } from './AlertActions';

export const initialState: Alert = {
  message: "",
  type: "success",
};

export const alertReducer = (
  state: Alert = initialState,
  action: AlertActions
): Alert => {
  switch (action.type) {
    case AlertActionTypes.SUCCESS:
      return {
        ...state,
        message: action.payload.message,
        type: "success",
      };
    case AlertActionTypes.ERROR:
      return {
        ...state,
        message: action.payload.message,
        type: "danger",
      };
    case AlertActionTypes.CLEAR:
      return {
        ...state,
        message: "",
      };
    default:
      return { ...state };
  }
};
