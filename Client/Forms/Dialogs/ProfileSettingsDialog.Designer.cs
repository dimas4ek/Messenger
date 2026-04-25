using System.ComponentModel;

namespace Client.Forms.Dialogs;

partial class ProfileSettingsDialog
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
        avatar = new Guna.UI2.WinForms.Guna2CirclePictureBox();
        usernameLabel = new System.Windows.Forms.Label();
        usernameValueLabel = new System.Windows.Forms.Label();
        editUsernameButton = new Guna.UI2.WinForms.Guna2Button();
        passwordLabel = new System.Windows.Forms.Label();
        passwordValueLabel = new System.Windows.Forms.Label();
        editPasswordButton = new Guna.UI2.WinForms.Guna2Button();
        ((System.ComponentModel.ISupportInitialize)avatar).BeginInit();
        SuspendLayout();
        // 
        // avatar
        // 
        avatar.BackColor = System.Drawing.Color.Transparent;
        avatar.ImageRotate = 0F;
        avatar.Location = new System.Drawing.Point(140, 20);
        avatar.Name = "avatar";
        avatar.ShadowDecoration.CustomizableEdges = customizableEdges1;
        avatar.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
        avatar.Size = new System.Drawing.Size(100, 100);
        avatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        avatar.TabIndex = 0;
        avatar.TabStop = false;
        // 
        // usernameLabel
        // 
        usernameLabel.AutoSize = true;
        usernameLabel.ForeColor = System.Drawing.Color.Gray;
        usernameLabel.Location = new System.Drawing.Point(30, 150);
        usernameLabel.Name = "usernameLabel";
        usernameLabel.Size = new System.Drawing.Size(60, 15);
        usernameLabel.TabIndex = 1;
        usernameLabel.Text = "Username";
        // 
        // usernameValueLabel
        // 
        usernameValueLabel.AutoSize = true;
        usernameValueLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)204));
        usernameValueLabel.ForeColor = System.Drawing.Color.White;
        usernameValueLabel.Location = new System.Drawing.Point(30, 175);
        usernameValueLabel.Name = "usernameValueLabel";
        usernameValueLabel.Size = new System.Drawing.Size(51, 20);
        usernameValueLabel.TabIndex = 2;
        usernameValueLabel.Text = "label2";
        // 
        // editUsernameButton
        // 
        editUsernameButton.CustomizableEdges = customizableEdges2;
        editUsernameButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
        editUsernameButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
        editUsernameButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)((byte)169)), ((int)((byte)169)), ((int)((byte)169)));
        editUsernameButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)141)), ((int)((byte)141)), ((int)((byte)141)));
        editUsernameButton.FillColor = System.Drawing.Color.FromArgb(((int)((byte)45)), ((int)((byte)140)), ((int)((byte)240)));
        editUsernameButton.Font = new System.Drawing.Font("Segoe UI", 9F);
        editUsernameButton.ForeColor = System.Drawing.Color.White;
        editUsernameButton.Location = new System.Drawing.Point(260, 170);
        editUsernameButton.Name = "editUsernameButton";
        editUsernameButton.ShadowDecoration.CustomizableEdges = customizableEdges3;
        editUsernameButton.Size = new System.Drawing.Size(100, 30);
        editUsernameButton.TabIndex = 3;
        editUsernameButton.Text = "Изменить";
        // 
        // passwordLabel
        // 
        passwordLabel.AutoSize = true;
        passwordLabel.ForeColor = System.Drawing.Color.Gray;
        passwordLabel.Location = new System.Drawing.Point(30, 230);
        passwordLabel.Name = "passwordLabel";
        passwordLabel.Size = new System.Drawing.Size(57, 15);
        passwordLabel.TabIndex = 4;
        passwordLabel.Text = "Password";
        // 
        // passwordValueLabel
        // 
        passwordValueLabel.AutoSize = true;
        passwordValueLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)204));
        passwordValueLabel.ForeColor = System.Drawing.Color.White;
        passwordValueLabel.Location = new System.Drawing.Point(30, 255);
        passwordValueLabel.Name = "passwordValueLabel";
        passwordValueLabel.Size = new System.Drawing.Size(58, 20);
        passwordValueLabel.TabIndex = 5;
        passwordValueLabel.Text = "*******";
        // 
        // editPasswordButton
        // 
        editPasswordButton.CustomizableEdges = customizableEdges4;
        editPasswordButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
        editPasswordButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
        editPasswordButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)((byte)169)), ((int)((byte)169)), ((int)((byte)169)));
        editPasswordButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)141)), ((int)((byte)141)), ((int)((byte)141)));
        editPasswordButton.FillColor = System.Drawing.Color.FromArgb(((int)((byte)45)), ((int)((byte)140)), ((int)((byte)240)));
        editPasswordButton.Font = new System.Drawing.Font("Segoe UI", 9F);
        editPasswordButton.ForeColor = System.Drawing.Color.White;
        editPasswordButton.Location = new System.Drawing.Point(260, 250);
        editPasswordButton.Name = "editPasswordButton";
        editPasswordButton.ShadowDecoration.CustomizableEdges = customizableEdges5;
        editPasswordButton.Size = new System.Drawing.Size(100, 30);
        editPasswordButton.TabIndex = 6;
        editPasswordButton.Text = "Изменить";
        // 
        // ProfileSettingsDialogForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.Color.FromArgb(((int)((byte)23)), ((int)((byte)33)), ((int)((byte)43)));
        ClientSize = new System.Drawing.Size(384, 381);
        Controls.Add(editPasswordButton);
        Controls.Add(passwordValueLabel);
        Controls.Add(passwordLabel);
        Controls.Add(editUsernameButton);
        Controls.Add(usernameValueLabel);
        Controls.Add(usernameLabel);
        Controls.Add(avatar);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        Text = "Profile Settings";
        ((System.ComponentModel.ISupportInitialize)avatar).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Guna.UI2.WinForms.Guna2CirclePictureBox avatar;
    private System.Windows.Forms.Label usernameLabel;
    private System.Windows.Forms.Label usernameValueLabel;
    private Guna.UI2.WinForms.Guna2Button editUsernameButton;
    private System.Windows.Forms.Label passwordLabel;
    private System.Windows.Forms.Label passwordValueLabel;
    private Guna.UI2.WinForms.Guna2Button editPasswordButton;

    #endregion
}