import {render, screen, waitFor} from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import TournamentSelector from './TournamentSelector';
import {tournamentService} from '../../services';
import {GameStatus} from "../../models/Game";

jest.mock('../../services', () => ({
	tournamentService: {
		getActiveTournaments: jest.fn(),
		getRounds: jest.fn(),
		getAvailableBoards: jest.fn(),
	},
}));

const mockTournaments = [
	{ id: 1, name: 'Spring Open' },
	{ id: 2, name: 'Fall Classic' },
];

const mockRounds = [
	{ number: 1, name: 'Round 1', boardCount: 5, availableBoards: [1, 2, 3] },
	{ number: 2, name: 'Round 2', boardCount: 5, availableBoards: [4, 5] },
];

describe('TournamentSelector', () => {
	const props = {
		game: null,
		tournamentId: undefined,
		tournamentName: undefined,
		round: undefined,
		board: undefined,
		onTournamentChange: jest.fn(),
		onRoundChange: jest.fn(),
		onBoardChange: jest.fn(),
	};

	beforeEach(() => {
		jest.clearAllMocks();
		(tournamentService.getActiveTournaments as jest.Mock).mockResolvedValue(
			mockTournaments
		);
		(tournamentService.getRounds as jest.Mock).mockResolvedValue(mockRounds);
		(tournamentService.getAvailableBoards as jest.Mock).mockResolvedValue([1, 2, 3]);
	});

	it('renders and selects a tournament', async () => {
		render(<TournamentSelector {...props} />);

		const user = userEvent.setup();
		const autocomplete = await screen.findByLabelText('Tournament');
		await user.click(autocomplete);
		await user.click(screen.getByText('Spring Open'));

		await waitFor(() =>
			expect(props.onTournamentChange).toHaveBeenCalledWith(1, 'Spring Open')
		);
	});

	it('renders and selects a round', async () => {
		render(<TournamentSelector {...props} tournamentId={1} />);

		const user = userEvent.setup();
		const roundSelect = await screen.findByLabelText('Round');
		await user.click(roundSelect);
		await user.click(screen.getByText('Round 2'));

		expect(props.onRoundChange).toHaveBeenCalledWith(2);
	});

	it('enters board number and calls onBoardChange', async () => {
		render(<TournamentSelector {...props} tournamentId={1} round={1} />);

		const user = userEvent.setup();
		const boardInput = await screen.findByLabelText('Board');
		await user.type(boardInput, '2');

		// `onBoardChange` gets called on each keystroke, so wait for final value
		expect(props.onBoardChange).toHaveBeenLastCalledWith(2);
	});

	it('shows validation warning for invalid board number', async () => {
		render(<TournamentSelector {...props} tournamentId={1} round={1} board={99} />);

		expect(
			await screen.findByText(/board 99 is already in use/i)
		).toBeInTheDocument();
	});

	it('displays available boards message', async () => {
		render(<TournamentSelector {...props} tournamentId={1} round={1} board={1} />);
		expect(await screen.findByText(/Available boards: 1, 2, 3/)).toBeInTheDocument();
	});

	it('displays error message on fetch failure', async () => {
		(tournamentService.getActiveTournaments as jest.Mock).mockRejectedValue(
			new Error('fail')
		);

		render(<TournamentSelector {...props} />);
		expect(
			await screen.findByText(/Failed to load tournaments/i)
		).toBeInTheDocument();
	});

	it('disables all inputs when game.id is present', async () => {
		render(<TournamentSelector {...props} game={{ id: 99, name: 'Bismark', status: GameStatus.Scheduled }} />);
		expect(screen.getByLabelText('Tournament')).toBeDisabled();
		expect(screen.getByLabelText('Round')).toBeDisabled();
		expect(screen.getByLabelText('Board')).toBeDisabled();
	});

	it('shows "No rounds available" helper text if no rounds', async () => {
		(tournamentService.getRounds as jest.Mock).mockResolvedValue([]);

		render(<TournamentSelector {...props} tournamentId={1} />);
		expect(await screen.findByText('No rounds available')).toBeInTheDocument();
	});
});
