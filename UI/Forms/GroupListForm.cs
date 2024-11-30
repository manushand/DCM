namespace DCM.UI.Forms;

internal sealed partial class GroupListForm : Form
{
	[DesignerSerializationVisibility(Hidden)]
	internal Group? Group { get; private set; }

	public GroupListForm()
		=> InitializeComponent();

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
}
