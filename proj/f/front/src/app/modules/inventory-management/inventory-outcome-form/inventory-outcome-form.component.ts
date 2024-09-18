import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { InventoryOutcomeItemModel, InventoryOutcomeModel } from 'src/app/core/models/common';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-inventory-outcome-form',
  templateUrl: './inventory-outcome-form.component.html',
  styleUrls: ['./inventory-outcome-form.component.scss']
})
export class InventoryOutcomeFormComponent implements OnInit {


  public outcome: InventoryOutcomeModel;
  public inventories: InventoryOutcomeItemModel[] = [];
  @Input() public isOpen: boolean;

  @Input() public set outcomeItem(item: InventoryOutcomeModel) {
    this.outcome = item;
    if (this.outcome) {
      this.apiService.get('api/OutcomeInventoryItem/GetByOutCome?outcomeId=' + this.outcome.id)
        .toPromise().then(th => {
          this.inventories = th;
        })
        .catch(error => { })
        .finally(() => { });
    }
  }

  @Output() public closed: EventEmitter<any> = new EventEmitter<any>();

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
  }

  public close() {
    this.closed.emit();
  }

}
