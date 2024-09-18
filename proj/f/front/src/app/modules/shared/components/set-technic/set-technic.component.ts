import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { TechnicModel } from 'src/app/core/models/user-management';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-set-technic',
  templateUrl: './set-technic.component.html',
  styleUrls: ['./set-technic.component.scss']
})
export class SetTechnicComponent implements OnInit {

  public selectedTechnic: TechnicModel;
  public technicShare: number;
  public technics: TechnicModel[] = [];

  @Input() public receptId: number;
  @Input() public isOpen: boolean;

  @Output() public closed: EventEmitter<any> = new EventEmitter<any>();
  @Output() public saved: EventEmitter<any> = new EventEmitter<any>();

  constructor(private fb: FormBuilder,
    private apiService: ApiService,
    private router: Router,
    private messageService: MessageService,
    private translate: TranslateService) { }

  ngOnInit(): void {
  }

  public searchTechnic(event) {
    this.apiService.get('api/Technic/SearchByName?name=' + event.query)
      .toPromise()
      .then(th => {
        this.technics = th;
      })
      .catch(error => { })
      .finally(() => { });
  }

  public onTechnicClear() {
    this.selectedTechnic = null;
  }

  public save() {

    if (!this.receptId || !this.selectedTechnic?.id || !this.technicShare) {
      this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_UPDATE'), life: 3000 });
      return;
    }

    const data = { id: this.receptId, technicId: this.selectedTechnic.id, technicShare: this.technicShare };
    this.apiService.put('api/Recept/SetTechnic', data)
      .toPromise().then(th => {
        if (th.result) {
          this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_UPDATED'), life: 3000 });
          this.saved.emit();
        } else {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_UPDATE'), life: 3000 });
        }
      })
      .catch(error => {
        this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_UPDATE'), life: 3000 });
      })
      .finally(() => { this.clearForm(); });

  }

  public cancel() {
    this.closed.emit();
    this.clearForm();
  }

  clearForm() {
    this.selectedTechnic = null;
    this.technicShare = undefined;
  }

}
