using System.ComponentModel;

namespace Client.Forms.Dialogs;

partial class CreateGroupChatDialog
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
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        groupImage = new Guna.UI2.WinForms.Guna2CirclePictureBox();
        groupNameLabel = new System.Windows.Forms.Label();
        groupNameTextBox = new Guna.UI2.WinForms.Guna2TextBox();
        addFriendsLabel = new System.Windows.Forms.Label();
        friendsPanel = new Guna.UI2.WinForms.Guna2Panel();
        createGroupChatButton = new Guna.UI2.WinForms.Guna2Button();
        ((System.ComponentModel.ISupportInitialize)groupImage).BeginInit();
        SuspendLayout();
        // 
        // groupImage
        // 
        groupImage.BackColor = System.Drawing.Color.Transparent;
        groupImage.ImageRotate = 0F;
        groupImage.Location = new System.Drawing.Point(140, 10);
        groupImage.Name = "groupImage";
        groupImage.ShadowDecoration.CustomizableEdges = customizableEdges1;
        groupImage.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
        groupImage.Size = new System.Drawing.Size(100, 100);
        groupImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        groupImage.TabIndex = 0;
        groupImage.TabStop = false;
        // 
        // nameLabel
        // 
        groupNameLabel.AutoSize = true;
        groupNameLabel.ForeColor = System.Drawing.Color.White;
        groupNameLabel.Location = new System.Drawing.Point(20, 120);
        groupNameLabel.Name = "groupNameLabel";
        groupNameLabel.Size = new System.Drawing.Size(75, 15);
        groupNameLabel.TabIndex = 1;
        groupNameLabel.Text = "Group Name";
        // 
        // groupNameTextBox
        // 
        groupNameTextBox.BorderColor = System.Drawing.Color.FromArgb(((int)((byte)55)), ((int)((byte)66)), ((int)((byte)80)));
        groupNameTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
        groupNameTextBox.CustomizableEdges = customizableEdges2;
        groupNameTextBox.DefaultText = "";
        groupNameTextBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)((byte)208)), ((int)((byte)208)), ((int)((byte)208)));
        groupNameTextBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)((byte)226)), ((int)((byte)226)), ((int)((byte)226)));
        groupNameTextBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)138)), ((int)((byte)138)), ((int)((byte)138)));
        groupNameTextBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)((byte)138)), ((int)((byte)138)), ((int)((byte)138)));
        groupNameTextBox.FillColor = System.Drawing.Color.FromArgb(((int)((byte)35)), ((int)((byte)46)), ((int)((byte)60)));
        groupNameTextBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)((byte)94)), ((int)((byte)148)), ((int)((byte)255)));
        groupNameTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
        groupNameTextBox.ForeColor = System.Drawing.Color.White;
        groupNameTextBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)((byte)94)), ((int)((byte)148)), ((int)((byte)255)));
        groupNameTextBox.Location = new System.Drawing.Point(20, 145);
        groupNameTextBox.Name = "groupNameTextBox";
        groupNameTextBox.PlaceholderText = "Enter group name...";
        groupNameTextBox.SelectedText = "";
        groupNameTextBox.ShadowDecoration.CustomizableEdges = customizableEdges3;
        groupNameTextBox.Size = new System.Drawing.Size(345, 36);
        groupNameTextBox.TabIndex = 2;
        // 
        // friendsLabel
        // 
        addFriendsLabel.AutoSize = true;
        addFriendsLabel.ForeColor = System.Drawing.Color.White;
        addFriendsLabel.Location = new System.Drawing.Point(20, 205);
        addFriendsLabel.Name = "addFriendsLabel";
        addFriendsLabel.Size = new System.Drawing.Size(68, 15);
        addFriendsLabel.TabIndex = 3;
        addFriendsLabel.Text = "Add friends";
        // 
        // friendsPanel
        // 
        friendsPanel.AutoScroll = true;
        friendsPanel.CustomizableEdges = customizableEdges4;
        friendsPanel.Location = new System.Drawing.Point(20, 230);
        friendsPanel.Name = "friendsPanel";
        friendsPanel.ShadowDecoration.CustomizableEdges = customizableEdges5;
        friendsPanel.Size = new System.Drawing.Size(345, 180);
        friendsPanel.TabIndex = 4;
        // 
        // createButton
        // 
        createGroupChatButton.CustomizableEdges = customizableEdges6;
        createGroupChatButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
        createGroupChatButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
        createGroupChatButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)((byte)169)), ((int)((byte)169)), ((int)((byte)169)));
        createGroupChatButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)141)), ((int)((byte)141)), ((int)((byte)141)));
        createGroupChatButton.FillColor = System.Drawing.Color.FromArgb(((int)((byte)114)), ((int)((byte)137)), ((int)((byte)218)));
        createGroupChatButton.Font = new System.Drawing.Font("Segoe UI", 9F);
        createGroupChatButton.ForeColor = System.Drawing.Color.White;
        createGroupChatButton.Location = new System.Drawing.Point(20, 420);
        createGroupChatButton.Name = "createGroupChatButton";
        createGroupChatButton.ShadowDecoration.CustomizableEdges = customizableEdges7;
        createGroupChatButton.Size = new System.Drawing.Size(345, 36);
        createGroupChatButton.TabIndex = 5;
        createGroupChatButton.Text = "Create";
        // 
        // CreateGroupChatDialog
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.Color.FromArgb(((int)((byte)23)), ((int)((byte)33)), ((int)((byte)43)));
        ClientSize = new System.Drawing.Size(384, 461);
        Controls.Add(createGroupChatButton);
        Controls.Add(friendsPanel);
        Controls.Add(addFriendsLabel);
        Controls.Add(groupNameTextBox);
        Controls.Add(groupNameLabel);
        Controls.Add(groupImage);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        Text = "Create Group Chat";
        ((System.ComponentModel.ISupportInitialize)groupImage).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Guna.UI2.WinForms.Guna2Button createGroupChatButton;

    private Guna.UI2.WinForms.Guna2Panel friendsPanel;

    private System.Windows.Forms.Label addFriendsLabel;

    private Guna.UI2.WinForms.Guna2TextBox groupNameTextBox;

    private System.Windows.Forms.Label groupNameLabel;

    private Guna.UI2.WinForms.Guna2CirclePictureBox groupImage;

    #endregion
}