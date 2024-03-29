import { JwtToken } from '../data-access/models/token';

export class JwtHelper {
  static decode(jwt: string): JwtToken | null {
    try {
      return JSON.parse(
        decodeURIComponent(
          window
            .atob(jwt.split('.')[1].replace(/-/g, '+').replace(/_/g, '/'))
            .split('')
            .map(c => `%${`00${c.charCodeAt(0).toString(16)}`.slice(-2)}`)
            .join('')
        )
      );
    } catch (e) {
      return null;
    }
  }
}
