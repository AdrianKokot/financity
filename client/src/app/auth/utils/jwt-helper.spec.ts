import { JwtHelper } from './jwt-helper';
import { ClaimTypes } from '../data-access/models/claim-types';

describe('JwtHelper', () => {
  const exampleToken =
    'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImpvaG5kb2UiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJqb2huQGRvZS5tYWlsIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IkpvaG4gRG9lIiwibmJmIjoxNjc3NDExNjg3ODUxLCJleHAiOjE2Nzc0MTE2ODc4NTF9.90vt1hcrNFzu2Ago4r2hl2ehC6hMqgFzisg1x8vfVR0';

  it('should decode token and get all claims', () => {
    const result = JwtHelper.decode(exampleToken);

    expect(result).toEqual({
      nbf: 1677411687851,
      exp: 1677411687851,
      [ClaimTypes.email]: 'john@doe.mail',
      [ClaimTypes.id]: 'johndoe',
      [ClaimTypes.name]: 'John Doe',
    });
  });

  it('should return null on invalid token', () => {
    const result = JwtHelper.decode('');

    expect(result).toBeNull();
  });
});
