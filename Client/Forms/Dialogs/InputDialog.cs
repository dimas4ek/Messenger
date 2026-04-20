using Guna.UI2.WinForms;

namespace Client.Forms.Dialogs;

public sealed class InputDialog : Form
{
    public InputDialog(
        string title,
        string initialValue = "",
        string placeholder = "",
        bool isPassword = false)
    {
        Size = new Size(400, 170);
        StartPosition = FormStartPosition.CenterParent;
        Text = title;
        BackColor = Color.FromArgb(23, 33, 43);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        var textBox = new Guna2TextBox
        {
            Text = initialValue,
            PlaceholderText = placeholder,
            Location = new Point(10, 10),
            Size = new Size(360, 40),
            ForeColor = Color.White,
            FillColor = Color.FromArgb(30, 43, 56),
            UseSystemPasswordChar = isPassword
        };

        var confirmButton = new Guna2Button
        {
            Text = "Сохранить",
            Location = new Point(270, 70),
            Size = new Size(100, 35),
            FillColor = Color.FromArgb(45, 140, 240),
            ForeColor = Color.White
        };

        confirmButton.Click += (_, _) =>
        {
            Result = textBox.Text.Trim();
            DialogResult = DialogResult.OK;
            Close();
        };

        Controls.Add(textBox);
        Controls.Add(confirmButton);
    }

    public string? Result { get; private set; }
}