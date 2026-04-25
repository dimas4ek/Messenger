using System.Diagnostics;
using Application.DTO;
using Client.Properties;
using Client.UI.Utils;
using Domain.Enums;
using Guna.UI2.WinForms;

namespace Client.UI.Factory;

public static class ChatPanelFactory
{
    public static Guna2Panel Create(
        ChatInfo chat,
        int currentUserId,
        MouseEventHandler onMove,
        EventHandler onLeave,
        MouseEventHandler onClick)
    {
        var chatPanel = new Guna2Panel();
        chatPanel.Size = new Size(210, 70);
        chatPanel.BackColor = Color.FromArgb(23, 33, 43);
        chatPanel.Dock = DockStyle.Top;
        chatPanel.Tag = chat;

        var chatImage = new Guna2CirclePictureBox
        {
            Size = new Size(50, 50),
            Name = "chatImage",
            Location = new Point(6, 10),
            SizeMode = PictureBoxSizeMode.StretchImage,
            Image = GetAvatar(chat),
            BackColor = Color.Transparent,
            Tag = chat
        };
        chatPanel.Controls.Add(chatImage);

        Debug.WriteLine(chat.Name);
        Debug.WriteLine(chat.Type);
        Debug.WriteLine(chat.Participants.Count);

        if (chat.Type == ChatType.Private)
        {
            var companion = chat.Participants.FirstOrDefault(p => p.UserId != currentUserId);
            var statusDot = new CirclePanel
            {
                Size = new Size(14, 14),
                Name = "statusDot",
                Location = new Point(chatImage.Right - 14, chatImage.Bottom - 14),
                BackColor = companion?.User.Status == UserStatus.Online ? Color.Green : Color.Gray,
                Tag = chat
            };
            chatPanel.Controls.Add(statusDot);
            statusDot.BringToFront();
        }

        var chatLabel = new Label();
        chatLabel.Name = "chatLabel";
        chatLabel.Parent = chatPanel;
        chatLabel.Text = chat.Name;
        chatLabel.Location = new Point(chatImage.Size.Width + 10, 13);
        chatLabel.Font = new Font(FontFamily.GenericSansSerif, 12);
        chatLabel.BackColor = Color.FromArgb(23, 33, 43);
        chatLabel.ForeColor = Color.White;
        chatLabel.Tag = chat;

        chatPanel.MouseMove += onMove;
        chatPanel.MouseLeave += onLeave;
        chatPanel.MouseClick += onClick;

        PropagateMouseEvents(chatImage, onMove, onLeave, onClick);
        PropagateMouseEvents(chatLabel, onMove, onLeave, onClick);

        return chatPanel;
    }

    public static void UpdateText(Guna2Panel panel, string newText)
    {
        if (panel.Controls.Find("chatLabel", false).FirstOrDefault() is Label label)
            label.Text = newText;
    }

    public static void UpdateAvatar(Guna2Panel panel, ImageInfo avatar)
    {
        if (panel.Controls.Find("chatImage", false).FirstOrDefault() is Guna2CirclePictureBox pictureBox)
            pictureBox.Image = Image.FromStream(new MemoryStream(avatar.Data));
    }

    public static void UpdateStatus(Guna2Panel panel, UserStatus status)
    {
        if (panel.Controls.Find("statusDot", false).FirstOrDefault() is CirclePanel circlePanel)
            circlePanel.BackColor = status == UserStatus.Online ? Color.Green : Color.Gray;
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