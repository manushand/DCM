namespace PC.Forms;

internal sealed partial class GroupListForm : Form
{
	#region Public interface

	#region Data

	[DesignerSerializationVisibility(Hidden)]
	internal Group? Group { get; private set; }

	#endregion

	#region Constructor

	public GroupListForm()
		=> InitializeComponent();

	#endregion

	#endregion

	#region Private implementation

	#region Event handlers

	private void GroupListForm_Load(object sender,
									EventArgs e)
		=> GroupComboBox.FillWithSorted<Group>();

	private void GroupComboBox_SelectedIndexChanged(object sender,
													EventArgs e)
	{
		if (GroupComboBox.SelectedItem is not Group group)
			return;
		Group = group;
		Close();
		DialogResult = DialogResult.OK;
	}

	#endregion

	#endregion
}
