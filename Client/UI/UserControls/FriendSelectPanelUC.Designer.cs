using System.ComponentModel;

namespace Client.UI.UserControls;

sealed partial class FriendSelectPanelUC
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
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        friendPanel = new Guna.UI2.WinForms.Guna2Panel();
        addButton = new Guna.UI2.WinForms.Guna2Button();
        friendLabel = new System.Windows.Forms.Label();
        friendPanel.SuspendLayout();
        SuspendLayout();
        // 
        // friendPanel
        // 
        friendPanel.Controls.Add(friendLabel);
        friendPanel.Controls.Add(addButton);
        friendPanel.CustomizableEdges = customizableEdges3;
        friendPanel.Dock = System.Windows.Forms.DockStyle.Fill;
        friendPanel.Location = new System.Drawing.Point(0, 0);
        friendPanel.Name = "friendPanel";
        friendPanel.ShadowDecoration.CustomizableEdges = customizableEdges4;
        friendPanel.Size = new System.Drawing.Size(330, 60);
        friendPanel.TabIndex = 0;
        // 
        // addButton
        // 
        addButton.CustomizableEdges = customizableEdges1;
        addButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
        addButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
        addButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)((byte)169)), ((int)((byte)169)), ((int)((byte)169)));
        addButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)141)), ((int)((byte)141)), ((int)((byte)141)));
        addButton.FillColor = System.Drawing.Color.FromArgb(((int)((byte)114)), ((int)((byte)137)), ((int)((byte)218)));
        addButton.Font = new System.Drawing.Font("Segoe UI", 9F);
        addButton.ForeColor = System.Drawing.Color.White;
        addButton.Location = new System.Drawing.Point(250, 15);
        addButton.Name = "addButton";
        addButton.ShadowDecoration.CustomizableEdges = customizableEdges2;
        addButton.Size = new System.Drawing.Size(70, 30);
        addButton.TabIndex = 0;
        addButton.Text = "Add";
        // 
        // friendLabel
        // 
        friendLabel.AutoSize = true;
        friendLabel.ForeColor = System.Drawing.Color.White;
        friendLabel.Location = new System.Drawing.Point(6, 18);
        friendLabel.Name = "friendLabel";
        friendLabel.Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 12);
        friendLabel.Size = new System.Drawing.Size(0, 15);
        friendLabel.TabIndex = 1;
        // 
        // FriendSelectPanelFactoryUserControl
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.Color.FromArgb(((int)((byte)23)), ((int)((byte)33)), ((int)((byte)43)));
        Controls.Add(friendPanel);
        Size = new System.Drawing.Size(330, 60);
        friendPanel.ResumeLayout(false);
        friendPanel.PerformLayout();
        ResumeLayout(false);
    }

    private Guna.UI2.WinForms.Guna2Button addButton;
    private System.Windows.Forms.Label friendLabel;

    private Guna.UI2.WinForms.Guna2Panel friendPanel;

    #endregion
}