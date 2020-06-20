import { UserModel } from '../../models/user/UserModel';
import { UserActions, UserActionTypes } from './UserActions';

const initialState = {
  loading: false,
  items: [],
};

export const userReducer = (state = initialState, action: UserActions) => {
  switch (action.type) {
    case UserActionTypes.GETALL_REQUEST:
      return {
        ...state,
        loading: true,
      };
    case UserActionTypes.GETALL_SUCCESS:
      return {
        ...state,
        loading: false,
        items: action.users,
      };
    case UserActionTypes.GETALL_FAILURE:
      return {
        ...state,
      };
    case UserActionTypes.DELETE_REQUEST:
      return {
        ...state,
        items: state.items.map((user: UserModel) =>
          user.id === action.id ? { ...user, deleting: true } : user
        ),
      };
    case UserActionTypes.DELETE_SUCCESS:
      return {
        ...state,
        items: state.items.filter((user: UserModel) => user.id !== action.id),
      };
    case UserActionTypes.DELETE_FAILURE:
      return {
        ...state,
        items: state.items.map((user: UserModel) => {
          if (user.id === action.id) {
            return { ...user, deleting: false };
          }

          return user;
        }),
      };
    default:
      return { ...state };
  }
};
