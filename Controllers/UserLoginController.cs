using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Rotativa;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using FinalProject.Repository;


namespace FinalProject.Controllers
{
    public class UserLoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        // GET: Login
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(LoginClass login)
        {

            string mainconn = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            SqlCommand sqlcomm = new SqlCommand("SPS_FinalLogin", sqlconn);

            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Parameters.AddWithValue("@EmailAddress", login.EmailAddress);
            sqlcomm.Parameters.AddWithValue("@Password", login.Password);

            sqlconn.Open();
            SqlDataReader sqr = sqlcomm.ExecuteReader();

            if (sqr.Read())
            {
                FormsAuthentication.SetAuthCookie(login.EmailAddress, true);
                Session["emailid"] = login.EmailAddress.ToString();
                return RedirectToAction("UserHome", "Userlogin");
                

            }
            else
            {
                ViewData["message"] = "Username & Password are wrong !";
            }
            sqlconn.Close();
            return View();
        }

        public ActionResult Welcome()
        {
            string displayimg = (string)Session["emailid"];
            string mainconn = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string sqlquery = "select * from [dbo].[UserReg] where EmailAddress='" + displayimg + "'";
            sqlconn.Open();
            SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
            sqlcomm.Parameters.AddWithValue("EmailAddress", Session["emailid"].ToString());
            SqlDataReader sdr = sqlcomm.ExecuteReader();

            UserClass user = new UserClass();
            if (sdr.Read())
            {
                string image = sdr["Photo"].ToString();
                ViewData["Img"] = image;
                TempData["Oldimg"] = image;


                user.FirstName = sdr["FirstName"].ToString();
                user.LastName = sdr["LastName"].ToString();
                user.DateOfBirth = (DateTime)sdr["DateOfBirth"];
                user.Gender = sdr["Gender"].ToString();
                user.PhoneNumber = sdr["PhoneNumber"].ToString();
                user.EmailAddress = sdr["EmailAddress"].ToString();
                user.Address = sdr["Address"].ToString();
                user.Country = sdr["Country"].ToString();
                user.State = sdr["State"].ToString();
                user.City = sdr["City"].ToString();
                user.Postcode = sdr["Postcode"].ToString();
                user.PassportNumber = sdr["PassportNumber"].ToString();
                user.AdharNumber = sdr["AdharNumber"].ToString();
                user.Username = sdr["Username"].ToString();
                user.Password = sdr["Password"].ToString();

            }
            sqlconn.Close();
            return View(user);
        }






       
        public ActionResult Userimgchange(HttpPostedFileBase file)
        {
            var emailId = (string)Session["emailid"];

            string imgpath = Server.MapPath((string)TempData["Oldimg"]);
            string fileimgpath = imgpath;
            FileInfo fi = new FileInfo(fileimgpath);
            if (fi.Exists)
            {
                fi.Delete();
            }

            if (file != null && file.ContentLength > 0)
            {
                string filename = Path.GetFileName(file.FileName);
                string filepath = Path.Combine(Server.MapPath("/User-Images/"), filename);
                file.SaveAs(filepath);

                string mainconn = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
                using (SqlConnection sqlconn = new SqlConnection(mainconn))
                {
                    sqlconn.Open();
                    string sqlquery = "UPDATE [dbo].[UserReg] SET  Photo = @Photo WHERE EmailAddress = @EmailAddress";
                    SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
                    sqlcomm.Parameters.AddWithValue("@Photo", "/User-Images/" + filename);
                    sqlcomm.Parameters.AddWithValue("@EmailAddress", emailId);
                    sqlcomm.ExecuteNonQuery();

                }
            }

            return RedirectToAction("Welcome", "UserLogin");
        }


        // POST: Login/UpdateUser
        [HttpPost]
        [ActionName("UpdateUser")]
        public ActionResult UpdateUser(UserClass user)
        {
            string mainconn = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
            using (SqlConnection sqlconn = new SqlConnection(mainconn))
            {
                sqlconn.Open();
                SqlCommand sqlcommand = new SqlCommand("SPU_UpdateUser", sqlconn);
                sqlcommand.CommandType = CommandType.StoredProcedure;

                sqlcommand.Parameters.AddWithValue("@FirstName", user.FirstName);
                sqlcommand.Parameters.AddWithValue("@LastName", user.LastName);
                sqlcommand.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                sqlcommand.Parameters.AddWithValue("@Gender", user.Gender);
                sqlcommand.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                sqlcommand.Parameters.AddWithValue("@Address", user.Address);
                sqlcommand.Parameters.AddWithValue("@Country", user.Country);
                sqlcommand.Parameters.AddWithValue("@State", user.State);
                sqlcommand.Parameters.AddWithValue("@City", user.City);
                sqlcommand.Parameters.AddWithValue("@Postcode", user.Postcode);
                sqlcommand.Parameters.AddWithValue("@PassportNumber", user.PassportNumber);
                sqlcommand.Parameters.AddWithValue("@AdharNumber", user.AdharNumber);
                sqlcommand.Parameters.AddWithValue("@Username", user.Username);
                sqlcommand.Parameters.AddWithValue("@Password", user.Password);
                sqlcommand.Parameters.AddWithValue("@EmailAddress", user.EmailAddress);

                sqlcommand.ExecuteNonQuery();
            }

            return RedirectToAction("Welcome");
        }

        

        public ActionResult UserHome()
        {
            ViewBag.Message = "User Home page";

            return View();
        }
       
        public ActionResult UpdateUser()
        {
            ViewBag.Message = "Update User page";

            return View();
        }
        public ActionResult Error()
        {

            return View();
        }


        // GET: VisaApplication
        public ActionResult VisaApplication()
        {
            return View();
        }

        

        // Post: VisaApplication
        [HttpPost]
        public ActionResult VisaApplication(VisaApplication model, HttpPostedFileBase file)
        {
            string mainconn = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            SqlCommand sqlCommand = new SqlCommand("SPI_VisaApplication", sqlconn);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@FirstName", model.FirstName);
            sqlCommand.Parameters.AddWithValue("@LastName", model.LastName);
            sqlCommand.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth);
            sqlCommand.Parameters.AddWithValue("@EmailID", model.EmailID);
            sqlCommand.Parameters.AddWithValue("@Phone", model.Phone);
            sqlCommand.Parameters.AddWithValue("@Address", model.Address);
            sqlCommand.Parameters.AddWithValue("@ExpectedDateOfArrival", model.ExpectedDateOfArrival);
            sqlCommand.Parameters.AddWithValue("@ExpectedDateOfDeparture", model.ExpectedDateOfDeparture);
            sqlCommand.Parameters.AddWithValue("@VisaService", model.VisaService);
            sqlCommand.Parameters.AddWithValue("@Gender", model.Gender);
            sqlCommand.Parameters.AddWithValue("@TownCityOfBirth", model.TownCityOfBirth);
            sqlCommand.Parameters.AddWithValue("@CountryOfBirth", model.CountryOfBirth);
            sqlCommand.Parameters.AddWithValue("@CitizenshipNationalIdNo", model.CitizenshipNationalIdNo);
            sqlCommand.Parameters.AddWithValue("@Religion", model.Religion);
            sqlCommand.Parameters.AddWithValue("@EducationalQualification", model.EducationalQualification);
            sqlCommand.Parameters.AddWithValue("@PassportType", model.PassportType);
            sqlCommand.Parameters.AddWithValue("@Nationality", model.Nationality);
            sqlCommand.Parameters.AddWithValue("@PassportNumber", model.PassportNumber);
            sqlCommand.Parameters.AddWithValue("@PlaceOfIssue", model.PlaceOfIssue);
            sqlCommand.Parameters.AddWithValue("@DateOfIssue", model.DateOfIssue);
            sqlCommand.Parameters.AddWithValue("@DateOfExpiry", model.DateOfExpiry);
            sqlCommand.Parameters.AddWithValue("@PassportOrICNo", model.PassportOrICNo);
            sqlCommand.Parameters.AddWithValue("@PortOfArrival", model.PortOfArrival);
            sqlCommand.Parameters.AddWithValue("@ReferenceNameInIndia", model.ReferenceNameInIndia);
            sqlCommand.Parameters.AddWithValue("@ReferenceAddressInIndia", model.ReferenceAddressInIndia);
            sqlCommand.Parameters.AddWithValue("@ReferencePhone", model.ReferencePhone);

            if (file != null && file.ContentLength > 0)
            {

                string filename = Path.GetFileName(file.FileName);
                string imgpath = Path.Combine(Server.MapPath("/Visa-Images/"), filename);
                file.SaveAs(imgpath);
                sqlCommand.Parameters.AddWithValue("@Photo", "/Visa-Images/" + filename);
            }
            else
            {
                sqlCommand.Parameters.AddWithValue("@Photo", DBNull.Value);
            }
            sqlconn.Open();
            sqlCommand.ExecuteNonQuery();
            sqlconn.Close();

            ViewData["Message"] = "Visa Application for " + model.FirstName + " Is Saved Successfully!";
            return View();


        }


        // GET: Login
        public ActionResult CheckStatus()
        {
            return View();
        }

        // POST: Login
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckStatus(VisaStatus visaStatus)
        {
           
                string mainconn = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
                SqlConnection sqlconn = new SqlConnection(mainconn);
                SqlCommand sqlcomm = new SqlCommand("SPS_CheckVisaApplication", sqlconn);

                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@EmailID", visaStatus.EmailID);
                sqlcomm.Parameters.AddWithValue("@PassportNumber", visaStatus.PassportNumber);

                sqlconn.Open();
                SqlDataReader sqr = sqlcomm.ExecuteReader();

                if (sqr.Read())
                {
                    FormsAuthentication.SetAuthCookie(visaStatus.EmailID, true);
                    Session["emailid"] = visaStatus.EmailID.ToString();
                    return RedirectToAction("UserVisaStatus", "Userlogin");
                }
                else
                {
                    ViewData["message"] = "Email Id and Passport Number combination is incorrect!";
                }

                sqlconn.Close();
            return View();
        }

          
        

        /// <summary>
        /// To check visa status
        /// </summary>
        /// <returns></returns>
        public ActionResult UserVisaStatus()
        {
            string display = (string)Session["emailid"];
            string mainconn = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            SqlCommand sqlcommand = new SqlCommand("SPS_GetVisaApplicationStatus", sqlconn);
            sqlcommand.CommandType = CommandType.StoredProcedure;
            sqlcommand.Parameters.AddWithValue("@EmailID", Session["emailid"].ToString());
            sqlconn.Open();
            SqlDataReader sdr = sqlcommand.ExecuteReader();

            VisaApplication visaApplication = new VisaApplication();
            if (sdr.Read())
            {
                visaApplication.FirstName = sdr["FirstName"].ToString();
                visaApplication.LastName = sdr["LastName"].ToString();
                visaApplication.DateOfBirth = (DateTime)sdr["DateOfBirth"];
                visaApplication.EmailID = sdr["EmailID"].ToString();
                visaApplication.Phone = sdr["Phone"].ToString();
                visaApplication.Address = sdr["Address"].ToString();
                visaApplication.ExpectedDateOfArrival = (DateTime)sdr["ExpectedDateOfArrival"];
                visaApplication.ExpectedDateOfDeparture = (DateTime)sdr["ExpectedDateOfDeparture"];
                visaApplication.VisaService = sdr["VisaService"].ToString();
                visaApplication.Gender = sdr["Gender"].ToString();
                visaApplication.TownCityOfBirth = sdr["TownCityOfBirth"].ToString();
                visaApplication.CountryOfBirth = sdr["CountryOfBirth"].ToString();
                visaApplication.CitizenshipNationalIdNo = sdr["CitizenshipNationalIdNo"].ToString();
                visaApplication.Religion = sdr["Religion"].ToString();
                visaApplication.EducationalQualification = sdr["EducationalQualification"].ToString();
                visaApplication.PassportType = sdr["PassportType"].ToString();
                visaApplication.Nationality = sdr["Nationality"].ToString();
                visaApplication.PassportNumber = sdr["PassportNumber"].ToString();
                visaApplication.PlaceOfIssue = sdr["PlaceOfIssue"].ToString();
                visaApplication.DateOfIssue = (DateTime)sdr["DateOfIssue"];
                visaApplication.DateOfExpiry = (DateTime)sdr["DateOfExpiry"];
                visaApplication.PassportOrICNo = sdr["PassportOrICNo"].ToString();
                visaApplication.PortOfArrival = sdr["PortOfArrival"].ToString();
                visaApplication.ReferenceNameInIndia = sdr["ReferenceNameInIndia"].ToString();
                visaApplication.ReferenceAddressInIndia = sdr["ReferenceAddressInIndia"].ToString();
                visaApplication.ReferencePhone = sdr["ReferencePhone"].ToString();
                
                visaApplication.Status = sdr["Status"].ToString();
            }
            sqlconn.Close();
            return View(visaApplication);
        }


        



    }
}
