import React from 'react';
import { Box, CircularProgress, Typography } from '@mui/material';

interface LoadingProps {
	text?: string;
	error?: string | null;
}

const Loading: React.FC<LoadingProps> = ({ text = 'Loading...', error }) => {
	return (
		<Box
			display="flex"
			flexDirection="column"
			alignItems="center"
			justifyContent="center"
			height="100vh"
		>
			{error ? (
				<Typography variant="h6" color="error">
					{error}
				</Typography>
			) : (
				<>
					<CircularProgress />
					<Typography variant="h6" mt={2}>
						{text}
					</Typography>
				</>
			)}
		</Box>
	);
};

export default Loading;