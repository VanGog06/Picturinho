import { UserActions, UserActionTypes } from './UserActions';

export const registrationReducer = (state = {}, action: UserActions) => {
  switch (action.type) {
    case UserActionTypes.REGISTER_REQUEST:
      return {
        ...state,
        registering: true,
      };
    case UserActionTypes.REGISTER_SUCCESS:
      return {
        ...state,
        registering: false,
      };
    case UserActionTypes.REGISTER_FAILURE:
      return {
        ...state,
        registering: false,
      };
    default:
      return { ...state };
  }
};
