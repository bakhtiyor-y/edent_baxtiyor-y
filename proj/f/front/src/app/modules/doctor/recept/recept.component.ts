import { HttpParams } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Dialog } from 'primeng/dialog';
import { ReceptModel } from 'src/app/core/models/common';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-recept',
  templateUrl: './recept.component.html',
  styleUrls: ['./recept.component.scss']
})
export class ReceptComponent implements OnInit {

  public viewDialog: boolean;
  public setTechnicDialog: boolean;
  public selectedItemId: number;

  public items: ReceptModel[];
  public viewItem: ReceptModel;

  public loading: boolean;
  public totalRecords: number;

  public isSearchClear = false;
  public lastTableLazyLoadEvent;

  @ViewChild('receptDialog') dialog: Dialog;


  constructor(private apiService: ApiService) {

  }

  ngOnInit(): void {
    this.loading = false;
  }

  public loadItems(event, name = '') {
    this.lastTableLazyLoadEvent = event;
    let params = new HttpParams()
      .set('filter', JSON.stringify(event));

    if (name) {
      params = new HttpParams()
        .set('filter', JSON.stringify(event))
        .set('name', name);
    }
    this.apiService.get('api/Recept/GetByDoctor', params).toPromise().then(th => {
      this.items = th.data;
      this.totalRecords = th.total;
    }).catch(error => {
    }).finally(() => {
      this.loading = false;
    });
  }

  public onPageChange(event) {
    this.loading = true;
  }

  public viewDetails(item: ReceptModel) {
    this.apiService.get('api/Recept/GetById/' + item.id)
      .toPromise()
      .then(data => {
        this.viewDialog = true;
        this.viewItem = data;
      })
      .catch(error => { })
      .finally(() => { });
  }

  public close() {
    this.viewDialog = false;
    this.viewItem = null;
  }

  public onSearch(value) {
    if (value && value.length > 3) {
      this.isSearchClear = true;
      this.loadItems(this.lastTableLazyLoadEvent, value);

    } else {
      if (this.isSearchClear) {
        this.isSearchClear = false;
        this.loadItems(this.lastTableLazyLoadEvent, '');
      }
    }
  }

  public setTechnic(item) {
    this.selectedItemId = item.id;
    this.setTechnicDialog = true;
  }

  public onTechnicSet() {
    this.setTechnicDialog = false;
    this.selectedItemId = 0;
  }

  public onCancelTechnicSet() {
    this.setTechnicDialog = false;
    this.selectedItemId = 0;
  }

}
