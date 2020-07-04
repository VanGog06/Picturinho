import { ApplicationRole } from '../enums/ApplicationRole';

export type UserModel = {
  id: number;
  username: string;
  firstName: string;
  lastName: string;
  token: string;
  deleting: boolean;
  role: ApplicationRole;
};
