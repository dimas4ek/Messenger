using System.ComponentModel;

namespace Client.Forms.Dialogs;

partial class FriendListDialog
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        rightPanel = new Guna.UI2.WinForms.Guna2Panel();
        friendRequestsPanel = new Guna.UI2.WinForms.Guna2Panel();
        noneFriendsRequestsLabel = new System.Windows.Forms.Label();
        friendRequestTopPanel = new Guna.UI2.WinForms.Guna2Panel();
        friendRequestsLabel = new System.Windows.Forms.Label();
        leftPanel = new Guna.UI2.WinForms.Guna2Panel();
        friendListPanel = new Guna.UI2.WinForms.Guna2Panel();
        noneFriendsLabel = new System.Windows.Forms.Label();
        friendListTopPanel = new Guna.UI2.WinForms.Guna2Panel();
        friendListLabel = new System.Windows.Forms.Label();
        separator = new Guna.UI2.WinForms.Guna2Panel();
        rightPanel.SuspendLayout();
        friendRequestsPanel.SuspendLayout();
        friendRequestTopPanel.SuspendLayout();
        leftPanel.SuspendLayout();
        friendListPanel.SuspendLayout();
        friendListTopPanel.SuspendLayout();
        SuspendLayout();
        // 
        // rightPanel
        // 
        rightPanel.Controls.Add(friendRequestsPanel);
        rightPanel.Controls.Add(friendRequestTopPanel);
        rightPanel.CustomizableEdges = customizableEdges5;
        rightPanel.Dock = System.Windows.Forms.DockStyle.Right;
        rightPanel.Location = new System.Drawing.Point(284, 0);
        rightPanel.Name = "rightPanel";
        rightPanel.ShadowDecoration.CustomizableEdges = customizableEdges6;
        rightPanel.Size = new System.Drawing.Size(300, 381);
        rightPanel.TabIndex = 0;
        // 
        // friendRequestsPanel
        // 
        friendRequestsPanel.BorderColor = System.Drawing.Color.Black;
        friendRequestsPanel.Controls.Add(noneFriendsRequestsLabel);
        friendRequestsPanel.CustomizableEdges = customizableEdges1;
        friendRequestsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
        friendRequestsPanel.Location = new System.Drawing.Point(0, 50);
        friendRequestsPanel.Name = "friendRequestsPanel";
        friendRequestsPanel.ShadowDecoration.CustomizableEdges = customizableEdges2;
        friendRequestsPanel.Size = new System.Drawing.Size(300, 331);
        friendRequestsPanel.TabIndex = 1;
        // 
        // noneFriendsRequestsLabel
        // 
        noneFriendsRequestsLabel.AutoSize = true;
        noneFriendsRequestsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
        noneFriendsRequestsLabel.ForeColor = System.Drawing.Color.White;
        noneFriendsRequestsLabel.Location = new System.Drawing.Point(69, 20);
        noneFriendsRequestsLabel.Name = "noneFriendsRequestsLabel";
        noneFriendsRequestsLabel.Size = new System.Drawing.Size(131, 20);
        noneFriendsRequestsLabel.TabIndex = 2;
        noneFriendsRequestsLabel.Text = "Тут пока пусто...";
        // 
        // friendRequestTopPanel
        // 
        friendRequestTopPanel.Controls.Add(friendRequestsLabel);
        friendRequestTopPanel.CustomizableEdges = customizableEdges3;
        friendRequestTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
        friendRequestTopPanel.Location = new System.Drawing.Point(0, 0);
        friendRequestTopPanel.Name = "friendRequestTopPanel";
        friendRequestTopPanel.ShadowDecoration.CustomizableEdges = customizableEdges4;
        friendRequestTopPanel.Size = new System.Drawing.Size(300, 50);
        friendRequestTopPanel.TabIndex = 0;
        // 
        // friendRequestsLabel
        // 
        friendRequestsLabel.AutoSize = true;
        friendRequestsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
        friendRequestsLabel.ForeColor = System.Drawing.Color.White;
        friendRequestsLabel.Location = new System.Drawing.Point(78, 18);
        friendRequestsLabel.Name = "friendRequestsLabel";
        friendRequestsLabel.Size = new System.Drawing.Size(127, 20);
        friendRequestsLabel.TabIndex = 0;
        friendRequestsLabel.Text = "Friend Requests";
        // 
        // leftPanel
        // 
        leftPanel.Controls.Add(friendListPanel);
        leftPanel.Controls.Add(friendListTopPanel);
        leftPanel.CustomizableEdges = customizableEdges11;
        leftPanel.Dock = System.Windows.Forms.DockStyle.Fill;
        leftPanel.Location = new System.Drawing.Point(0, 0);
        leftPanel.Name = "leftPanel";
        leftPanel.ShadowDecoration.CustomizableEdges = customizableEdges12;
        leftPanel.Size = new System.Drawing.Size(283, 381);
        leftPanel.TabIndex = 1;
        // 
        // friendListPanel
        // 
        friendListPanel.BorderColor = System.Drawing.Color.Black;
        friendListPanel.Controls.Add(noneFriendsLabel);
        friendListPanel.CustomizableEdges = customizableEdges7;
        friendListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
        friendListPanel.Location = new System.Drawing.Point(0, 50);
        friendListPanel.Name = "friendListPanel";
        friendListPanel.ShadowDecoration.CustomizableEdges = customizableEdges8;
        friendListPanel.Size = new System.Drawing.Size(283, 331);
        friendListPanel.TabIndex = 1;
        // 
        // noneFriendsLabel
        // 
        noneFriendsLabel.AutoSize = true;
        noneFriendsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
        noneFriendsLabel.ForeColor = System.Drawing.Color.White;
        noneFriendsLabel.Location = new System.Drawing.Point(63, 20);
        noneFriendsLabel.Name = "noneFriendsLabel";
        noneFriendsLabel.Size = new System.Drawing.Size(131, 20);
        noneFriendsLabel.TabIndex = 1;
        noneFriendsLabel.Text = "Тут пока пусто...";
        // 
        // friendListTopPanel
        // 
        friendListTopPanel.Controls.Add(friendListLabel);
        friendListTopPanel.CustomizableEdges = customizableEdges9;
        friendListTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
        friendListTopPanel.Location = new System.Drawing.Point(0, 0);
        friendListTopPanel.Name = "friendListTopPanel";
        friendListTopPanel.ShadowDecoration.CustomizableEdges = customizableEdges10;
        friendListTopPanel.Size = new System.Drawing.Size(283, 50);
        friendListTopPanel.TabIndex = 0;
        // 
        // friendListLabel
        // 
        friendListLabel.AutoSize = true;
        friendListLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
        friendListLabel.ForeColor = System.Drawing.Color.White;
        friendListLabel.Location = new System.Drawing.Point(85, 18);
        friendListLabel.Name = "friendListLabel";
        friendListLabel.Size = new System.Drawing.Size(84, 25);
        friendListLabel.TabIndex = 0;
        friendListLabel.Text = "Friends";
        // 
        // separator
        // 
        separator.BackColor = System.Drawing.Color.Black;
        separator.CustomizableEdges = customizableEdges13;
        separator.Dock = System.Windows.Forms.DockStyle.Right;
        separator.Location = new System.Drawing.Point(283, 0);
        separator.Name = "separator";
        separator.ShadowDecoration.CustomizableEdges = customizableEdges14;
        separator.Size = new System.Drawing.Size(1, 381);
        separator.TabIndex = 0;
        // 
        // FriendsDialog
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.Color.FromArgb(((int)((byte)23)), ((int)((byte)33)), ((int)((byte)43)));
        ClientSize = new System.Drawing.Size(584, 381);
        Controls.Add(leftPanel);
        Controls.Add(separator);
        Controls.Add(rightPanel);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        Text = "Friends";
        rightPanel.ResumeLayout(false);
        friendRequestsPanel.ResumeLayout(false);
        friendRequestsPanel.PerformLayout();
        friendRequestTopPanel.ResumeLayout(false);
        friendRequestTopPanel.PerformLayout();
        leftPanel.ResumeLayout(false);
        friendListPanel.ResumeLayout(false);
        friendListPanel.PerformLayout();
        friendListTopPanel.ResumeLayout(false);
        friendListTopPanel.PerformLayout();
        ResumeLayout(false);
    }

    private System.Windows.Forms.Label noneFriendsLabel;
    private System.Windows.Forms.Label noneFriendsRequestsLabel;

    private Guna.UI2.WinForms.Guna2Panel separator;

    private Guna.UI2.WinForms.Guna2Panel friendListPanel;

    private Guna.UI2.WinForms.Guna2Panel friendRequestsPanel;

    private System.Windows.Forms.Label friendRequestsLabel;

    private System.Windows.Forms.Label friendListLabel;

    private Guna.UI2.WinForms.Guna2Panel friendListTopPanel;

    private Guna.UI2.WinForms.Guna2Panel friendRequestTopPanel;

    private Guna.UI2.WinForms.Guna2Panel leftPanel;

    private Guna.UI2.WinForms.Guna2Panel rightPanel;

    #endregion
}