using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;

namespace Assignment1
{
    public partial class Registration : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int value = 1 / int.Parse("0");
            }
            catch (Exception ex)
            {
                Response.Write("HelpLink = {0}" + ex.HelpLink + "<br>");
                Response.Write("Message = {0}" + ex.Message + "<br>");
                Response.Write("Source = {0}" + ex.Source + "<br>");
                Response.Write("StackTrace = {0}" + ex.StackTrace + "<br>");
                Response.Write("TargetSite = {0}" + ex.TargetSite + "<br>");
            }
        }

        protected int checkPassword(string password)
        {
            int score = 0;
            if (password.Length < 8)
            {
                return 1;
            }
            else
            {
                score = 1;
            }

            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }

            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }

            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }

            if (Regex.IsMatch(password, "[!@#$%^&*()<>,./?]"))
            {
                score++;
            }

            return score;
        }

        protected void checkpwd_Click(object sender, EventArgs e)
        {
            // implement codes for the button event
            // Extract data from textbox
            int scores = checkPassword(tb_password.Text);
            string status = "";
            switch (scores)
            {
                case 1:
                    status = "Very Weak";
                    break;
                case 2:
                    status = "Weak";
                    break;
                case 3:
                    status = "Medium";
                    break;
                case 4:
                    status = "Strong";
                    break;
                case 5:
                    status = "Excellent";
                    break;
                default:
                    break;
            }
            lbl_pwdchecker.Visible = true;
            lbl_pwdchecker.Text = "Status : " + status;
            if (scores < 4)
            {
                lbl_pwdchecker.ForeColor = Color.Red;
                return;
            }
            lbl_pwdchecker.ForeColor = Color.Blue;
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            tb_email.Text = HttpUtility.HtmlEncode(tb_email.Text);
            tb_firstname.Text = HttpUtility.HtmlEncode(tb_firstname.Text);
            tb_lastname.Text = HttpUtility.HtmlEncode(tb_lastname.Text);
            tb_dob.Text = HttpUtility.HtmlEncode(tb_dob.Text);
            tb_password.Text = HttpUtility.HtmlEncode(tb_password.Text);
            tb_creditcard.Text = HttpUtility.HtmlEncode(tb_creditcard.Text);

            //string pwd = get value from your Textbox
            string pwd = tb_password.Text.ToString().Trim();

            //Generate random "salt"
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltByte = new byte[8];

            //Fills array of bytes with a cryptographically strong sequence of random values.
            rng.GetBytes(saltByte);
            salt = Convert.ToBase64String(saltByte);
            SHA512Managed hashing = new SHA512Managed();
            string pwdWithSalt = pwd + salt;
            byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
            finalHash = Convert.ToBase64String(hashWithSalt);
            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;
            createAccount();
        }

        public void createAccount()
        {
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@Email, @FirstName, @LastName, @DateOfBirth, @PasswordSalt, @PasswordHash, @CreditCard)"))
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter())
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@Email", tb_email.Text.Trim());
                                cmd.Parameters.AddWithValue("@FirstName", tb_firstname.Text.Trim());
                                cmd.Parameters.AddWithValue("@LastName", tb_lastname.Text.Trim());
                                cmd.Parameters.AddWithValue("@DateOfBirth", tb_dob.Text.Trim());
                                cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                                cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                                cmd.Parameters.AddWithValue("@CreditCard", encryptData(tb_creditcard.Text.Trim()));

                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    // lbl_error.Text = "Please enter valid information! XSS has been prevented.";
                    throw new Exception(ex.ToString());
                }
            }
        }

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0,
               plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }

        public bool checkValidation()
        {
            bool valid = false;
            if (tb_creditcard.Text.Length < 16)
            {
                error_creditcard.Text = "Please enter a valid credit card number!";
                error_creditcard.Visible = true;
                error_creditcard.ForeColor = Color.Red;
            }

            if (tb_dob.Text.Length < 8)
            {
                error_dob.Text = "Please enter a valid date of birth!";
                error_dob.Visible = true;
                error_dob.ForeColor = Color.Red;
            }
            
            else
            {
                valid = true;
            }
            return valid;
        }
    }
}