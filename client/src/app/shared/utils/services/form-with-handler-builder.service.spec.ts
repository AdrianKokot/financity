import { TestBed } from '@angular/core/testing';

import { FormWithHandlerBuilder } from './form-with-handler-builder.service';

describe('FormWithHandlerBuilderService', () => {
  let service: FormWithHandlerBuilder;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FormWithHandlerBuilder);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
