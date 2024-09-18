import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { InventoryIncomeItemModel, InventoryIncomeModel } from 'src/app/core/models/common';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-inventory-income-form',
  templateUrl: './inventory-income-form.component.html',
  styleUrls: ['./inventory-income-form.component.scss']
})
export class InventoryIncomeFormComponent implements OnInit {

  public income: InventoryIncomeModel;
  public inventories: InventoryIncomeItemModel[] = [];
  @Input() public isOpen: boolean;

  @Input() public set incomeItem(item: InventoryIncomeModel) {
    this.income = item;
    if (this.income) {
      this.apiService.get('api/IncomeInventoryItem/GetByIncome?incomeId=' + this.income.id)
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
