import { SpecializationModel } from './specialization.model';

export interface ProfessionModel {
    id: number;
    name: string;
    description: string;
    specializations: SpecializationModel[];
}
