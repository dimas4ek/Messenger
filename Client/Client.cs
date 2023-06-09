using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
using System.Windows.Forms;
using Database;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Client
{
    public partial class Client : Form
    {
        private static TcpClient client;
        private static NetworkStream stream;
        private static string username;
        private static string companion;

        private static readonly List<string> servers = new List<string>();

        private readonly List<Guna2Button> availableServersButtonsAdded = new List<Guna2Button>();

        private List<int> messages = new List<int>();

        private List<string> friendList = new List<string>();

        private int sizePlus;

        private bool isProfileOpen;
        private bool isFriendTxtBoxOpen;

        private Guna2Panel profilePanel;
        private Guna2TextBox addFriendTxtBox;

        public Client()
        {
            InitializeComponent();

            DoImportantThings();

            PopulateIpAddresses();
            CreateProfilePanel();
            UpdateFriendList();
        }

        private void DoImportantThings()
        {
            username = User.username;

            availableServers.Visible = false;
            companionPanel.Visible = false;
            txtBoxMessage.Visible = false;
            guna2Panel9.Visible = false;
            btnSndMsg.Visible = false;
            btnUpdServers.Visible = false;
        }

        private void CreateProfilePanel()
        {
            profilePanel = new Guna2Panel();
            profilePanel.Size = new Size(285, Height);
            profilePanel.BackColor = Color.FromArgb(23, 33, 43);
            profilePanel.Location = new Point(0, 0);
            profilePanel.Dock = DockStyle.Left;

            Controls.Add(profilePanel);

            var profileUsernameLabel = new Label();
            profileUsernameLabel.Parent = profilePanel;
            profileUsernameLabel.Location = new Point(75, 20);
            profileUsernameLabel.Font = new Font(FontFamily.GenericSansSerif, 15.75f);
            profileUsernameLabel.ForeColor = Color.White;
            profileUsernameLabel.Text = username;

            var closeProfileButton = new Guna2ImageButton();
            closeProfileButton.Parent = profilePanel;
            closeProfileButton.Location = new Point(5, 6);
            closeProfileButton.Size = new Size(64, 52);

            closeProfileButton.Image = Image.FromFile("btnProfileImage.png");
            var imageSize = new Size(40, 40);
            closeProfileButton.ImageSize = imageSize;
            closeProfileButton.HoverState.ImageSize = imageSize;
            closeProfileButton.CheckedState.ImageSize = imageSize;
            closeProfileButton.PressedState.ImageSize = imageSize;

            closeProfileButton.Click += CloseProfileButton_Click;

            var addFriendButton = new Guna2Button();
            addFriendButton.Parent = profilePanel;
            addFriendButton.Location = new Point(0, 58);
            addFriendButton.Size = new Size(285, 45);
            addFriendButton.FillColor = Color.FromArgb(23, 33, 43);
            addFriendButton.HoverState.FillColor = Color.FromArgb(35, 46, 60);
            addFriendButton.Text = "Add friend";
            addFriendButton.Font = new Font("Segoe UI", 12);
            addFriendButton.TextAlign = HorizontalAlignment.Center;

            addFriendButton.Click += AddFriendButton_Click;
            addFriendTxtBox = new Guna2TextBox();

            profilePanel.Visible = false;
        }

        private void AddFriendButton_Click(object sender, EventArgs e)
        {
            if (!isFriendTxtBoxOpen)
            {
                isFriendTxtBoxOpen = true;

                addFriendTxtBox.Visible = true;
                addFriendTxtBox.Parent = profilePanel;
                addFriendTxtBox.PlaceholderText = "Enter friend name...";
                addFriendTxtBox.PlaceholderForeColor = Color.FromArgb(193, 200, 207);
                addFriendTxtBox.FillColor = Color.FromArgb(35, 46, 60);
                addFriendTxtBox.Location = new Point(0, 103);
                addFriendTxtBox.Size = new Size(285, 45);
                addFriendTxtBox.BorderThickness = 0;

                addFriendTxtBox.KeyUp += AddFriendTxtBox_KeyUp;

                return;
            }

            isFriendTxtBoxOpen = false;
            addFriendTxtBox.Visible = false;
        }

        private void AddFriendTxtBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if(addFriendTxtBox.Text != "")
                { 
                    var foundFriend = DatabaseManager.FindFriend(addFriendTxtBox.Text);
                    if(foundFriend)
                    {
                        if(DatabaseManager.AlreadyFriends(username, addFriendTxtBox.Text))
                        {
                            MessageBox.Show("You are already friends with this user!");
                            return;
                        }
                        if(addFriendTxtBox.Text == username)
                        {
                            MessageBox.Show("You can't add yourself as a friend");
                            return;
                        }
                        
                        DatabaseManager.AddFriend(addFriendTxtBox.Text);
                        MessageBox.Show("User has been added to your friend list!");

                        UpdateFriendList();
                    }
                    else
                    {
                        MessageBox.Show("User not found!");
                    }
                    
                    addFriendTxtBox.Clear();
                }
            }
        }

        private void UpdateFriendList()
        {
            panelParent.Controls.Clear();

            friendList = DatabaseManager.GetFriendList();

            foreach (var friend in friendList)
            {
                if (ThereIsAlreadyFriend(friend)) continue;
                
                var friendPanel = new Guna2Panel();
                friendPanel.Parent = panelParent;
                friendPanel.Size = new Size(210, 70);
                friendPanel.BackColor = Color.FromArgb(23, 33, 43);
                friendPanel.MouseMove += FriendPanel_MouseMove;
                friendPanel.MouseLeave += FriendPanel_MouseLeave;
                friendPanel.MouseClick += FriendPanel_MouseClick;
                friendPanel.Dock = DockStyle.Top;

                var friendLabel = new Label();
                friendLabel.Parent = friendPanel;
                friendLabel.Text = friend;
                friendLabel.Location = new Point(6, 13);
                friendLabel.Font = new Font(FontFamily.GenericSansSerif, 12);
                friendLabel.BackColor = Color.FromArgb(23, 33, 43);
                friendLabel.ForeColor = Color.White;
                friendLabel.MouseMove += FriendLabel_MouseMove;
                friendLabel.MouseLeave += FriendLabel_MouseLeave;
            }
        }

        private bool ThereIsAlreadyFriend(string friend)
        {
            return (
                from Guna2Panel panel in panelParent.Controls 
                select panel.Controls.OfType<Label>().FirstOrDefault())
                .Any(label => label?.Text == friend);
        }

        private void FriendPanel_MouseClick(object sender, MouseEventArgs e)
        {
            var panel = sender as Guna2Panel;
            var label = panel?.Controls.OfType<Label>().FirstOrDefault();
            companion = label?.Text;
            lblCompanionUsername.Text = label?.Text;

            mySide.Controls.Clear();
            companionSide.Controls.Clear();

            LoadDialog();
        }

        private void LoadDialog()
        {
            companionPanel.Visible = true;
            btnSndMsg.Visible = true;
            txtBoxMessage.Visible = true;
            guna2Panel9.Visible = true;

            messages = DatabaseManager.LoadDialog(username, companion);
            if (messages.Count != 0)
            {
                foreach (var messageId in messages)
                {
                    LoadMessage(DatabaseManager.GetMessage(messageId), DatabaseManager.CheckSender(messageId));
                    sizePlus += 60;
                }
            }
        }

        private void LoadMessage(string message, bool isMeSender)
        {
            mySide.HorizontalScroll.Maximum = 0;
            companionSide.HorizontalScroll.Maximum = 0;

            var friendPanel = new Guna2Panel();
            friendPanel.Parent = isMeSender ? mySide : companionSide;
            friendPanel.Size = new Size(200, 50);
            friendPanel.Location = new Point(friendPanel.Location.X, friendPanel.Location.Y + sizePlus);
            friendPanel.BackColor = Color.FromArgb(24, 37, 51);

            var friendLabel = new Label();
            friendLabel.Parent = friendPanel;
            friendLabel.Text = message;
            friendLabel.Location = new Point(6, 13);
            friendLabel.Font = new Font(FontFamily.GenericSansSerif, 12);
            friendLabel.BackColor = Color.FromArgb(24, 37, 51);
            friendLabel.ForeColor = Color.White;
        }

        private static void FriendLabel_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Label label)
            {
                label.BackColor = Color.FromArgb(35, 46, 60);

                if (label.Parent is Guna2Panel panel)
                {
                    panel.BackColor = Color.FromArgb(35, 46, 60);
                }
            }
        }

        private static void FriendLabel_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Label label)
            {
                label.BackColor = Color.FromArgb(35, 46, 60);

                if (label.Parent is Guna2Panel panel)
                {
                    panel.BackColor = Color.FromArgb(35, 46, 60);
                }
            }
        }

        private static void FriendPanel_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Guna2Panel panel)
            {
                panel.BackColor = Color.FromArgb(23, 33, 43);

                var label = panel.Controls.OfType<Label>().FirstOrDefault();
                if (label != null)
                {
                    label.BackColor = Color.FromArgb(23, 33, 43);
                    label.ForeColor = Color.White;
                }
            }
        }

        private static void FriendPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Guna2Panel panel)
            {
                panel.BackColor = Color.FromArgb(35, 46, 60);

                var label = panel.Controls.OfType<Label>().FirstOrDefault();
                if (label != null)
                {
                    label.BackColor = Color.FromArgb(35, 46, 60);
                    label.ForeColor = Color.White;
                }
            }
        }

        private void ProcessChatting()
        {
            availableServers.Visible = false;

            if (servers.Count == 0)
            { 
                availableServersButtonsAdded.Insert(0, btnUpdServers);

                panelParent.Visible = false;
                guna2Panel.Visible = false;
                guna2Panel3.Visible = false;
                txtBoxSearch.Visible = false;
                guna2Panel10.Visible = false;

                availableServers.Visible = true;
                btnUpdServers.Visible = true;

                return;
            }

            panelParent.Visible = true;
            guna2Panel.Visible = true;
            guna2Panel3.Visible = true;
            txtBoxSearch.Visible = true;
            guna2Panel10.Visible = true;

            availableServers.Visible = false;
            btnUpdServers.Visible = false;

            if (availableServersButtonsAdded.Count > 0)
            {
                availableServers.Visible = false;
                btnUpdServers.Visible = false;

                var buttonToRemove = availableServersButtonsAdded[0];
                availableServersButtonsAdded.Remove(buttonToRemove);
                Controls.Remove(buttonToRemove);
            }

            try
            {
                var ipAddress = servers[0];

                if (!string.IsNullOrEmpty(ipAddress))
                {
                    client = new TcpClient(ipAddress, 5151);
                }
            }
            catch
            {
                // ignored
            }

            if (client != null && client.Connected)
            {
                stream = client.GetStream();
                var usernameBuffer = Encoding.ASCII.GetBytes(username);
                stream.Write(usernameBuffer, 0, usernameBuffer.Length);
                stream.Flush();

                var readThread = new Thread(ReadMessages);
                readThread.Start();
            }

            var clientListBuffer = Encoding.ASCII.GetBytes("");
            stream?.Write(clientListBuffer, 0, clientListBuffer.Length);
            stream?.Flush();
        }

        private void PopulateIpAddresses()
        {
            servers?.Clear();

            var localIPs = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            var ipv4Addresses = localIPs.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToArray();

            foreach (var ipAddress in ipv4Addresses)
            {
                var ip = ipAddress.ToString();
                var port = 5151;

                try
                {
                    using (var tcpClient = new TcpClient())
                    {
                        tcpClient.Connect(ip, port);

                        servers?.Add(ip);
                    }
                }
                catch
                {
                    // ignored
                }
            }

            ProcessChatting();
        }

        private async void ReadMessages()
        {
            while (true)
            {
                if (client != null && stream != null)
                {
                    var buffer = new byte[client.ReceiveBufferSize];

                    var bytesRead = await stream.ReadAsync(buffer, 0, client.ReceiveBufferSize);
                    if (bytesRead == 0)
                    {
                        MessageBox.Show("Disconnected from server");
                        break;
                    }

                    var message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    if (!string.IsNullOrEmpty(message))
                    {
                        if (lblCompanionUsername.Text != "companion" || companion != null)
                        {
                            if (message.StartsWith("You:"))
                            {
                                message = message.Remove(0, 4);
                            }
                            if (companion != null && message.StartsWith(companion))
                            {
                                message = message.Remove(0, companion.Length + 1);
                                Invoke(new Action(() =>
                                {
                                    LoadMessage(message, false);
                                }));
                            }
                        }
                    }
                }
            }
        }

        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            stream?.Close();
            client?.Close();
            Environment.Exit(0);
        }

        private void btnUpdServers_Click(object sender, EventArgs e)
        {
            PopulateIpAddresses();
        }

        private void btnOpenProfile_Click(object sender, EventArgs e)
        {
            OpenProfile();
        }

        private void OpenProfile()
        {
            if (!isProfileOpen)
            {
                isProfileOpen = true;

                profilePanel.Visible = true;

                guna2Panel10.Visible = false;
                panelParent.Visible = false;
                guna2Panel.Visible = false;
                guna2Panel3.Visible = false;
                txtBoxSearch.Visible = false;
            }
        }

        private void CloseProfileButton_Click(object sender, EventArgs e)
        {
            CloseProfile();
        }

        private void CloseProfile()
        {
            if (isProfileOpen)
            {
                isProfileOpen = false;

                profilePanel.Visible = false;

                guna2Panel10.Visible = true;
                panelParent.Visible = true;
                guna2Panel.Visible = true;
                guna2Panel3.Visible = true;
                txtBoxSearch.Visible = true;
            }
        }

        private void btnSndMsg_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void txtBoxMessage_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBoxMessage.Text != "")
                {
                    SendMessage();
                }
            }
        }

        private void SendMessage()
        {
            var message = txtBoxMessage.Text;

            DatabaseManager.SaveMessage(username, companion, message);

            var messageBuffer = Encoding.ASCII.GetBytes(message);
            stream?.Write(messageBuffer, 0, messageBuffer.Length);
            stream?.Flush();

            txtBoxMessage.Clear();

            LoadMessage(message, true);
        }

        private void txtBoxSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBoxSearch.Text != "")
                {
                    var foundedFriends = DatabaseManager.FriendsFromSearch(txtBoxSearch.Text);

                    if (foundedFriends.Count != 0)
                    {
                        panelParent.Controls.Clear();

                        foreach (var friend in foundedFriends)
                        {
                            if (friend == username) continue;

                            var friendPanel = new Guna2Panel();
                            friendPanel.Parent = panelParent;
                            friendPanel.Size = new Size(210, 70);
                            friendPanel.BackColor = Color.FromArgb(23, 33, 43);
                            friendPanel.MouseMove += FriendPanel_MouseMove;
                            friendPanel.MouseLeave += FriendPanel_MouseLeave;
                            friendPanel.MouseClick += FriendPanel_MouseClick;
                            friendPanel.Dock = DockStyle.Top;

                            var friendLabel = new Label();
                            friendLabel.Parent = friendPanel;
                            friendLabel.Text = friend;
                            friendLabel.Location = new Point(6, 13);
                            friendLabel.Font = new Font(FontFamily.GenericSansSerif, 12);
                            friendLabel.BackColor = Color.FromArgb(23, 33, 43);
                            friendLabel.ForeColor = Color.White;
                            friendLabel.MouseMove += FriendLabel_MouseMove;
                            friendLabel.MouseLeave += FriendLabel_MouseLeave;
                        }
                    }
                }
                else
                {
                    UpdateFriendList();
                }

                txtBoxSearch.Clear();
            }
        }
    }
}
