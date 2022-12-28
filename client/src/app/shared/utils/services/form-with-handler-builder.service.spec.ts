import { TestBed } from '@angular/core/testing';

import { FormWithHandlerBuilderService } from './form-with-handler-builder.service';

describe('FormWithHandlerBuilderService', () => {
  let service: FormWithHandlerBuilderService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FormWithHandlerBuilderService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
