import { ApiTournamentService } from './tournamentService';
import { ApiPlayerService } from './playerService';
import { ApiGameService } from './gameService';
import { ApiGroupService } from './groupService';
import { ApiScoringSystemService } from './scoringSystemService';
import { ApiRankingSystemService } from './rankingSystemService';

import { MockTournamentService } from '../mocks/tournamentService.mock';
import { MockPlayerService } from '../mocks/playerService.mock';
import { MockScoringSystemService } from '../mocks/scoringSystemService.mock';
import { MockGroupService } from '../mocks/groupService.mock';
import { MockGameService } from '../mocks/gameService.mock';
import { MockRankingSystemService } from '../mocks/rankingSystemService.mock';

import type { TournamentService } from '../types/services/TournamentService';
import type { PlayerService } from '../types/services/PlayerService';
import type { ScoringSystemService } from '../types/services/ScoringSystemService';
import type { GameService } from '../types/services/GameService';
import type { GroupService } from '../types/services/GroupService';
import type { RankingSystemService } from '../types/services/RankingSystemService';

const useMocks = process.env.REACT_APP_USE_MOCKS === 'true';

export const groupService: GroupService = useMocks
  ? new MockGroupService()
  : new ApiGroupService();
export const gameService: GameService = useMocks
  ? new MockGameService()
  : new ApiGameService();
export const playerService: PlayerService = useMocks
  ? new MockPlayerService()
  : new ApiPlayerService();
export const scoringSystemService: ScoringSystemService = useMocks
  ? new MockScoringSystemService()
  : new ApiScoringSystemService();
export const rankingSystemService: RankingSystemService = useMocks
  ? new MockRankingSystemService()
  : new ApiRankingSystemService();
export const tournamentService: TournamentService = useMocks
  ? new MockTournamentService()
  : new ApiTournamentService();
