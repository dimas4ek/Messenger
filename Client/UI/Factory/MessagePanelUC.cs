using Application.DTO;

namespace Client.UI.Factory;

public partial class MessagePanelUC : UserControl
{
    public MessagePanelUC(MessageInfo message, MouseEventHandler onRightClick)
    {
        InitializeComponent();

        Message = message;

        nameLabel.Text = message.Sender.Username;
        nameLabel.Tag = message.Sender;

        messageLabel.Text = message.Text;
        messageLabel.MouseClick += onRightClick;

        messagePanel.Tag = message;
        messagePanel.MouseClick += onRightClick;
    }

    public MessageInfo Message { get; private set; }

    public void UpdateText(string newText)
    {
        messageLabel.Text = newText;
    }

    public void ConfirmMessage(MessageInfo confirmedMessage)
    {
        Message = confirmedMessage;
    }
}