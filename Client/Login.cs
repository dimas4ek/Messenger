using Database;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Client
{
    public partial class Login : Form
    {
        int loginSwitch = 0;

        public Login()
        {
            InitializeComponent();

            lblAccount.Text = "У вас нет аккаунта?\nЗарегистрируйтесь!";
            txtPassword.PasswordChar = '*';
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            LoginToAccount();
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoginToAccount();
            }
        }

        private void LoginToAccount()
        {
            var dbManager = new DatabaseManager();

            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (loginSwitch == 0)
            {
                if (!DatabaseManager.CheckIfUserExists(username))
                {
                    MessageBox.Show("Ошибка входа в аккаунт");
                }
                else
                {
                    if (DatabaseManager.LoginUser(username, password))
                    {
                        Client client = new Client();
                        client.Show();

                        Hide();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка входа в аккаунт");
                    }
                }
            }

            if (loginSwitch == 1)
            {
                if (DatabaseManager.CheckIfUserExists(username))
                {
                    MessageBox.Show("Имя уже занято");
                }
                else
                {
                    if (username != "" && password != "")
                    {
                        DatabaseManager.RegisterUser(username, password);
                    }
                    else
                    {
                        MessageBox.Show("Имя или пароль не должны быть пустыми");
                    }

                    ChangeText();
                }
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            ChangeText();
        }

        private void ChangeText()
        {
            switch (loginSwitch)
            {
                case 1:
                    lblLogin.Text = "Вход";
                    lblLogin.Location = new Point(94, 22);
                    btnLogin.Text = "Войти";
                    lblAccount.Text = "У вас нет аккаунта?\nЗарегистрируйтесь!";
                    lblAccount.Location = new Point(68, 218);
                    btnRegister.Text = "Регистрация";

                    loginSwitch = 0;
                    
                    break;
                case 0:
                    lblLogin.Text = "Регистрация";
                    lblLogin.Location = new Point(61, 22);
                    btnLogin.Text = "Регистрация";
                    lblAccount.Text = "У вас уже есть аккаунт?\nВойдите!";
                    lblAccount.Location = new Point(56, 218);
                    btnRegister.Text = "Вход";

                    loginSwitch = 1;
                    
                    break;
            }
            
            lblAccount.TextAlign = ContentAlignment.MiddleCenter;
            txtUsername.Clear();
            txtPassword.Clear();
        }
    }
}
