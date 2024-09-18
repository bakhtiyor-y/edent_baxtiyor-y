import { ToothDirection, ToothType } from '../../enums';

export interface ToothModel {
    id: number;
    name: string;
    position: number;
    direction: ToothDirection;
    toothType: ToothType;
}
