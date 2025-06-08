import { Powers } from '../models/Game';

// Get power color based on the power
export const getPowerColor = (power: Powers) => {
  switch (power) {
    case Powers.Austria:
      return { color: 'white', backgroundColor: 'red' };
    case Powers.England:
      return { color: 'white', backgroundColor: 'royalblue' };
    case Powers.France:
      return { color: 'black', backgroundColor: 'skyblue' };
    case Powers.Germany:
      return { color: 'white', backgroundColor: 'black' };
    case Powers.Italy:
      return { color: 'black', backgroundColor: 'lime' };
    case Powers.Russia:
      return { color: 'black', backgroundColor: 'white' };
    case Powers.Turkey:
      return { color: 'black', backgroundColor: 'yellow' };
    default:
      return { color: 'inherit', backgroundColor: 'inherit' };
  }
};
