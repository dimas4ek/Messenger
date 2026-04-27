using Application.DTO;
using Client.Properties;
using Domain.Enums;

namespace Client.UI.Factory;

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

        chatImage.Image = GetAvatar(chat);
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

            PropagateMouseEvents(statusDot, onMove, onLeave, onClick);
        }

        chatLabel.Text = chat.Name;
        chatLabel.Location = new Point(chatImage.Size.Width + 10, 13);
        chatLabel.Tag = chat;

        PropagateMouseEvents(chatPanel, onMove, onLeave, onClick);
        PropagateMouseEvents(chatImage, onMove, onLeave, onClick);
        PropagateMouseEvents(chatLabel, onMove, onLeave, onClick);
    }

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

    private static Image GetAvatar(ChatInfo chat)
    {
        if (chat.Image?.Data == null) return Resources.DefaultAvatarImage;

        using var ms = new MemoryStream(chat.Image.Data);
        return Image.FromStream(ms);
    }

    private static void PropagateMouseEvents(Control control, MouseEventHandler onMove, EventHandler onLeave,
        MouseEventHandler onClick)
    {
        control.MouseMove += onMove;
        control.MouseLeave += onLeave;
        control.MouseClick += onClick;
    }
}