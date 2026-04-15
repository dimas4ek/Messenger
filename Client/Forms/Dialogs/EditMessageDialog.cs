using Guna.UI2.WinForms;

namespace Client.Forms.Dialogs;

public sealed class EditMessageDialog : Form
{
    public string? Result { get; private set; }

    public EditMessageDialog(string currentText)
    {
        Size = new Size(400, 150);
        StartPosition = FormStartPosition.CenterParent;
        Text = "Изменить сообщение";
        BackColor = Color.FromArgb(23, 33, 43);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        var textBox = new Guna2TextBox
        {
            Text = currentText,
            Location = new Point(10, 10),
            Size = new Size(360, 40),
            ForeColor = Color.White,
            FillColor = Color.FromArgb(30, 43, 56)
        };

        var confirmButton = new Guna2Button
        {
            Text = "Сохранить",
            Location = new Point(270, 60),
            Size = new Size(100, 35),
            FillColor = Color.FromArgb(45, 140, 240),
            ForeColor = Color.White
        };

        confirmButton.Click += (_, _) =>
        {
            Result = textBox.Text.Trim();
            Close();
        };

        Controls.Add(textBox);
        Controls.Add(confirmButton);
    }
}