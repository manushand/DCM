import React, { ReactNode } from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  IconButton,
  Typography,
  Box
} from '@mui/material';
import CloseIcon from '@mui/icons-material/Close';

interface FormDialogProps {
  open: boolean;
  onClose: () => void;
  onSubmit?: () => void;
  title: string;
  children: ReactNode;
  maxWidth?: 'xs' | 'sm' | 'md' | 'lg' | 'xl';
  fullWidth?: boolean;
  submitLabel?: string;
  cancelLabel?: string;
  disableSubmit?: boolean;
}

const FormDialog: React.FC<FormDialogProps> = ({
  open,
  onClose,
  onSubmit,
  title,
  children,
  maxWidth = 'sm',
  fullWidth = true,
  submitLabel = 'Save',
  cancelLabel = 'Cancel',
  disableSubmit = false
}) => {
  const handleSubmit = () => {
    if (onSubmit) {
      onSubmit();
    }
  };

  return (
    <Dialog
      open={open}
      onClose={onClose}
      maxWidth={maxWidth}
      fullWidth={fullWidth}
      aria-labelledby="form-dialog-title"
    >
      <DialogTitle id="form-dialog-title">
        <Box display="flex" alignItems="center" justifyContent="space-between">
          <Typography variant="h6">{title}</Typography>
          <IconButton aria-label="close" onClick={onClose} size="small">
            <CloseIcon />
          </IconButton>
        </Box>
      </DialogTitle>
      <DialogContent dividers>
        {children}
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} color="primary">
          {cancelLabel}
        </Button>
        {onSubmit && (
          <Button
            onClick={handleSubmit}
            color="primary"
            variant="contained"
            disabled={disableSubmit}
          >
            {submitLabel}
          </Button>
        )}
      </DialogActions>
    </Dialog>
  );
};

export default FormDialog;