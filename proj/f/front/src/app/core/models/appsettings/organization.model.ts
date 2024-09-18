import { AddressModel } from '../manuals';
import { DoctorModel } from '../user-management';

export interface OrganizationModel {
    name: string;
    inn: string;
    okonx: string;
    oked: string;
    mfo: string;
    addressId: number;
    address: AddressModel;
    logoImage: string;
    doctors: DoctorModel[];
}
