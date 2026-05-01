using System.ComponentModel;

namespace Client.UI.UserControls;

sealed partial class FriendPanelUC
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
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        friendPanel = new Guna.UI2.WinForms.Guna2Panel();
        friendAvatar = new Guna.UI2.WinForms.Guna2CirclePictureBox();
        friendLabel = new System.Windows.Forms.Label();
        friendPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)friendAvatar).BeginInit();
        SuspendLayout();
        // 
        // friendPanel
        // 
        friendPanel.Controls.Add(friendAvatar);
        friendPanel.Controls.Add(friendLabel);
        friendPanel.CustomizableEdges = customizableEdges2;
        friendPanel.Dock = System.Windows.Forms.DockStyle.Fill;
        friendPanel.Location = new System.Drawing.Point(0, 0);
        friendPanel.Name = "friendPanel";
        friendPanel.ShadowDecoration.CustomizableEdges = customizableEdges3;
        friendPanel.Size = new System.Drawing.Size(300, 70);
        friendPanel.TabIndex = 0;
        // 
        // friendAvatar
        // 
        friendAvatar.BackColor = System.Drawing.Color.Transparent;
        friendAvatar.ImageRotate = 0F;
        friendAvatar.Location = new System.Drawing.Point(15, 10);
        friendAvatar.Name = "friendAvatar";
        friendAvatar.ShadowDecoration.CustomizableEdges = customizableEdges1;
        friendAvatar.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
        friendAvatar.Size = new System.Drawing.Size(50, 50);
        friendAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        friendAvatar.TabIndex = 2;
        friendAvatar.TabStop = false;
        // 
        // friendLabel
        // 
        friendLabel.AutoSize = true;
        friendLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
        friendLabel.ForeColor = System.Drawing.Color.White;
        friendLabel.Location = new System.Drawing.Point(70, 18);
        friendLabel.Name = "friendLabel";
        friendLabel.Size = new System.Drawing.Size(0, 20);
        friendLabel.TabIndex = 1;
        // 
        // FriendPanelUC
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.Color.FromArgb(((int)((byte)23)), ((int)((byte)33)), ((int)((byte)43)));
        Controls.Add(friendPanel);
        Size = new System.Drawing.Size(300, 70);
        friendPanel.ResumeLayout(false);
        friendPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)friendAvatar).EndInit();
        ResumeLayout(false);
    }

    private Guna.UI2.WinForms.Guna2CirclePictureBox friendAvatar;

    private Guna.UI2.WinForms.Guna2Panel friendPanel;
    private System.Windows.Forms.Label friendLabel;

    #endregion
}