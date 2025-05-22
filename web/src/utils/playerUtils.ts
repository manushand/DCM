import { Player } from '../models/Player';
import { GroupMember } from '../models/Group';

export const normalizePlayerName = (p: Player | GroupMember) => {
  if ('firstName' in p && p.firstName && p.lastName) {
    return `${p.firstName} ${p.lastName}`;
  }
  if ('name' in p && p.name) {
    return p.name;
  }
  if ('playerName' in p && p.playerName) {
    return p.playerName;
  }
  return 'Unknown Player';
};
