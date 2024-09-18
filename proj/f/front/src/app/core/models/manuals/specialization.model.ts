import { ProfessionModel } from './profession.model';

export interface SpecializationModel {
    id: number;
    name: string;
    description: string;
    professionId: number;
    profession: ProfessionModel;
}
