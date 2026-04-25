using System.ComponentModel;

namespace Client.Forms.Dialogs;

partial class InputDialog
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
        textBox = new Guna.UI2.WinForms.Guna2TextBox();
        confirmButton = new Guna.UI2.WinForms.Guna2Button();
        SuspendLayout();
        // 
        // textBox
        // 
        textBox.Cursor = System.Windows.Forms.Cursors.IBeam;
        textBox.CustomizableEdges = customizableEdges1;
        textBox.DefaultText = "";
        textBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)((byte)208)), ((int)((byte)208)), ((int)((byte)208)));
        textBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)((byte)226)), ((int)((byte)226)), ((int)((byte)226)));
        textBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)138)), ((int)((byte)138)), ((int)((byte)138)));
        textBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)((byte)138)), ((int)((byte)138)), ((int)((byte)138)));
        textBox.FillColor = System.Drawing.Color.FromArgb(((int)((byte)30)), ((int)((byte)43)), ((int)((byte)56)));
        textBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)((byte)94)), ((int)((byte)148)), ((int)((byte)255)));
        textBox.Font = new System.Drawing.Font("Segoe UI", 9F);
        textBox.ForeColor = System.Drawing.Color.White;
        textBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)((byte)94)), ((int)((byte)148)), ((int)((byte)255)));
        textBox.Location = new System.Drawing.Point(10, 10);
        textBox.Name = "textBox";
        textBox.PlaceholderText = "";
        textBox.SelectedText = "";
        textBox.ShadowDecoration.CustomizableEdges = customizableEdges2;
        textBox.Size = new System.Drawing.Size(360, 40);
        textBox.TabIndex = 0;
        // 
        // confirmButton
        // 
        confirmButton.CustomizableEdges = customizableEdges3;
        confirmButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
        confirmButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
        confirmButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)((byte)169)), ((int)((byte)169)), ((int)((byte)169)));
        confirmButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)141)), ((int)((byte)141)), ((int)((byte)141)));
        confirmButton.FillColor = System.Drawing.Color.FromArgb(((int)((byte)45)), ((int)((byte)140)), ((int)((byte)240)));
        confirmButton.Font = new System.Drawing.Font("Segoe UI", 9F);
        confirmButton.ForeColor = System.Drawing.Color.White;
        confirmButton.Location = new System.Drawing.Point(270, 70);
        confirmButton.Name = "confirmButton";
        confirmButton.ShadowDecoration.CustomizableEdges = customizableEdges4;
        confirmButton.Size = new System.Drawing.Size(100, 35);
        confirmButton.TabIndex = 1;
        confirmButton.Text = "Сохранить";
        // 
        // InputDialogForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.Color.FromArgb(((int)((byte)23)), ((int)((byte)33)), ((int)((byte)43)));
        ClientSize = new System.Drawing.Size(384, 131);
        Controls.Add(confirmButton);
        Controls.Add(textBox);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        Text = "InputDialogForm";
        ResumeLayout(false);
    }

    private Guna.UI2.WinForms.Guna2Button confirmButton;

    private Guna.UI2.WinForms.Guna2TextBox textBox;

    #endregion
}