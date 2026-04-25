namespace Client.Forms.Dialogs;

public partial class InputDialog : Form
{
    public InputDialog(string title,
        string initialValue = "",
        string placeholder = "",
        bool isPassword = false)
    {
        InitializeComponent();

        Load += (_, _) =>
        {
            Text = title;

            textBox.Text = initialValue;
            textBox.PlaceholderText = placeholder;
            textBox.UseSystemPasswordChar = isPassword;

            confirmButton.Click += (_, _) =>
            {
                Result = textBox.Text.Trim();
                DialogResult = DialogResult.OK;
                Close();
            };
        };
    }

    public string? Result { get; private set; }
}