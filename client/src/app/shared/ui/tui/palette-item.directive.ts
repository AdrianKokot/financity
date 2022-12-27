import { Directive, HostBinding, Input } from '@angular/core';

@Directive({
  selector: '[appPaletteItem]',
  standalone: true,
})
export class PaletteItemDirective {
  @Input() @HostBinding('attr.rounded') rounded = false;
  @Input() @HostBinding('attr.data-appearance-color') appPaletteItem:
    | string
    | null = '';
}
