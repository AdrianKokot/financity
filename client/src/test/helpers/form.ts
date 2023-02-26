import { DebugElement } from '@angular/core';

export const fillInput = (input: DebugElement, value: string) => {
  input.triggerEventHandler('input', { target: { value } });
};
