using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using System.Web.Helpers;


namespace JQGrid4U.BL
{

	public class User
	{

		public int ID { get; set; }
		public string Pwd { get; set; }
		public string FirstName { get; set; }
		public string SurName { get; set; }
		public string EmailAdd { get; set; }
		public int UserLevel { get; set; }
		public Boolean AutoEmailAlert { get; set; }
		public string Mobileno { get; set; }
		public Boolean Disabled { get; set; }


	}


	public class UserBusinessLogic
	{
		string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
		SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);


		public IEnumerable<User> Users
		{
			get
			{
				List<User> Users = new List<User>();
				using (SqlConnection conObj = new SqlConnection(conStr))
				{
					SqlCommand cmdObj = new SqlCommand("select * from tblUser", conObj);
					conObj.Open();
					SqlDataReader readerObj = cmdObj.ExecuteReader();

					while (readerObj.Read())
					{
						User User = new User();
						User.ID = Convert.ToInt32(readerObj["ID"]);
						//User.UserId = Convert.ToInt32(readerObj["userid"]);
						User.Pwd = "xxxxxxxxxx"; //readerObj["pwd"].ToString();
						User.FirstName = readerObj["firstname"].ToString();
						User.SurName = readerObj["surname"].ToString();
						User.EmailAdd = readerObj["emailAdd"].ToString();
						User.UserLevel = Convert.ToInt32(readerObj["UserLevel"].ToString());
						User.AutoEmailAlert = Convert.ToBoolean(readerObj["autoemailalert"].ToString());
						//User.Country = readerObj["country"].ToString();
						User.Mobileno = readerObj["Mobileno"].ToString();
						User.Disabled = Convert.ToBoolean(readerObj["disabled"].ToString());
						Users.Add(User);
					}
				}
				return Users;
			}
		}

		public string pfNameToPwd(string yName)
		{
			string vName = "";
			string pwd = "";
			int xlen = yName.Length - 1;
			for (int i = 0; i <= xlen; i++)
			{
				vName = yName.Substring(i, 1);

				pwd = pwd + CharToNum(vName);
			}
			return (pwd.Substring(0, 10));

		}

		public string CharToNum(string yChar)
		{
			string vret = "";
			int xnum = 0;
			switch (yChar)
			{
				case "A":
					vret = "11";
					break;
				case "a":
					vret = "11";
					break;
				case "B":
				case "b":
					vret = "12";
					break;
				case "C":
				case "c":
					vret = "13";
					break;
				case "D":
				case "d":
					vret = "14";
					break;
				case "E":
				case "e":
					vret = "15";
					break;
				case "F":
				case "f":
					vret = "16";
					break;
				case "G":
				case "g":
					vret = "17";
					break;
				case "H":
				case "h":
					vret = "18";
					break;
				case "I":
				case "i":
					vret = "19";
					break;
				case "J":
				case "j":
					vret = "20";
					break;
				case "K":
				case "k":
					vret = "21";
					break;
				case "L":
				case "l":
					vret = "22";
					break;
				case "M":
				case "m":
					vret = "23";
					break;
				case "N":
				case "n":
					vret = "24";
					break;
				case "O":
				case "o":
					vret = "25";
					break;
				case "P":
				case "p":
					vret = "26";
					break;
				case "Q":
				case "q":
					vret = "27";
					break;
				case "R":
				case "r":
					vret = "28";
					break;
				case "S":
				case "s":
					vret = "29";
					break;
				case "T":
				case "t":
					vret = "30";
					break;
				case "U":
				case "u":
					vret = "31";
					break;

				case "V":
				case "v":
					vret = "32";
					break;

				case "W":
				case "w":
					vret = "33";
					break;
				case "X":
				case "x":
					vret = "34";
					break;
				case "Y":
				case "y":
					vret = "35";
					break;
				case "Z":
				case "z":
					vret = "36";
					break;

			}

			return vret;


		}


		public string Puz1(string pwd, string fname)
		{
			int xlen = pwd.Length;
			string p1 = pwd.Substring(0, 3);
			string p2 = pwd.Substring(3, xlen - 3);
			string str1 = "";
			str1 = p1 + fname.Substring(0, 1).ToUpper() + p2;
			return str1;
		}

		public int InsertUser(User User)
		{
			int xlen = 0;
			string baseName = User.SurName;
			baseName = baseName.Trim() + User.FirstName;
			xlen = baseName.Length;
			baseName = baseName.Substring(0, xlen);
			string origpwd = "";
			string newpwd = pfNameToPwd(baseName);
			origpwd = newpwd;
			newpwd = Crypto.SHA1(newpwd);
			newpwd = Puz1(newpwd, baseName);

			using (SqlConnection conObj = new SqlConnection(conStr))
			{
				SqlCommand cmdObj = new SqlCommand("uspInsertUser", conObj);
				cmdObj.CommandType = CommandType.StoredProcedure;
				//	cmdObj.Parameters.Add(new SqlParameter("@UserId", User.UserId));
				cmdObj.Parameters.Add(new SqlParameter("@EmailAdd", User.EmailAdd));
				cmdObj.Parameters.Add(new SqlParameter("@Pwd", newpwd));
				cmdObj.Parameters.Add(new SqlParameter("@Firstname", User.FirstName));
				cmdObj.Parameters.Add(new SqlParameter("@Surname", User.SurName));
				cmdObj.Parameters.Add(new SqlParameter("@Userlevel", User.UserLevel));
				cmdObj.Parameters.Add(new SqlParameter("@AutoEmailAlert", User.AutoEmailAlert));
				//cmdObj.Parameters.Add(new SqlParameter("@country", User.Country));
				cmdObj.Parameters.Add(new SqlParameter("@Mobileno", User.Mobileno));
				cmdObj.Parameters.Add(new SqlParameter("@Disabled", User.Disabled));

				try
				{
					conObj.Open();
					UserSendMail(User.EmailAdd, origpwd);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
				return Convert.ToInt32(cmdObj.ExecuteScalar());
			}
		}

		public void UserSendMail(string yMailto, string yPwd)
		{

			string vbody = "New User Password : " + yPwd;
			MailMessage mail = new MailMessage();
			mail.From = new MailAddress("haroldtoralba@gmail.com");
			//mail.Sender = new MailAddress("haroldtoralba@gmail.com");
			//mail.To.Add("alan.sepe@delonix.com.au");
			mail.IsBodyHtml = true;
			mail.Subject = "New User Password";
			mail.Body = vbody;
			mail.To.Add(yMailto);

			SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
			smtp.UseDefaultCredentials = false;

			smtp.Credentials = new System.Net.NetworkCredential("haroldtoralba@gmail.com", "sepealan123");
			smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
			smtp.EnableSsl = true;
			smtp.Timeout = 30000;
			smtp.Send(mail);

		}

		public int UpdatePwd(string pUser, string pPwd)
		{

			using (SqlConnection conObj = new SqlConnection(conStr))
			{
				SqlCommand cmdObj = new SqlCommand("update tblUser set pwd='" + pPwd + "' where emailAdd='" + pUser + "'", conObj);
				conObj.Open();
				return Convert.ToInt32(cmdObj.ExecuteNonQuery());
			}


		}


		public int UpdateDisabled(string pUser)
		{

			using (SqlConnection conObj = new SqlConnection(conStr))
			{
				SqlCommand cmdObj = new SqlCommand("update tblUser set disabled='true' where emailAdd='" + pUser + "'", conObj);
				conObj.Open();
				return Convert.ToInt32(cmdObj.ExecuteNonQuery());
			}


		}


		public int UpdateUser(User User)
		{
			using (SqlConnection conObj = new SqlConnection(conStr))
			{
				conObj.Open();
				SqlCommand cmdObj = new SqlCommand("uspUpdateUser", conObj);
				cmdObj.CommandType = CommandType.StoredProcedure;
				cmdObj.Parameters.Add(new SqlParameter("@ID", User.ID));
				//cmdObj.Parameters.Add(new SqlParameter("@UserId", User.UserId));
				cmdObj.Parameters.Add(new SqlParameter("@Firstname", User.FirstName));
				cmdObj.Parameters.Add(new SqlParameter("@Surname", User.SurName));
				cmdObj.Parameters.Add(new SqlParameter("@EmailAdd", User.EmailAdd));
				cmdObj.Parameters.Add(new SqlParameter("@UserLevel", User.UserLevel));
				cmdObj.Parameters.Add(new SqlParameter("@AutoEmailAlert", User.AutoEmailAlert));
				cmdObj.Parameters.Add(new SqlParameter("@Pwd", User.Pwd));
				cmdObj.Parameters.Add(new SqlParameter("@Mobileno", User.Mobileno));
				cmdObj.Parameters.Add(new SqlParameter("@Disabled", User.Disabled));
				//conObj.Close();
				return Convert.ToInt32(cmdObj.ExecuteScalar());
			}
		}





		public int UserLevel(string yUser)
		{

			int vRetX = 1;
			using (SqlConnection conObj = new SqlConnection(conStr))
			{

				string qry = "select userlevel from tblUser where emailadd='" + yUser + "'";
				SqlCommand cmdObj = new SqlCommand(qry, conObj);
				conObj.Open();
				SqlDataReader readerObj = cmdObj.ExecuteReader();

				while (readerObj.Read())
				{
					vRetX = Convert.ToInt32(readerObj["userlevel"].ToString());
				}
				cmdObj.Dispose();
				readerObj.Close();

			}
			return vRetX;

		}


		public Boolean isAcctDisabled(string yUser, string Pwd)
		{
			Boolean vRet = false;
			using (SqlConnection conObj = new SqlConnection(conStr))
			{

				string qry = "select * from tblUser where emailadd='" + yUser + "' and pwd='" + Pwd + "' and disabled='true'";
				SqlCommand cmdObj = new SqlCommand(qry, conObj);
				conObj.Open();
				SqlDataReader readerObj = cmdObj.ExecuteReader();

				while (readerObj.Read())
				{
					vRet = true;
				}
				cmdObj.Dispose();
				readerObj.Close();

			}
			return vRet;
		}

		public Boolean isValidUser(string yUser, string Pwd)
		{
			Boolean vRet = false;
			using (SqlConnection conObj = new SqlConnection(conStr))
			{

				string qry = "select * from tblUser where emailadd='" + yUser + "' and  ( substring(pwd,1,3) + substring(pwd,5,76) ) ='" + Pwd + "'";
				SqlCommand cmdObj = new SqlCommand(qry, conObj);
				conObj.Open();
				SqlDataReader readerObj = cmdObj.ExecuteReader();

				while (readerObj.Read())
				{
					vRet = true;
				}
				cmdObj.Dispose();
				readerObj.Close();

			}
			return vRet;
		}


		public int DeleteUser(int ID)
		{
			using (SqlConnection conObj = new SqlConnection(conStr))
			{
				SqlCommand cmdObj = new SqlCommand("uspDeleteUser", conObj);
				conObj.Open();
				cmdObj.CommandType = CommandType.StoredProcedure;
				cmdObj.Parameters.Add(new SqlParameter("@ID", ID));
				
				return Convert.ToInt32(cmdObj.ExecuteScalar());
			}
		}


		

	}





}