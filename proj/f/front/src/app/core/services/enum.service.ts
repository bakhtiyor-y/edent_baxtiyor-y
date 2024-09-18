import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { AppointmentStatus, DentalServiceType, DiscountType, Gender, PatientAgeType, PaymentType, ProfitType, TermType, ToothState } from '../enums';

@Injectable({
    providedIn: 'root'
})
export class EnumService {

    constructor(private translate: TranslateService) {

    }

    public getTermTypes() {

        return [
            { value: TermType.Fixed, text: this.translate.instant('FIXED') },
            { value: TermType.Percent, text: this.translate.instant('PERCENT') },
            { value: TermType.Rent, text: this.translate.instant('RENT') }
        ];
    }

    public getTermTypeText(type: TermType) {

        let text = '';
        switch (type) {
            case TermType.Fixed:
                text = this.translate.instant('FIXED');
                break;
            case TermType.Percent:
                text = this.translate.instant('PERCENT');
                break;
            case TermType.Rent:
                text = this.translate.instant('RENT');
                break;
            default:
                text = '';
                break;
        }
        return text;
    }

    public getProfitTypes() {

        return [
            { value: ProfitType.Fixed, text: this.translate.instant('FIXED') },
            { value: ProfitType.Percent, text: this.translate.instant('PERCENT') }
        ];
    }

    public getDiscountTypes() {

        return [
            { value: DiscountType.Fixed, text: this.translate.instant('FIXED') },
            { value: DiscountType.Percent, text: this.translate.instant('PERCENT') }
        ];
    }

    public getProfitTypeText(type: ProfitType) {

        let text = '';
        switch (type) {
            case ProfitType.Fixed:
                text = this.translate.instant('FIXED');
                break;
            case ProfitType.Percent:
                text = this.translate.instant('PERCENT');
                break;
            default:
                text = '';
                break;
        }
        return text;
    }

    public getAppointmentStatusText(type: AppointmentStatus) {

        let text = '';
        switch (type) {
            case AppointmentStatus.Appointed:
                text = this.translate.instant('APPOINTED');
                break;
            case AppointmentStatus.Cancelled:
                text = this.translate.instant('CANCELLED');
                break;
            case AppointmentStatus.Postponed:
                text = this.translate.instant('POSTPONED');
                break;
            case AppointmentStatus.CarriedOut:
                text = this.translate.instant('CARRIED_OUT');
                break;
            default:
                text = '';
                break;
        }
        return text;
    }

    public getPaymentModes() {
        return [
            { value: false, text: this.translate.instant('DEFAULT') },
            { value: true, text: this.translate.instant('FROM_BALANCE') }
        ];
    }

    public getPaymentModeText(mode: boolean) {
        if (mode) {
            return this.translate.instant('FROM_BALANCE');
        } else {
            return this.translate.instant('DEFAULT');
        }
    }

    public getPaymentTypes() {
        return [
            { value: PaymentType.Cash, text: this.translate.instant('CASH') },
            { value: PaymentType.Card, text: this.translate.instant('CARD') },
            { value: PaymentType.Bank, text: this.translate.instant('BANK') }
        ];
    }

    public getPaymentTypeText(type: PaymentType) {

        let text = '';
        switch (type) {
            case PaymentType.Cash:
                text = this.translate.instant('CASH');
                break;
            case PaymentType.Card:
                text = this.translate.instant('CARD');
                break;
            case PaymentType.Bank:
                text = this.translate.instant('BANK');
                break;
            default:
                text = '';
                break;
        }
        return text;
    }

    public getPatientAgeTypeText(type: PatientAgeType) {
        let text = '';
        switch (type) {
            case PatientAgeType.Adult:
                text = this.translate.instant('ADULT');
                break;
            case PatientAgeType.Child:
                text = this.translate.instant('CHILD');
                break;
            case PatientAgeType.Baby:
                text = this.translate.instant('BABY');
                break;
            default:
                text = '';
                break;
        }
        return text;
    }

    public getPatientAgeTypes() {
        return [
            { value: PatientAgeType.Adult, text: this.translate.instant('ADULT') },
            { value: PatientAgeType.Child, text: this.translate.instant('CHILD') },
            { value: PatientAgeType.Baby, text: this.translate.instant('BABY') }
        ];
    }

    public getGenderText(type: Gender) {
        let text = '';
        switch (type) {
            case Gender.Female:
                text = this.translate.instant('FEMALE');
                break;
            case Gender.Male:
                text = this.translate.instant('MALE');
                break;
            default:
                text = '';
                break;
        }
        return text;
    }

    public getGenders() {
        return [
            { value: Gender.Male, text: this.translate.instant('MALE') },
            { value: Gender.Female, text: this.translate.instant('FEMALE') }
        ];
    }

    public getDentalServiceTypes() {
        return [
            { value: DentalServiceType.Common, text: this.translate.instant('COMMON') },
            { value: DentalServiceType.Treatment, text: this.translate.instant('TREATMENT') },
            { value: DentalServiceType.Additional, text: this.translate.instant('ADDITIONAL') }
        ];
    }

    public getDentalServiceTypeText(type: DentalServiceType): string {
        let text = '';
        switch (type) {
            case DentalServiceType.Additional:
                text = this.translate.instant('ADDITIONAL');
                break;
            case DentalServiceType.Common:
                text = this.translate.instant('COMMON');
                break;
            case DentalServiceType.Treatment:
                text = this.translate.instant('TREATMENT');
                break;
            default:
                text = '';
                break;
        }
        return text;
    }

    public getToothStates() {
        return [
            { value: ToothState.Healthy, text: this.translate.instant('TOOTH_STATE_HEALTHY') },
            { value: ToothState.Sealed, text: this.translate.instant('TOOTH_STATE_SEALED') },
            { value: ToothState.Depulpated, text: this.translate.instant('TOOTH_STATE_DEPULPATED') },
            { value: ToothState.Extracted, text: this.translate.instant('TOOTH_STATE_EXTRACTED') },
            { value: ToothState.Implanted, text: this.translate.instant('TOOTH_STATE_IMPLANTED') },
            { value: ToothState.Crowned, text: this.translate.instant('TOOTH_STATE_CROWNED') }
        ];
    }

    public getToothStateText(type: ToothState): string {
        let text = '';
        switch (type) {
            case ToothState.Healthy:
                text = this.translate.instant('TOOTH_STATE_HEALTHY');
                break;
            case ToothState.Sealed:
                text = this.translate.instant('TOOTH_STATE_SEALED');
                break;
            case ToothState.Depulpated:
                text = this.translate.instant('TOOTH_STATE_DEPULPATED');
                break;
            case ToothState.Extracted:
                text = this.translate.instant('TOOTH_STATE_EXTRACTED');
                break;
            case ToothState.Implanted:
                text = this.translate.instant('TOOTH_STATE_IMPLANTED');
                break;
            case ToothState.Crowned:
                text = this.translate.instant('TOOTH_STATE_CROWNED');
                break;
            default:
                text = '';
                break;
        }
        return text;
    }
}
