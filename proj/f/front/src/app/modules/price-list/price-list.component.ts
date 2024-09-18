import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-price-list',
  templateUrl: './price-list.component.html',
  styleUrls: ['./price-list.component.scss']
})
export class PriceListComponent implements OnInit {
  public priceList: any[] = [];

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
    this.apiService.get('api/DentalServiceGroup/GetAsPriceList')
      .toPromise()
      .then(th => {
        this.priceList = th;
      })
      .catch(error => { })
      .finally(() => { });
  }
  onSearch(value){
    this.apiService.get(`api/DentalServiceGroup/GetByDentalService?name=${value}`)
      .toPromise()
      .then(th => {
        this.priceList = th;
      })
      .catch(error => { })
      .finally(() => { });
  }
  onClear(){
    this.onSearch("");
  }
}
