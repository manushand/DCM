import React from 'react';
import TournamentSelector from './TournamentSelector';
import { tournamentService } from '../../services';
import {TournamentStatus} from "../../models/Tournament";

export default {
	title: 'Components/TournamentSelector',
	component: TournamentSelector,
};

tournamentService.getActiveTournaments = async () => [
	{ id: 1, name: 'Spring Open', status: TournamentStatus.Finished },
	{ id: 2, name: 'Fall Classic', status: TournamentStatus.Scheduled },
];
tournamentService.getRounds = async () => [
	{ number: 1, name: 'Round 1', boardCount: 5, availableBoards: [1, 2, 3] },
];
tournamentService.getAvailableBoards = async () => [1, 2, 3];

export const Default = () => (
	<TournamentSelector
		game={null}
		tournamentId={1}
		tournamentName="Spring Open"
		round={1}
		board={2}
		onTournamentChange={(id, name) =>
			console.log('tournament change', id, name)
		}
		onRoundChange={(round) => console.log('round change', round)}
		onBoardChange={(board) => console.log('board change', board)}
	/>
);
