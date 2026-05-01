using System.ComponentModel;

namespace Client.UI.UserControls;

sealed partial class ChatPanelUC
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
        chatPanel = new Guna.UI2.WinForms.Guna2Panel();
        statusDot = new Client.UI.Utils.CirclePanel();
        chatLabel = new System.Windows.Forms.Label();
        chatImage = new Guna.UI2.WinForms.Guna2CirclePictureBox();
        chatPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)chatImage).BeginInit();
        SuspendLayout();
        // 
        // chatPanel
        // 
        chatPanel.BackColor = System.Drawing.Color.FromArgb(((int)((byte)23)), ((int)((byte)33)), ((int)((byte)43)));
        chatPanel.Controls.Add(statusDot);
        chatPanel.Controls.Add(chatLabel);
        chatPanel.Controls.Add(chatImage);
        chatPanel.CustomizableEdges = customizableEdges2;
        chatPanel.Location = new System.Drawing.Point(0, 0);
        chatPanel.Name = "chatPanel";
        chatPanel.ShadowDecoration.CustomizableEdges = customizableEdges3;
        chatPanel.Size = new System.Drawing.Size(245, 70);
        chatPanel.TabIndex = 0;
        // 
        // statusDot
        // 
        statusDot.BackColor = System.Drawing.Color.Transparent;
        statusDot.Location = new System.Drawing.Point(82, 46);
        statusDot.Name = "statusDot";
        statusDot.Size = new System.Drawing.Size(14, 14);
        statusDot.TabIndex = 2;
        // 
        // chatLabel
        // 
        chatLabel.AutoSize = true;
        chatLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
        chatLabel.ForeColor = System.Drawing.Color.White;
        chatLabel.Location = new System.Drawing.Point(0, 0);
        chatLabel.Name = "chatLabel";
        chatLabel.Size = new System.Drawing.Size(0, 20);
        chatLabel.TabIndex = 1;
        // 
        // chatImage
        // 
        chatImage.BackColor = System.Drawing.Color.Transparent;
        chatImage.ImageRotate = 0F;
        chatImage.Location = new System.Drawing.Point(6, 10);
        chatImage.Name = "chatImage";
        chatImage.ShadowDecoration.CustomizableEdges = customizableEdges1;
        chatImage.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
        chatImage.Size = new System.Drawing.Size(50, 50);
        chatImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        chatImage.TabIndex = 0;
        chatImage.TabStop = false;
        // 
        // ChatPanelFactoryUserControl
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        Controls.Add(chatPanel);
        Location = new System.Drawing.Point(15, 15);
        Size = new System.Drawing.Size(245, 70);
        chatPanel.ResumeLayout(false);
        chatPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)chatImage).EndInit();
        ResumeLayout(false);
    }

    private Client.UI.Utils.CirclePanel statusDot;

    private System.Windows.Forms.Label chatLabel;

    private Guna.UI2.WinForms.Guna2CirclePictureBox chatImage;

    private Guna.UI2.WinForms.Guna2Panel chatPanel;

    #endregion
}