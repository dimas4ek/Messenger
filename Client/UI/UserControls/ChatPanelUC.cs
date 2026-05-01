using Application.DTO;
using Client.UI.Utils;
using Domain.Enums;

namespace Client.UI.UserControls;

public sealed partial class ChatPanelUC : UserControl
{
    public ChatPanelUC(
        ChatInfo chat,
        int currentUserId,
        MouseEventHandler onMove,
        EventHandler onLeave,
        MouseEventHandler onClick)
    {
        InitializeComponent();

        Tag = chat;
        Dock = DockStyle.Top;
        chatPanel.Tag = chat;

        Chat = chat;

        chatImage.Image = ImageHelper.GetAvatar(chat.Image);
        chatImage.Tag = chat;

        statusDot.Visible = false;
        if (chat.Type == ChatType.Private)
        {
            statusDot.Visible = true;
            var companion = chat.Participants.FirstOrDefault(p => p.UserId != currentUserId);

            statusDot.Location = new Point(chatImage.Right - 14, chatImage.Bottom - 14);
            statusDot.BackColor = companion?.User.Status == UserStatus.Online ? Color.Green : Color.Gray;
            statusDot.Tag = chat;

            statusDot.BringToFront();

            MouseEventUtils.PropagateMouseEvents(statusDot, onMove, onLeave, onClick);
        }

        chatLabel.Text = chat.Name + " {" + chat.Id + "}";
        chatLabel.Location = new Point(chatImage.Size.Width + 10, 13);
        chatLabel.Tag = chat;

        MouseEventUtils.PropagateMouseEvents(chatPanel, onMove, onLeave, onClick);
        MouseEventUtils.PropagateMouseEvents(chatImage, onMove, onLeave, onClick);
        MouseEventUtils.PropagateMouseEvents(chatLabel, onMove, onLeave, onClick);
    }

    public ChatInfo Chat { get; private set; }

    public void UpdateText(string newText)
    {
        chatLabel.Text = newText;
    }

    public void UpdateAvatar(ImageInfo avatar)
    {
        chatImage.Image = Image.FromStream(new MemoryStream(avatar.Data));
    }

    public void UpdateStatus(UserStatus status)
    {
        statusDot.BackColor = status == UserStatus.Online ? Color.Green : Color.Gray;
    }
}