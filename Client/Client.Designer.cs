using System.Windows.Forms;

namespace Client
{
    partial class Client
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Client));
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Panel = new Guna.UI2.WinForms.Guna2Panel();
            this.panelParent = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Panel3 = new Guna.UI2.WinForms.Guna2Panel();
            this.txtBoxSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.companionSide = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Panel8 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Panel5 = new Guna.UI2.WinForms.Guna2Panel();
            this.companionPanel = new Guna.UI2.WinForms.Guna2Panel();
            this.lblCompanionUsername = new System.Windows.Forms.Label();
            this.guna2Panel9 = new Guna.UI2.WinForms.Guna2Panel();
            this.btnSndMsg = new Guna.UI2.WinForms.Guna2ImageButton();
            this.txtBoxMessage = new Guna.UI2.WinForms.Guna2TextBox();
            this.guna2Panel10 = new Guna.UI2.WinForms.Guna2Panel();
            this.btnOpenProfile = new Guna.UI2.WinForms.Guna2ImageButton();
            this.availableServers = new System.Windows.Forms.Label();
            this.btnUpdServers = new Guna.UI2.WinForms.Guna2Button();
            this.mySide = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Panel.SuspendLayout();
            this.guna2Panel3.SuspendLayout();
            this.companionPanel.SuspendLayout();
            this.guna2Panel9.SuspendLayout();
            this.guna2Panel10.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.Black;
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.guna2Panel1.Location = new System.Drawing.Point(75, 0);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(1, 681);
            this.guna2Panel1.TabIndex = 0;
            // 
            // guna2Panel
            // 
            this.guna2Panel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(33)))), ((int)(((byte)(43)))));
            this.guna2Panel.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.guna2Panel.Controls.Add(this.panelParent);
            this.guna2Panel.Controls.Add(this.guna2Panel3);
            this.guna2Panel.Dock = System.Windows.Forms.DockStyle.Left;
            this.guna2Panel.Location = new System.Drawing.Point(76, 0);
            this.guna2Panel.Name = "guna2Panel";
            this.guna2Panel.Size = new System.Drawing.Size(210, 681);
            this.guna2Panel.TabIndex = 3;
            // 
            // panelParent
            // 
            this.panelParent.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelParent.Location = new System.Drawing.Point(0, 58);
            this.panelParent.Name = "panelParent";
            this.panelParent.Size = new System.Drawing.Size(210, 623);
            this.panelParent.TabIndex = 11;
            // 
            // guna2Panel3
            // 
            this.guna2Panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(33)))), ((int)(((byte)(43)))));
            this.guna2Panel3.Controls.Add(this.txtBoxSearch);
            this.guna2Panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel3.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel3.Name = "guna2Panel3";
            this.guna2Panel3.Size = new System.Drawing.Size(210, 58);
            this.guna2Panel3.TabIndex = 0;
            // 
            // txtBoxSearch
            // 
            this.txtBoxSearch.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.txtBoxSearch.BorderRadius = 7;
            this.txtBoxSearch.BorderThickness = 0;
            this.txtBoxSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtBoxSearch.DefaultText = "";
            this.txtBoxSearch.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtBoxSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtBoxSearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtBoxSearch.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtBoxSearch.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(47)))), ((int)(((byte)(61)))));
            this.txtBoxSearch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtBoxSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtBoxSearch.ForeColor = System.Drawing.Color.White;
            this.txtBoxSearch.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtBoxSearch.Location = new System.Drawing.Point(6, 6);
            this.txtBoxSearch.Name = "txtBoxSearch";
            this.txtBoxSearch.PasswordChar = '\0';
            this.txtBoxSearch.PlaceholderText = "Поиск...";
            this.txtBoxSearch.SelectedText = "";
            this.txtBoxSearch.Size = new System.Drawing.Size(198, 43);
            this.txtBoxSearch.TabIndex = 1;
            this.txtBoxSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBoxSearch_KeyUp);
            // 
            // companionSide
            // 
            this.companionSide.Dock = System.Windows.Forms.DockStyle.Left;
            this.companionSide.Location = new System.Drawing.Point(287, 58);
            this.companionSide.Name = "companionSide";
            this.companionSide.Size = new System.Drawing.Size(227, 567);
            this.companionSide.TabIndex = 14;
            // 
            // guna2Panel8
            // 
            this.guna2Panel8.BackColor = System.Drawing.Color.Black;
            this.guna2Panel8.Dock = System.Windows.Forms.DockStyle.Left;
            this.guna2Panel8.Location = new System.Drawing.Point(286, 0);
            this.guna2Panel8.Name = "guna2Panel8";
            this.guna2Panel8.Size = new System.Drawing.Size(1, 681);
            this.guna2Panel8.TabIndex = 6;
            // 
            // guna2Panel5
            // 
            this.guna2Panel5.BackColor = System.Drawing.Color.Black;
            this.guna2Panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.guna2Panel5.Location = new System.Drawing.Point(1263, 0);
            this.guna2Panel5.Name = "guna2Panel5";
            this.guna2Panel5.Size = new System.Drawing.Size(1, 681);
            this.guna2Panel5.TabIndex = 7;
            // 
            // companionPanel
            // 
            this.companionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(33)))), ((int)(((byte)(43)))));
            this.companionPanel.Controls.Add(this.lblCompanionUsername);
            this.companionPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.companionPanel.Location = new System.Drawing.Point(287, 0);
            this.companionPanel.Name = "companionPanel";
            this.companionPanel.Size = new System.Drawing.Size(976, 58);
            this.companionPanel.TabIndex = 8;
            // 
            // lblCompanionUsername
            // 
            this.lblCompanionUsername.AutoSize = true;
            this.lblCompanionUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblCompanionUsername.ForeColor = System.Drawing.Color.White;
            this.lblCompanionUsername.Location = new System.Drawing.Point(6, 9);
            this.lblCompanionUsername.Name = "lblCompanionUsername";
            this.lblCompanionUsername.Size = new System.Drawing.Size(87, 20);
            this.lblCompanionUsername.TabIndex = 12;
            this.lblCompanionUsername.Text = "companion";
            // 
            // guna2Panel9
            // 
            this.guna2Panel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(33)))), ((int)(((byte)(43)))));
            this.guna2Panel9.Controls.Add(this.btnSndMsg);
            this.guna2Panel9.Controls.Add(this.txtBoxMessage);
            this.guna2Panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.guna2Panel9.Location = new System.Drawing.Point(287, 625);
            this.guna2Panel9.Name = "guna2Panel9";
            this.guna2Panel9.Size = new System.Drawing.Size(976, 56);
            this.guna2Panel9.TabIndex = 9;
            // 
            // btnSndMsg
            // 
            this.btnSndMsg.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.btnSndMsg.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSndMsg.HoverState.ImageSize = new System.Drawing.Size(32, 32);
            this.btnSndMsg.Image = ((System.Drawing.Image)(resources.GetObject("btnSndMsg.Image")));
            this.btnSndMsg.ImageOffset = new System.Drawing.Point(0, 0);
            this.btnSndMsg.ImageRotate = 0F;
            this.btnSndMsg.ImageSize = new System.Drawing.Size(32, 32);
            this.btnSndMsg.Location = new System.Drawing.Point(923, 0);
            this.btnSndMsg.Name = "btnSndMsg";
            this.btnSndMsg.PressedState.ImageSize = new System.Drawing.Size(32, 32);
            this.btnSndMsg.Size = new System.Drawing.Size(53, 56);
            this.btnSndMsg.TabIndex = 12;
            this.btnSndMsg.Click += new System.EventHandler(this.btnSndMsg_Click);
            // 
            // txtBoxMessage
            // 
            this.txtBoxMessage.BorderThickness = 0;
            this.txtBoxMessage.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtBoxMessage.DefaultText = "";
            this.txtBoxMessage.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtBoxMessage.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtBoxMessage.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtBoxMessage.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtBoxMessage.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(33)))), ((int)(((byte)(43)))));
            this.txtBoxMessage.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtBoxMessage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtBoxMessage.ForeColor = System.Drawing.Color.White;
            this.txtBoxMessage.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtBoxMessage.Location = new System.Drawing.Point(-1, 0);
            this.txtBoxMessage.Name = "txtBoxMessage";
            this.txtBoxMessage.PasswordChar = '\0';
            this.txtBoxMessage.PlaceholderText = "Введите сообщение...";
            this.txtBoxMessage.SelectedText = "";
            this.txtBoxMessage.Size = new System.Drawing.Size(918, 56);
            this.txtBoxMessage.TabIndex = 12;
            this.txtBoxMessage.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBoxMessage_KeyUp);
            // 
            // guna2Panel10
            // 
            this.guna2Panel10.Controls.Add(this.btnOpenProfile);
            this.guna2Panel10.Dock = System.Windows.Forms.DockStyle.Left;
            this.guna2Panel10.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel10.Name = "guna2Panel10";
            this.guna2Panel10.Size = new System.Drawing.Size(75, 681);
            this.guna2Panel10.TabIndex = 10;
            // 
            // btnOpenProfile
            // 
            this.btnOpenProfile.CheckedState.ImageSize = new System.Drawing.Size(40, 40);
            this.btnOpenProfile.HoverState.ImageSize = new System.Drawing.Size(40, 40);
            this.btnOpenProfile.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenProfile.Image")));
            this.btnOpenProfile.ImageOffset = new System.Drawing.Point(0, 0);
            this.btnOpenProfile.ImageRotate = 0F;
            this.btnOpenProfile.ImageSize = new System.Drawing.Size(40, 40);
            this.btnOpenProfile.Location = new System.Drawing.Point(5, 6);
            this.btnOpenProfile.Name = "btnOpenProfile";
            this.btnOpenProfile.PressedState.ImageSize = new System.Drawing.Size(40, 40);
            this.btnOpenProfile.Size = new System.Drawing.Size(64, 52);
            this.btnOpenProfile.TabIndex = 11;
            this.btnOpenProfile.Click += new System.EventHandler(this.btnOpenProfile_Click);
            // 
            // availableServers
            // 
            this.availableServers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.availableServers.AutoSize = true;
            this.availableServers.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.availableServers.ForeColor = System.Drawing.Color.White;
            this.availableServers.Location = new System.Drawing.Point(545, 235);
            this.availableServers.Name = "availableServers";
            this.availableServers.Size = new System.Drawing.Size(238, 24);
            this.availableServers.TabIndex = 12;
            this.availableServers.Text = "Нет доступных серверов";
            // 
            // btnUpdServers
            // 
            this.btnUpdServers.BorderRadius = 6;
            this.btnUpdServers.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnUpdServers.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnUpdServers.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnUpdServers.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnUpdServers.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(33)))), ((int)(((byte)(43)))));
            this.btnUpdServers.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnUpdServers.ForeColor = System.Drawing.Color.White;
            this.btnUpdServers.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(46)))), ((int)(((byte)(60)))));
            this.btnUpdServers.Location = new System.Drawing.Point(595, 282);
            this.btnUpdServers.Name = "btnUpdServers";
            this.btnUpdServers.Size = new System.Drawing.Size(121, 33);
            this.btnUpdServers.TabIndex = 13;
            this.btnUpdServers.Text = "Обновить";
            this.btnUpdServers.Click += new System.EventHandler(this.btnUpdServers_Click);
            // 
            // mySide
            // 
            this.mySide.Dock = System.Windows.Forms.DockStyle.Right;
            this.mySide.Location = new System.Drawing.Point(1036, 58);
            this.mySide.Name = "mySide";
            this.mySide.Size = new System.Drawing.Size(227, 567);
            this.mySide.TabIndex = 15;
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(22)))), ((int)(((byte)(33)))));
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.companionSide);
            this.Controls.Add(this.mySide);
            this.Controls.Add(this.btnUpdServers);
            this.Controls.Add(this.availableServers);
            this.Controls.Add(this.guna2Panel9);
            this.Controls.Add(this.companionPanel);
            this.Controls.Add(this.guna2Panel5);
            this.Controls.Add(this.guna2Panel8);
            this.Controls.Add(this.guna2Panel);
            this.Controls.Add(this.guna2Panel1);
            this.Controls.Add(this.guna2Panel10);
            this.Name = "Client";
            this.Text = "Messenger";
            this.guna2Panel.ResumeLayout(false);
            this.guna2Panel3.ResumeLayout(false);
            this.companionPanel.ResumeLayout(false);
            this.companionPanel.PerformLayout();
            this.guna2Panel9.ResumeLayout(false);
            this.guna2Panel10.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
            this.FormClosing += this.Client_FormClosing;
        }

        #endregion
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel3;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel8;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel5;
        private Guna.UI2.WinForms.Guna2Panel companionPanel;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel9;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel10;
        private Guna.UI2.WinForms.Guna2ImageButton btnOpenProfile;
        private Guna.UI2.WinForms.Guna2TextBox txtBoxMessage;
        private Guna.UI2.WinForms.Guna2ImageButton btnSndMsg;
        private Guna.UI2.WinForms.Guna2TextBox txtBoxSearch;
        private System.Windows.Forms.Label lblCompanionUsername;
        private Guna.UI2.WinForms.Guna2Panel panelParent;
        private System.Windows.Forms.Label availableServers;
        private Guna.UI2.WinForms.Guna2Button btnUpdServers;
        private Guna.UI2.WinForms.Guna2Panel companionSide;
        private Guna.UI2.WinForms.Guna2Panel mySide;
    }
}

