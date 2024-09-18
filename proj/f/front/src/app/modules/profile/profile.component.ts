import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ApiService, UserService } from 'src/app/core/services';
import { Location } from '@angular/common';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  public submitted: boolean;
  public passwordSubmitted: boolean;
  public emailRegex = '^\\w+[\\w-\\.]*\\@\\w+((-\\w+)|(\\w*))\\.[a-z]{2,3}$';

  public pImage = 'assets/profile.png';

  public editForm = this.fb.group({
    userName: this.fb.control('', Validators.required),
    firstName: this.fb.control('', Validators.required),
    lastName: this.fb.control('', Validators.required),
    patronymic: this.fb.control('', Validators.required),
    phoneNumber: this.fb.control('', Validators.required),
    email: this.fb.control('', Validators.required),
    birthDate: this.fb.control(new Date(), Validators.required),
    profileImage: this.fb.control('')
  });

  public passwordForm = this.fb.group({
    currentPassword: this.fb.control('', Validators.required),
    newPassword: this.fb.control('', Validators.required),
    confirmNewPassword: this.fb.control('', Validators.required)
  });

  @ViewChild('fileUpload', { static: false }) fileUpload: ElementRef;

  constructor(private fb: FormBuilder,
    private apiService: ApiService,
    private location: Location,
    private userService: UserService,
    private messageService: MessageService) {

  }


  ngOnInit(): void {
    this.apiService.get('api/User/GetProfile').toPromise()
      .then(th => {
        const inputData = th;
        inputData.birthDate = new Date(th.birthDate);
        if (th.profileImage) {
          this.pImage = th.profileImage;
        }
        this.editForm.patchValue(th);
      })
      .catch(error => { })
      .finally(() => { });
  }

  changePassword() {
    this.passwordSubmitted = true;
    if (!this.passwordForm.valid) {
      return;
    }
    const model = this.passwordForm.value;
    this.apiService.put('api/User/ChangePassword', model).toPromise()
      .then(th => {
        //this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Password changes successfully. You will logouted know.', life: 3000 });
        this.userService.purgeAuth();
      })
      .catch(error => { })
      .finally(() => { });
  }

  save() {
    this.submitted = true;
    if (!this.editForm.valid) {
      return;
    }

    const model = this.editForm.value;
    model.birthDate = new Date(model.birthDate + 'Z');
    this.apiService.put('api/User/UpdateProfile', model).toPromise()
      .then(th => {
        //this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Profile updated', life: 3000 });
      })
      .catch(error => { })
      .finally(() => { });
  }

  goToBack() {
    this.location.back();
  }

  public openFileUpload() {
    this.fileUpload.nativeElement.click();
  }


  upload(files) {
    if (files.length > 0) {
      const formData = new FormData();

      for (const file of files) {
        formData.append('toothImages', file);
      }

      this.apiService.post('/api/User/UpdateAvatar', formData)
        .toPromise()
        .then(th => {
          console.log(th);
          this.pImage = th.profileImage;
        })
        .catch(error => {

        })
        .finally(() => {

        });
    }
  }

}
