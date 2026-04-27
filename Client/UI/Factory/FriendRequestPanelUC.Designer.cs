using System.ComponentModel;

namespace Client.UI.Factory;

sealed partial class FriendRequestPanelUC
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
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        requestLabel = new System.Windows.Forms.Label();
        acceptButton = new Guna.UI2.WinForms.Guna2Button();
        declineButton = new Guna.UI2.WinForms.Guna2Button();
        requestPanel = new Guna.UI2.WinForms.Guna2Panel();
        requestPanel.SuspendLayout();
        SuspendLayout();
        // 
        // requestLabel
        // 
        requestLabel.AutoSize = true;
        requestLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
        requestLabel.ForeColor = System.Drawing.Color.White;
        requestLabel.Location = new System.Drawing.Point(6, 25);
        requestLabel.Name = "requestLabel";
        requestLabel.Size = new System.Drawing.Size(0, 20);
        requestLabel.TabIndex = 0;
        // 
        // acceptButton
        // 
        acceptButton.CustomizableEdges = customizableEdges1;
        acceptButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
        acceptButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
        acceptButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)((byte)169)), ((int)((byte)169)), ((int)((byte)169)));
        acceptButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)141)), ((int)((byte)141)), ((int)((byte)141)));
        acceptButton.FillColor = System.Drawing.Color.FromArgb(((int)((byte)39)), ((int)((byte)174)), ((int)((byte)96)));
        acceptButton.Font = new System.Drawing.Font("Segoe UI", 9F);
        acceptButton.ForeColor = System.Drawing.Color.White;
        acceptButton.Location = new System.Drawing.Point(270, 20);
        acceptButton.Name = "acceptButton";
        acceptButton.ShadowDecoration.CustomizableEdges = customizableEdges2;
        acceptButton.Size = new System.Drawing.Size(40, 30);
        acceptButton.TabIndex = 1;
        acceptButton.Text = "✓";
        // 
        // declineButton
        // 
        declineButton.CustomizableEdges = customizableEdges3;
        declineButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
        declineButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
        declineButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)((byte)169)), ((int)((byte)169)), ((int)((byte)169)));
        declineButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)141)), ((int)((byte)141)), ((int)((byte)141)));
        declineButton.FillColor = System.Drawing.Color.FromArgb(((int)((byte)192)), ((int)((byte)57)), ((int)((byte)43)));
        declineButton.Font = new System.Drawing.Font("Segoe UI", 9F);
        declineButton.ForeColor = System.Drawing.Color.White;
        declineButton.Location = new System.Drawing.Point(315, 20);
        declineButton.Name = "declineButton";
        declineButton.ShadowDecoration.CustomizableEdges = customizableEdges4;
        declineButton.Size = new System.Drawing.Size(40, 30);
        declineButton.TabIndex = 2;
        declineButton.Text = "✗";
        // 
        // requestPanel
        // 
        requestPanel.Controls.Add(declineButton);
        requestPanel.Controls.Add(acceptButton);
        requestPanel.Controls.Add(requestLabel);
        requestPanel.CustomizableEdges = customizableEdges5;
        requestPanel.Dock = System.Windows.Forms.DockStyle.Top;
        requestPanel.Location = new System.Drawing.Point(0, 0);
        requestPanel.Name = "requestPanel";
        requestPanel.ShadowDecoration.CustomizableEdges = customizableEdges6;
        requestPanel.Size = new System.Drawing.Size(360, 70);
        requestPanel.TabIndex = 3;
        // 
        // FriendRequestPanelFactoryUserControl
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.Color.FromArgb(((int)((byte)23)), ((int)((byte)33)), ((int)((byte)43)));
        Controls.Add(requestPanel);
        Size = new System.Drawing.Size(360, 70);
        requestPanel.ResumeLayout(false);
        requestPanel.PerformLayout();
        ResumeLayout(false);
    }

    private Guna.UI2.WinForms.Guna2Panel requestPanel;

    private System.Windows.Forms.Label requestLabel;
    private Guna.UI2.WinForms.Guna2Button acceptButton;
    private Guna.UI2.WinForms.Guna2Button declineButton;

    #endregion
}