import { TestBed } from '@angular/core/testing';

import { ModelFormBuilder } from './model-form.builder';

describe('ModelFormBuilder', () => {
  let service: ModelFormBuilder;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ModelFormBuilder);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
