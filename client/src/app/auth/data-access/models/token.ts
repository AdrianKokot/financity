import { ClaimTypes } from './claim-types';

export interface JwtToken
  extends Record<typeof ClaimTypes[keyof typeof ClaimTypes], string> {
  /**
   * @description Unix Timestamp
   */
  nbf: number;
  /**
   * @description Unix Timestamp
   */
  exp: number;
}
