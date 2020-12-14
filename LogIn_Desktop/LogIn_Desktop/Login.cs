using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace LogIn_Desktop
{
    public partial class FmrLogin : Form
    {
        private Connection connection;
        private SqlParameter paramEmail;
        private SqlParameter paramPass;
        private SqlParameter paramName;
        private SqlParameter paramFullname;
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
            paramName = new SqlParameter();
            paramFullname = new SqlParameter();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (!RegisterMode)
            {
                DoLogin(txtEmail.Text, txtPassword.Text);
            }
            else
            {
                RegisterUser(txtFullName.Text, txtEmail.Text, txtPassword.Text);
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
            Query.Parameters.Clear();

            if (Id.Count == 1)
            {
                return Id[0];
            }
            else return 0;
        }

        private void RegisterUser(string Fullname, string Email, string Pass)
        {
            string Sql = " INSERT INTO Users (Name, FullName, Email, Password)" +
                         " VALUES (@name, @fullname, @email, @password); ";
            SqlCommand Query = connection.Query(Sql);

            // Valid Vars
            string ValidFullName = "";
            string ValidName     = "";
            string ValidEmail    = "";
            string ValidPass     = "";

            // Validate Names
            int NamesCount = Fullname.ToString().Trim().Count(x => x == ' ');
            if (NamesCount < 0)           
            {
                ValidFullName   = Fullname;
                ValidName       = Fullname.Substring(0, Fullname.IndexOf(' '));
            }
            
            // Validate Email
            ValidEmail = Email;

            // Validate Pass
            ValidPass = Pass;

            // Fill Params
            paramName.ParameterName = "@name";
            paramName.Value = ValidName;

            paramFullname.ParameterName = "@fullname";
            paramFullname.Value = ValidFullName;

            paramEmail.ParameterName = "@email";
            paramEmail.Value = Email;

            paramPass.ParameterName = "@password";
            paramPass.Value = ValidPass;
            
            // Add Params
            Query.Parameters.Add(paramName);
            Query.Parameters.Add(paramFullname);
            Query.Parameters.Add(paramEmail);
            Query.Parameters.Add(paramPass);

            // Validate Unique User

            // Insert Into Database
            int InsertedLines = 
                Query.ExecuteNonQuery();

            // Validate Inserted Lines
            if (InsertedLines <= 0) MessageBox.Show("Error inserting data into Database.");
            
            Query.Parameters.Clear();
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
