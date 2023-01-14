export interface User {
  id: string;
  email: string;
  name: string;
}

interface UserPassword {
  password: string;
}

export interface RegisterPayload
  extends Pick<User, 'email' | 'name'>,
    UserPassword {}

export interface LoginPayload extends Pick<User, 'email'>, UserPassword {}

export interface ResetPasswordPayload extends LoginPayload {
  token: string;
}

export interface ChangePasswordPayload extends UserPassword {
  newPassword: string;
}
