using Client.Properties;
using Client.Utils;

namespace Client.Forms.Dialogs;

public partial class InputDialog : Form
{
    public InputDialog(string title,
        string initialValue = "",
        string placeholder = "",
        bool isPassword = false)
    {
        InitializeComponent();
        LanguageManager.LanguageChanged += ApplyLocalization;
        ApplyLocalization();

        Load += (_, _) =>
        {
            Text = title;

            textBox.Text = initialValue;
            textBox.PlaceholderText = placeholder;
            textBox.UseSystemPasswordChar = isPassword;

            saveButton.Click += (_, _) =>
            {
                Result = textBox.Text.Trim();
                DialogResult = DialogResult.OK;
                Close();
            };
        };
    }
    
    #region Language
    
    private void ApplyLocalization()
    {
        saveButton.Text = Strings.InputDialog_Save;
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        LanguageManager.LanguageChanged -= ApplyLocalization;
        base.OnFormClosed(e);
    }
    
    #endregion

    public string? Result { get; private set; }
}