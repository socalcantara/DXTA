using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data; 

namespace JQGrid4U.BL.Repositories
{

  public class User
  {

    public int ID { get; set; }
    public string UserId { get; set; }
    public string Pwd { get; set; }
    public string UserName { get; set; }
    public string Yearmo { get; set; }
    public int UserLevel { get; set; }

  }


  public class UserBusinessLogic
  {
    string conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
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
            User.UserId = readerObj["userid"].ToString();
            User.Pwd = readerObj["pwd"].ToString();
            User.UserName = readerObj["username"].ToString();
            User.Yearmo = readerObj["yearmo"].ToString();
            Users.Add(User);
          }
        }
        return Users;
      }
    }

    public int InsertUser(User User)
    {
      using (SqlConnection conObj = new SqlConnection(conStr))
      {
        SqlCommand cmdObj = new SqlCommand("uspInsertUser", conObj);
        cmdObj.CommandType = CommandType.StoredProcedure;
        cmdObj.Parameters.Add(new SqlParameter("@Yearmo", User.Yearmo));
        cmdObj.Parameters.Add(new SqlParameter("@Userid", User.UserId));
        cmdObj.Parameters.Add(new SqlParameter("@Pwd", User.Pwd));
        cmdObj.Parameters.Add(new SqlParameter("@Username", User.UserName));
        cmdObj.Parameters.Add(new SqlParameter("@Userlevel", User.UserLevel));
        conObj.Open();
        return Convert.ToInt32(cmdObj.ExecuteScalar());
      }
    }

    public int UpdateUser(User User)
    {
      using (SqlConnection conObj = new SqlConnection(conStr))
      {
        SqlCommand cmdObj = new SqlCommand("uspUpdateUser", conObj);
        cmdObj.CommandType = CommandType.StoredProcedure;
        cmdObj.Parameters.Add(new SqlParameter("@ID", User.ID));
        cmdObj.Parameters.Add(new SqlParameter("@UserId", User.UserId));
        cmdObj.Parameters.Add(new SqlParameter("@Pwd", User.Pwd));
        cmdObj.Parameters.Add(new SqlParameter("@Username", User.UserName));
        cmdObj.Parameters.Add(new SqlParameter("@Yearmo", User.Yearmo));
        cmdObj.Parameters.Add(new SqlParameter("@Level", User.UserLevel));
        conObj.Open();
        return Convert.ToInt32(cmdObj.ExecuteScalar());
      }
    }


    

    public Boolean isValidUser(string yUser, string Pwd)
    {
      Boolean vRet = false;
      using (SqlConnection conObj = new SqlConnection(conStr))
      {

        string qry = "select * from tblUser where userid='" + yUser + "' and pwd='" + Pwd + "'";
        SqlCommand cmdObj = new SqlCommand(qry, conObj);
        conObj.Open();
        SqlDataReader readerObj = cmdObj.ExecuteReader();

        while (readerObj.Read())
        {
          vRet = true;
        }
        readerObj.Close();
      }
      return vRet;
    }


    public int DeleteUser(int ID)
    {
      using (SqlConnection conObj = new SqlConnection(conStr))
      {
        SqlCommand cmdObj = new SqlCommand("uspDeleteUser", conObj);
        cmdObj.CommandType = CommandType.StoredProcedure;
        cmdObj.Parameters.Add(new SqlParameter("@ID", ID));
        conObj.Open();
        return Convert.ToInt32(cmdObj.ExecuteScalar());
      }
    }
  }




}
