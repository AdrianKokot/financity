import { User } from './user';

const userClaims = {
  id: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier' as const,
  email:
    'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress' as const,
  name: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name' as const,
};

export const ClaimTypes: { [T in keyof User]: typeof userClaims[T] } =
  userClaims;
