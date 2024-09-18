import { Directive, Input, HostListener } from '@angular/core';
import { Table } from 'primeng/table';

@Directive({
    selector: '[pAddRow]'
})
export class AddRowDirective {
    @Input() table: Table;
    @Input() newRow: any;

    @HostListener('click', ['$event'])
    onClick(event: Event) {
        if (!this.table.value.some(s => s.id === this.newRow.id)) {
            // Insert a new row
            this.table.value.push(this.newRow);
            // Set the new row in edit mode
            this.table.initRowEdit(this.newRow);
        }
        event.preventDefault();
    }
}
