using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LogIn_Desktop
{
    public partial class FmrLogin : Form
    {
        private Connection connection;
        private SqlParameter paramEmail;
        private SqlParameter paramPass;
        private bool RegisterMode = false;

        public FmrLogin()
        {
            InitializeComponent();
        }

        private void FmrLogin_Load(object sender, EventArgs e)
        {
            connection = new Connection();
            paramEmail = new SqlParameter();
            paramPass = new SqlParameter();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (!RegisterMode)
            {
                DoLogin(txtEmail.Text, txtPassword.Text);
            }
            else
            {
                // Salvar no banco
            }
        }

        private void LblNew_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AlterLoginRegisterMode();
        }

        private int DoLogin(string Email, string Pass)
        {
            String Sql = "SELECT * FROM Users WHERE Email = @email AND Password = @password";
            SqlCommand Query = connection.Query(Sql);

            paramEmail.ParameterName = "@email";
            paramEmail.Value = Email;
            Query.Parameters.Add(paramEmail);

            paramPass.ParameterName = "@password";
            paramPass.Value = Pass;
            Query.Parameters.Add(paramPass);

            var Id = new List<int>();
            Id.Clear();
            SqlDataReader reader = Query.ExecuteReader();
            while (reader.Read())
            {
                Id.Add(Convert.ToInt32(reader["ID"]));
            }

            String Message;
            switch (Id.Count)
            {
                case 0:
                    Message = "Nenhum usuário encontrado. Verifique se as credenciais foram inseridas corretamente!";
                    break;

                case 1:
                    Message = "Usuário encontrado!";
                    break;

                default:
                    Message = "Erro encontrado no momento de login!";
                    break;
            }

            MessageBox.Show(Message);

            if (Id.Count == 1)
            {
                return Id[0];
            }
            else return 0;
        }

        private void AlterLoginRegisterMode()
        {
            RegisterMode = !RegisterMode;
            if (RegisterMode)
            {
                lblNew.Text = "Voltar";
                lblNew.Left += 35;
                btnLogin.Text = "Registrar";
            }
            else
            {
                lblNew.Text = "Cadastrar nova conta";
                lblNew.Left -= 35;
                btnLogin.Text = "Login";
                txtFullName.Text = "";
            }
            lblFullName.Visible = RegisterMode;
            txtFullName.Visible = RegisterMode;
        }

    }
}
