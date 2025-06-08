import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import FormDialog from './FormDialog';

describe('FormDialog', () => {
	const defaultProps = {
		open: true,
		onClose: jest.fn(),
		onSubmit: jest.fn(),
		title: 'Test Form',
		children: <div>Form content</div>,
	};

	beforeEach(() => {
		jest.clearAllMocks();
	});

	it('renders the dialog when open is true', () => {
		render(<FormDialog {...defaultProps} />);
		expect(screen.getByRole('dialog')).toBeInTheDocument();
		expect(screen.getByText('Test Form')).toBeInTheDocument();
		expect(screen.getByText('Form content')).toBeInTheDocument();
	});

	it('calls onClose when clicking the close icon', async () => {
		const user = userEvent.setup();
		render(<FormDialog {...defaultProps} />);
		await user.click(screen.getByLabelText(/close/i));
		expect(defaultProps.onClose).toHaveBeenCalled();
	});

	it('calls onClose when clicking the cancel button', async () => {
		const user = userEvent.setup();
		render(<FormDialog {...defaultProps} />);
		await user.click(screen.getByText('Cancel'));
		expect(defaultProps.onClose).toHaveBeenCalled();
	});

	it('calls onSubmit when clicking the submit button', async () => {
		const user = userEvent.setup();
		render(<FormDialog {...defaultProps} />);
		await user.click(screen.getByText('Save'));
		expect(defaultProps.onSubmit).toHaveBeenCalled();
	});

	it('disables the submit button when disableSubmit is true', () => {
		render(<FormDialog {...defaultProps} disableSubmit />);
		const submitBtn = screen.getByText('Save');
		expect(submitBtn).toBeDisabled();
	});

	it('does not render submit button when onSubmit is not provided', () => {
		render(<FormDialog {...defaultProps} onSubmit={undefined} />);
		expect(screen.queryByText('Save')).not.toBeInTheDocument();
	});

	it('uses custom labels if provided', () => {
		render(
			<FormDialog
				{...defaultProps}
				submitLabel="Confirm"
				cancelLabel="Back"
			/>
		);
		expect(screen.getByText('Confirm')).toBeInTheDocument();
		expect(screen.getByText('Back')).toBeInTheDocument();
	});
});
