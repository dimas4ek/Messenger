using Application.DTO;
using Guna.UI2.WinForms;

namespace Client.UI;

public class MessagePanelFactory
{
    public static Guna2Panel Create(MessageInfo message, MouseEventHandler onRightClick)
    {
        var nameLabel = new Label();
        nameLabel.Text = message.Sender.Username;
        nameLabel.Font = new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold);
        nameLabel.ForeColor = Color.LightGray;
        nameLabel.AutoSize = true;
        nameLabel.Tag = message.Sender;

        var messageLabel = new Label();
        messageLabel.Name = "messageLabel";
        messageLabel.Text = message.Text;
        messageLabel.Font = new Font(FontFamily.GenericSansSerif, 12);
        messageLabel.ForeColor = Color.White;
        messageLabel.AutoSize = true;
        messageLabel.MaximumSize = new Size(380, 0);
        messageLabel.Location = new Point(0, 20);

        var messagePanel = new Guna2Panel();
        messagePanel.BackColor = Color.FromArgb(24, 37, 51);
        messagePanel.MaximumSize = new Size(400, 0);
        messagePanel.AutoSize = true;
        messagePanel.Tag = message;
        messagePanel.MouseClick += onRightClick;

        messagePanel.Controls.Add(nameLabel);
        messagePanel.Controls.Add(messageLabel);

        return messagePanel;
    }

    public static void UpdateText(Guna2Panel panel, string newText)
    {
        if (panel.Controls.Find("messageLabel", false).FirstOrDefault() is Label label)
            label.Text = newText;
    }
}