using System.ComponentModel;

namespace Client.UI.UserControls;

partial class MessagePanelUC
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
        messagePanel = new Guna.UI2.WinForms.Guna2Panel();
        messageLabel = new System.Windows.Forms.Label();
        nameLabel = new System.Windows.Forms.Label();
        messagePanel.SuspendLayout();
        SuspendLayout();
        // 
        // messagePanel
        // 
        messagePanel.AutoSize = true;
        messagePanel.Controls.Add(messageLabel);
        messagePanel.Controls.Add(nameLabel);
        messagePanel.CustomizableEdges = customizableEdges1;
        messagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
        messagePanel.Location = new System.Drawing.Point(0, 0);
        messagePanel.MaximumSize = new System.Drawing.Size(400, 0);
        messagePanel.Name = "messagePanel";
        messagePanel.ShadowDecoration.CustomizableEdges = customizableEdges2;
        messagePanel.Size = new System.Drawing.Size(150, 100);
        messagePanel.TabIndex = 0;
        // 
        // messageLabel
        // 
        messageLabel.AutoSize = true;
        messageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
        messageLabel.ForeColor = System.Drawing.Color.White;
        messageLabel.Location = new System.Drawing.Point(0, 20);
        messageLabel.MaximumSize = new System.Drawing.Size(380, 0);
        messageLabel.Name = "messageLabel";
        messageLabel.Size = new System.Drawing.Size(0, 20);
        messageLabel.TabIndex = 1;
        // 
        // nameLabel
        // 
        nameLabel.AutoSize = true;
        nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
        nameLabel.ForeColor = System.Drawing.Color.LightGray;
        nameLabel.Location = new System.Drawing.Point(0, 0);
        nameLabel.Name = "nameLabel";
        nameLabel.Size = new System.Drawing.Size(0, 15);
        nameLabel.TabIndex = 0;
        // 
        // MessagePanelFactoryUserControl
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        AutoSize = true;
        BackColor = System.Drawing.Color.FromArgb(((int)((byte)24)), ((int)((byte)37)), ((int)((byte)51)));
        Controls.Add(messagePanel);
        MaximumSize = new System.Drawing.Size(400, 0);
        Size = new System.Drawing.Size(150, 100);
        messagePanel.ResumeLayout(false);
        messagePanel.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Label nameLabel;
    private System.Windows.Forms.Label messageLabel;

    private Guna.UI2.WinForms.Guna2Panel messagePanel;

    #endregion
}