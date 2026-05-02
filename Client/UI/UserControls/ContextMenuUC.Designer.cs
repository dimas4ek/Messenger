using System.ComponentModel;

namespace Client.UI.UserControls;

partial class ContextMenuUC
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
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        menuPanel = new Guna.UI2.WinForms.Guna2Panel();
        deleteButton = new Guna.UI2.WinForms.Guna2Button();
        editButton = new Guna.UI2.WinForms.Guna2Button();
        menuPanel.SuspendLayout();
        SuspendLayout();
        // 
        // menuPanel
        // 
        menuPanel.BorderColor = System.Drawing.Color.FromArgb(((int)((byte)50)), ((int)((byte)63)), ((int)((byte)76)));
        menuPanel.BorderRadius = 8;
        menuPanel.BorderThickness = 1;
        menuPanel.Controls.Add(deleteButton);
        menuPanel.Controls.Add(editButton);
        menuPanel.CustomizableEdges = customizableEdges5;
        menuPanel.Dock = System.Windows.Forms.DockStyle.Fill;
        menuPanel.ForeColor = System.Drawing.Color.White;
        menuPanel.Location = new System.Drawing.Point(0, 0);
        menuPanel.Name = "menuPanel";
        menuPanel.ShadowDecoration.CustomizableEdges = customizableEdges6;
        menuPanel.Size = new System.Drawing.Size(160, 80);
        menuPanel.TabIndex = 0;
        // 
        // deleteButton
        // 
        deleteButton.BorderRadius = 6;
        deleteButton.CustomizableEdges = customizableEdges1;
        deleteButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
        deleteButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
        deleteButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)((byte)169)), ((int)((byte)169)), ((int)((byte)169)));
        deleteButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)141)), ((int)((byte)141)), ((int)((byte)141)));
        deleteButton.FillColor = System.Drawing.Color.Transparent;
        deleteButton.Font = new System.Drawing.Font("Segoe UI", 9F);
        deleteButton.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)220)), ((int)((byte)80)), ((int)((byte)80)));
        deleteButton.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)((byte)45)), ((int)((byte)58)), ((int)((byte)71)));
        deleteButton.Location = new System.Drawing.Point(5, 42);
        deleteButton.Name = "deleteButton";
        deleteButton.ShadowDecoration.CustomizableEdges = customizableEdges2;
        deleteButton.Size = new System.Drawing.Size(150, 32);
        deleteButton.TabIndex = 1;
        deleteButton.Text = "Удалить";
        // 
        // editButton
        // 
        editButton.BorderRadius = 6;
        editButton.CustomizableEdges = customizableEdges3;
        editButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
        editButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
        editButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)((byte)169)), ((int)((byte)169)), ((int)((byte)169)));
        editButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)141)), ((int)((byte)141)), ((int)((byte)141)));
        editButton.FillColor = System.Drawing.Color.Transparent;
        editButton.Font = new System.Drawing.Font("Segoe UI", 9F);
        editButton.ForeColor = System.Drawing.Color.White;
        editButton.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)((byte)45)), ((int)((byte)58)), ((int)((byte)71)));
        editButton.Location = new System.Drawing.Point(5, 5);
        editButton.Name = "editButton";
        editButton.ShadowDecoration.CustomizableEdges = customizableEdges4;
        editButton.Size = new System.Drawing.Size(150, 32);
        editButton.TabIndex = 0;
        editButton.Text = "Изменить";
        // 
        // ContextMenuUC
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.Color.FromArgb(((int)((byte)30)), ((int)((byte)43)), ((int)((byte)56)));
        Controls.Add(menuPanel);
        Size = new System.Drawing.Size(160, 80);
        menuPanel.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Guna.UI2.WinForms.Guna2Button editButton;
    private Guna.UI2.WinForms.Guna2Button deleteButton;

    private Guna.UI2.WinForms.Guna2Panel menuPanel;

    #endregion
}