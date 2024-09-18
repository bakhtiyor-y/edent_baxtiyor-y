import { Component, OnInit } from '@angular/core';
import { AppComponent } from '../../app.component';
import { ApiService } from '../services';

@Component({
  selector: 'app-footer',
  templateUrl: './app.footer.component.html'
})
export class AppFooterComponent implements OnInit {
  organizationName: string = "Edent Medical";
  constructor(public app: AppComponent, private apiService: ApiService) {
  }
  ngOnInit(): void {
    const orgInfo = JSON.parse(localStorage.getItem('orgInfo'));
    if (!orgInfo) {
      this.apiService.get('api/Organization')
        .toPromise()
        .then(o => {
          this.organizationName = o.name;
          localStorage.setItem('orgInfo', JSON.stringify(o));
        });
    } else {
      this.organizationName = orgInfo.name;
    }
  }

}
