import { DetailedModel } from './BaseModel';

export interface ScoringSystem extends DetailedModel {
  description?: string;
  drawCalculation?: string;
  eliminationValue?: number;
  survivorValue?: number;
  soloValue?: number;
  centerValue?: number;
  yearValue?: number;
  isDefault?: boolean;
}
