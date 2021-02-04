using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace Assignment1
{
    public partial class Login : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            if (ValidateCaptcha())
            {
                string pwd = tb_password.Text.ToString().Trim();
                string userid = tb_userid.Text.ToString().Trim();
                SHA512Managed hashing = new SHA512Managed();
                string dbHash = getDBHash(userid);
                string dbSalt = getDBSalt(userid);
                try
                {
                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                    {
                        string pwdWithSalt = pwd + dbSalt;
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                        string userHash = Convert.ToBase64String(hashWithSalt);
                        if (userHash.Equals(dbHash))
                        {
                            Session["LoggedIn"] = tb_userid.Text.Trim();
                            // create a new GUID and save into session
                            string guid = Guid.NewGuid().ToString();
                            Session["AuthToken"] = guid;

                            // create a new cookie with guid value
                            Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                            Response.Redirect("Homepage.aspx", false);
                        }
                    }
                    else
                    {
                        // Response.Redirect("Login.aspx", false);
                        lbl_error.Visible = true;
                        lbl_error.Text = "Userid or password is not valid. Please try again.";
                        lbl_error.ForeColor = Color.Red;
                        tb_userid.Text = "";
                        tb_password.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally { }
            }
        }

        protected string getDBHash(string userid)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash FROM Account WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return h;
        }

        protected string getDBSalt(string userid)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PASSWORDSALT FROM ACCOUNT WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != null)
                        {
                            if (reader["PASSWORDSALT"] != DBNull.Value)
                            {
                                s = reader["PASSWORDSALT"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return s;
        }

         public bool ValidateCaptcha()
        {
            bool result = true;

            // when user submits form, user gets a POST parameter
            // captchaResponse consists of the user click pattern
            string captchaResponse = Request.Form["g-recaptcha-response"];

            // to send a GET request to Google along with response and secret key
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6LceN0caAAAAAP3gku-OUjJGHd2uySsq6PSrXq-u &response= " + captchaResponse);

            try
            {
                // codes to receive response in JSON format from Google server
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        // response in JSON format
                        string jsonResponse = readStream.ReadToEnd();

                        // show JSON resposne string for learning purposes
                        lbl_gScore.Visible = true;
                        lbl_gScore.Text = "Error message: " + jsonResponse.ToString();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        // create jsonObject to handle response
                        // deserialize json
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

                        // convert string "false" to bool false or "true" to bool true
                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }

                return result;
            } 
            catch (WebException ex)
            {
                throw ex;
            }
        }

        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }
    }
}