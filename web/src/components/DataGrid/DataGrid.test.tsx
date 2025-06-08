import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import DataGrid from './DataGrid';
import userEvent from "@testing-library/user-event";

const columns = [
	{ id: 'name', label: 'Name', minWidth: 100 },
	{ id: 'age', label: 'Age', format: (value: number) => `Age: ${value}` },
];

const rows = [
	{ name: 'Alice', age: 30 },
	{ name: 'Bob', age: 25 },
	{ name: 'Charlie', age: 35 },
];

describe('DataGrid', () => {
	it('renders with a title', () => {
		render(<DataGrid columns={columns} rows={rows} title="Test Grid" />);
		expect(screen.getByText('Test Grid')).toBeInTheDocument();
	});

	it('renders column headers', () => {
		render(<DataGrid columns={columns} rows={rows} />);
		expect(screen.getByText('Name')).toBeInTheDocument();
		expect(screen.getByText('Age')).toBeInTheDocument();
	});

	it('renders formatted cell content', () => {
		render(<DataGrid columns={columns} rows={rows} />);
		expect(screen.getByText('Age: 30')).toBeInTheDocument();
		expect(screen.getByText('Age: 25')).toBeInTheDocument();
	});

	it('calls onRowClick when a row is clicked', () => {
		const onRowClick = jest.fn();
		render(<DataGrid columns={columns} rows={rows} onRowClick={onRowClick} />);

		const row = screen.getByText('Alice').closest('tr');
		fireEvent.click(row!);

		expect(onRowClick).toHaveBeenCalledWith(rows[0]);
	});

	it('paginates results correctly', async () => {
		const largeRows = Array.from({ length: 20 }, (_, i) => ({
			name: `User${i}`,
			age: 20 + i,
		}));

		render(<DataGrid columns={columns} rows={largeRows} />);

		// Only first 10 rows shown
		expect(screen.getByText('User0')).toBeInTheDocument();
		expect(screen.queryByText('User15')).not.toBeInTheDocument();

		// Go to next page
		const nextButton = screen.getByLabelText('Go to next page');
		fireEvent.click(nextButton);

		expect(await screen.findByText('User15')).toBeInTheDocument();
	});

	it('changes number of rows per page and resets page to 0', async () => {
		const user = userEvent.setup();
		const largeRows = Array.from({ length: 25 }, (_, i) => ({
			name: `User${i}`,
			age: 20 + i,
		}));

		render(<DataGrid columns={columns} rows={largeRows} />);

		// Open rows-per-page menu
		const rowsPerPageButton = screen.getByLabelText(/rows per page/i);
		await user.click(rowsPerPageButton);

		// Click on "25"
		await user.click(screen.getByRole('option', { name: '25' }));

		// Confirm more rows are now visible
		expect(screen.getByText('User20')).toBeInTheDocument();
	});


});
