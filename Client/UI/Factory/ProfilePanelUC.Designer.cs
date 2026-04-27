using System.ComponentModel;

namespace Client.UI.Factory;

sealed partial class ProfilePanelUC
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

    #region Component Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        profilePanel = new Guna.UI2.WinForms.Guna2Panel();
        settingsButton = new Guna.UI2.WinForms.Guna2Button();
        addFriendTextBox = new Guna.UI2.WinForms.Guna2TextBox();
        addFriendButton = new Guna.UI2.WinForms.Guna2Button();
        closeProfileButton = new Guna.UI2.WinForms.Guna2ImageButton();
        usernameLabel = new System.Windows.Forms.Label();
        profilePanel.SuspendLayout();
        SuspendLayout();
        // 
        // profilePanel
        // 
        profilePanel.Controls.Add(settingsButton);
        profilePanel.Controls.Add(addFriendTextBox);
        profilePanel.Controls.Add(addFriendButton);
        profilePanel.Controls.Add(closeProfileButton);
        profilePanel.Controls.Add(usernameLabel);
        profilePanel.CustomizableEdges = customizableEdges8;
        profilePanel.Dock = System.Windows.Forms.DockStyle.Fill;
        profilePanel.Location = new System.Drawing.Point(0, 0);
        profilePanel.Name = "profilePanel";
        profilePanel.ShadowDecoration.CustomizableEdges = customizableEdges9;
        profilePanel.Size = new System.Drawing.Size(300, 300);
        profilePanel.TabIndex = 0;
        // 
        // settingsButton
        // 
        settingsButton.Cursor = System.Windows.Forms.Cursors.Hand;
        settingsButton.CustomizableEdges = customizableEdges1;
        settingsButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
        settingsButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
        settingsButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)((byte)169)), ((int)((byte)169)), ((int)((byte)169)));
        settingsButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)141)), ((int)((byte)141)), ((int)((byte)141)));
        settingsButton.Dock = System.Windows.Forms.DockStyle.Bottom;
        settingsButton.FillColor = System.Drawing.Color.FromArgb(((int)((byte)23)), ((int)((byte)33)), ((int)((byte)43)));
        settingsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
        settingsButton.ForeColor = System.Drawing.Color.White;
        settingsButton.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)((byte)35)), ((int)((byte)46)), ((int)((byte)60)));
        settingsButton.Location = new System.Drawing.Point(0, 250);
        settingsButton.Name = "settingsButton";
        settingsButton.ShadowDecoration.CustomizableEdges = customizableEdges2;
        settingsButton.Size = new System.Drawing.Size(300, 50);
        settingsButton.TabIndex = 4;
        settingsButton.Text = "Settings";
        // 
        // addFriendTextBox
        // 
        addFriendTextBox.BorderThickness = 0;
        addFriendTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
        addFriendTextBox.CustomizableEdges = customizableEdges3;
        addFriendTextBox.DefaultText = "";
        addFriendTextBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)((byte)208)), ((int)((byte)208)), ((int)((byte)208)));
        addFriendTextBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)((byte)226)), ((int)((byte)226)), ((int)((byte)226)));
        addFriendTextBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)138)), ((int)((byte)138)), ((int)((byte)138)));
        addFriendTextBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)((byte)138)), ((int)((byte)138)), ((int)((byte)138)));
        addFriendTextBox.FillColor = System.Drawing.Color.FromArgb(((int)((byte)35)), ((int)((byte)46)), ((int)((byte)60)));
        addFriendTextBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)((byte)94)), ((int)((byte)148)), ((int)((byte)255)));
        addFriendTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
        addFriendTextBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)((byte)94)), ((int)((byte)148)), ((int)((byte)255)));
        addFriendTextBox.Location = new System.Drawing.Point(0, 103);
        addFriendTextBox.Name = "addFriendTextBox";
        addFriendTextBox.PlaceholderText = "Enter Friend name...";
        addFriendTextBox.SelectedText = "";
        addFriendTextBox.ShadowDecoration.CustomizableEdges = customizableEdges4;
        addFriendTextBox.Size = new System.Drawing.Size(112, 35);
        addFriendTextBox.TabIndex = 3;
        addFriendTextBox.Visible = false;
        // 
        // addFriendButton
        // 
        addFriendButton.CustomizableEdges = customizableEdges5;
        addFriendButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
        addFriendButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
        addFriendButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)((byte)169)), ((int)((byte)169)), ((int)((byte)169)));
        addFriendButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)141)), ((int)((byte)141)), ((int)((byte)141)));
        addFriendButton.FillColor = System.Drawing.Color.FromArgb(((int)((byte)23)), ((int)((byte)33)), ((int)((byte)43)));
        addFriendButton.Font = new System.Drawing.Font("Segoe UI", 12F);
        addFriendButton.ForeColor = System.Drawing.Color.White;
        addFriendButton.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)((byte)35)), ((int)((byte)46)), ((int)((byte)60)));
        addFriendButton.Location = new System.Drawing.Point(0, 58);
        addFriendButton.Name = "addFriendButton";
        addFriendButton.ShadowDecoration.CustomizableEdges = customizableEdges6;
        addFriendButton.Size = new System.Drawing.Size(0, 45);
        addFriendButton.TabIndex = 2;
        addFriendButton.Text = "Add Friend";
        // 
        // closeProfileButton
        // 
        closeProfileButton.Cursor = System.Windows.Forms.Cursors.Hand;
        closeProfileButton.Image = global::Client.Properties.Resources.ProfileButtonImage;
        closeProfileButton.ImageOffset = new System.Drawing.Point(0, 0);
        closeProfileButton.ImageRotate = 0F;
        closeProfileButton.ImageSize = new System.Drawing.Size(20, 20);
        closeProfileButton.Location = new System.Drawing.Point(0, 0);
        closeProfileButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        closeProfileButton.Name = "closeProfileButton";
        closeProfileButton.ShadowDecoration.CustomizableEdges = customizableEdges7;
        closeProfileButton.Size = new System.Drawing.Size(89, 67);
        closeProfileButton.TabIndex = 1;
        closeProfileButton.Text = "image";
        // 
        // usernameLabel
        // 
        usernameLabel.AutoSize = true;
        usernameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
        usernameLabel.ForeColor = System.Drawing.Color.White;
        usernameLabel.Location = new System.Drawing.Point(0, 0);
        usernameLabel.Name = "usernameLabel";
        usernameLabel.Size = new System.Drawing.Size(0, 25);
        usernameLabel.TabIndex = 0;
        // 
        // ProfilePanelUC
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.Color.FromArgb(((int)((byte)23)), ((int)((byte)33)), ((int)((byte)43)));
        Controls.Add(profilePanel);
        Size = new System.Drawing.Size(300, 300);
        profilePanel.ResumeLayout(false);
        profilePanel.PerformLayout();
        ResumeLayout(false);
    }

    private Guna.UI2.WinForms.Guna2Button settingsButton;

    private Guna.UI2.WinForms.Guna2TextBox addFriendTextBox;

    private System.Windows.Forms.Label usernameLabel;
    private Guna.UI2.WinForms.Guna2ImageButton closeProfileButton;
    private Guna.UI2.WinForms.Guna2Button addFriendButton;

    private Guna.UI2.WinForms.Guna2Panel profilePanel;

    #endregion
}