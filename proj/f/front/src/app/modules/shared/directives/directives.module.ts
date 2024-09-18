
import { NgModule } from '@angular/core';
import { AddRowDirective } from './add-row.directive';

const DIRECTIVES = [
    AddRowDirective
];

@NgModule({
    imports: [],
    declarations: [...DIRECTIVES],
    providers: [],
    exports: [...DIRECTIVES]
})
export class DirectivesModule {
}
