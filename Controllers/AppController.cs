using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using RakTDAApi.BLL.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RakTDAApi.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RAKHolidayHomesBL;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using System.Configuration;
using System.Globalization;

namespace RakTDAApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AppController(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;

            _env = env;
            BLL.Common.General.dbConStr = _config.GetConnectionString("DBConnection").ToString();
        }
        private readonly IWebHostEnvironment _env;

      

        #region JWT Token Builder
        //private string BuildToken(string UType, string Name, string UserID, string Email, bool IsEmailVerified = false, DataTable UserProfile = null)
        //{
        //    var claims = new[]
        //    {
        //        new Claim("UType",UType),
        //        new Claim("UserID", UserID),
        //        new Claim("Email", Email),
        //        new Claim("Name", Name),
        //        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
        //    };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTSettings:Secretkey"]));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        _config["JWTSettings:Issuer"],
        //        _config["JWTSettings:Audience"],
        //        claims,
        //        expires: DateTime.Now.AddDays(1), // Set Expiry
        //        signingCredentials: creds);
        //    return new JwtSecurityTokenHandler().WriteToken(token);

        //}
        #endregion


        #region JWT Token Builder FOR ADMIN
        private string BuildToken(string AdminUserID, string AdminUser)
        {
            var claims = new[]
            {
                     new Claim("AdminUserID",AdminUserID),
                     new Claim("AdminUser", AdminUser),
                     new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTSettings:Secretkey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["JWTSettings:Issuer"],
                _config["JWTSettings:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1), // Set Expiry
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        #endregion

        [HttpPost]
        public IActionResult AdminLogin([FromBody] Models.Payloads.Login login)
        {
            clsOwner objowner = new clsOwner();
            DataTable dtlogin = new DataTable();
            dtlogin = objowner.AdminLoginCheck(login.userName, EncryptDecryptHelper.Encrypt(login.password));

            if (dtlogin != null && dtlogin.Rows.Count > 0)
            {
                dynamic rstJSON = new System.Dynamic.ExpandoObject();
                rstJSON.Session = General.Dt2JSON(dtlogin);

                var tokenString = BuildToken(dtlogin.Rows[0]["AdminUserID"].ToString(),
                                             dtlogin.Rows[0]["AdminUser"].ToString());


                //#region ActivityLog Add;

                #region ActivityLog Add

                decimal userid = 0;
                string username = string.Empty, usertype = string.Empty;
                userid = Convert.ToInt32(dtlogin.Rows[0]["AdminUserID"].ToString());
                username = dtlogin.Rows[0]["AdminUser"].ToString();
                usertype = dtlogin.Rows[0]["AdminType"].ToString();
                clsActivityLog objActivityLog = new clsActivityLog();
                objActivityLog.Form = "AdminPanel/Login";
                objActivityLog.Action = "Login";
                objActivityLog.Activity = "Login";
                objActivityLog.Description = "Login Success";
                objActivityLog.Status = "Success";
                objActivityLog.Parameters = "User Name:" + username + ",Admin Type:" + usertype;
                objActivityLog.MasterId = userid;
                objActivityLog.MasterItem = username;
                objActivityLog.MasterType = "tbl_AdminUsers";
                objActivityLog.UserID = userid;
                objActivityLog.IsAdmin = true;
                objActivityLog.UserType = usertype;
                objActivityLog.InsertActivityLog();

                #endregion//ActivityLog Add

                return Ok(new { statusCode = 200, data = new { config = rstJSON, token = tokenString } });
            }
            else
            {
                return Ok(new { statusCode = 400, data = new { data = "Invalid Login Credentials." } });

                #region ActivityLog Add

                clsActivityLog objActivityLog = new clsActivityLog();
                objActivityLog.Form = "AdminPanel/Default";
                objActivityLog.Action = "Login";
                objActivityLog.Activity = "Login";
                objActivityLog.Description = "Login Failed";
                objActivityLog.Status = "Failed";
                objActivityLog.Parameters = "User Name:" + "";
                objActivityLog.MasterId = 0;
                objActivityLog.MasterItem = string.Empty;
                objActivityLog.MasterType = "tbl_AdminUsers";
                objActivityLog.UserID = 0;
                objActivityLog.IsAdmin = true;
                objActivityLog.UserType = string.Empty;
                objActivityLog.InsertActivityLog();

                #endregion//ActivityLog Add
            }

        }


        [HttpPost]
        [Authorize]
        [RequestSizeLimit(500 * 1024 * 1024)]       //unit is bytes => 500Mb
        [RequestFormLimits(MultipartBodyLengthLimit = 500 * 1024 * 1024)]
        public IActionResult LoadAssignedList()
        {
            clsOwner objowner = new clsOwner();
            dynamic rstJSON = new System.Dynamic.ExpandoObject();
            var UserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "AdminUserID").Value;
            int userId = int.Parse(UserId);
            DataTable dtAssignedList = objowner.LoadAssignedList(userId);
            rstJSON.AssignedList = General.Dt2JSON(dtAssignedList);
            if (dtAssignedList.Rows.Count > 0)
            {


                return Ok(new { statusCode = 200, data = new { data = General.Dt2JSON(dtAssignedList) } });
            }

            else
            {
                return Ok(new { statusCode = 400, data = new { data = General.Dt2JSON(dtAssignedList) } });

            }

        }

        [HttpPost]
        [Authorize]
        [RequestSizeLimit(500 * 1024 * 1024)]       //unit is bytes => 500Mb
        [RequestFormLimits(MultipartBodyLengthLimit = 500 * 1024 * 1024)]
        public IActionResult ScheduledList()
        {
            clsOwner objowner = new clsOwner();
            dynamic rstJSON = new System.Dynamic.ExpandoObject();
            var UserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "AdminUserID").Value;
            int userId = int.Parse(UserId);
            DataTable dtScheduledList = objowner.ScheduledList(userId);
            rstJSON.ScheduledList = General.Dt2JSON(dtScheduledList);     
            if (dtScheduledList.Rows.Count > 0)
            {


                return Ok(new { statusCode = 200, data = new { data = General.Dt2JSON(dtScheduledList) } });
            }

            else
            {
                return Ok(new { statusCode = 400, data = new { data = General.Dt2JSON(dtScheduledList) } });

            }

        }

        [HttpPost]
        [Authorize]
        [RequestSizeLimit(500 * 1024 * 1024)]       //unit is bytes => 500Mb
        [RequestFormLimits(MultipartBodyLengthLimit = 500 * 1024 * 1024)]
        public IActionResult VisitedHistory()
        {
            clsOwner objowner = new clsOwner();
            dynamic rstJSON = new System.Dynamic.ExpandoObject();
            var UserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "AdminUserID").Value;
            int userId = int.Parse(UserId);
            DataTable dtVistiedHistory = objowner.LoadVisitedList(userId);
            rstJSON.VisitedHistory = General.Dt2JSON(dtVistiedHistory);
            if (dtVistiedHistory.Rows.Count > 0)
            {


                return Ok(new { statusCode = 200, data = new { data = General.Dt2JSON(dtVistiedHistory) } });
            }

            else
            {
                return Ok(new { statusCode = 400, data = new { data = General.Dt2JSON(dtVistiedHistory) } });

            }

        }


        [HttpPost]
        [Authorize]
        [RequestSizeLimit(500 * 1024 * 1024)]       //unit is bytes => 500Mb
        [RequestFormLimits(MultipartBodyLengthLimit = 500 * 1024 * 1024)]
        public IActionResult ReScheduledList()
        {
            clsOwner objowner = new clsOwner();
            dynamic rstJSON = new System.Dynamic.ExpandoObject();
            var UserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "AdminUserID").Value;
            int userId = int.Parse(UserId);
            DataTable dtReScheduledList = objowner.ReScheduledList(userId);
            rstJSON.ReScheduledList = General.Dt2JSON(dtReScheduledList);
            if (dtReScheduledList.Rows.Count > 0)
            {


                return Ok(new { statusCode = 200, data = new { data = General.Dt2JSON(dtReScheduledList) } });
            }

            else
            {
                return Ok(new { statusCode = 400, data = new { data = General.Dt2JSON(dtReScheduledList) } });

            }

        }


        [HttpPost]
        [RequestSizeLimit(500 * 1024 * 1024)]       //unit is bytes => 500Mb
        [RequestFormLimits(MultipartBodyLengthLimit = 500 * 1024 * 1024)]
        public IActionResult ScheduleSiteVisit([FromBody] Models.Payloads.SiteVisit siteVisit)
        {
            clsOwner objowner = new clsOwner();

            DateTime dt = DateTime.ParseExact(siteVisit.date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            int rst = objowner.ScheduleSiteVisit(dt, siteVisit.unitId, siteVisit.time);
            
            if (rst == 1)
            {  //need to send email notifications to client and admin;
                return Ok(new { statusCode = 200, data = new { msg = "Successfully Scheduled Site Visit" } });
            }
            else
            {
                return Ok(new { statusCode = 400, data = new { data = "Invalid Operation Scheduled date should not be less than current date." } });
            }
        }

        [HttpPost]
        [Authorize]
        [RequestSizeLimit(500 * 1024 * 1024)]       //unit is bytes => 500Mb
        [RequestFormLimits(MultipartBodyLengthLimit = 500 * 1024 * 1024)]
        public IActionResult LoadForInspection([FromBody] Models.Payloads.LoadCriteria criteria)
        {
            clsOwner objowner = new clsOwner();
            DataTable rst = objowner.LoadForInspection(criteria.UnitId, criteria.UCId);

            if (rst.Rows.Count > 0)
            {
                return Ok(new { statusCode = 200, data = new { data = General.Dt2JSON(rst), 
                    msg = "Successfully Loaded Data" } });
            }
            else
            {
                return Ok(new { statusCode = 400, data = new { data = General.Dt2JSON(rst) } });
            }
        }

        [HttpPost]
        [Authorize]
        [RequestSizeLimit(500 * 1024 * 1024)]       //unit is bytes => 500Mb
        [RequestFormLimits(MultipartBodyLengthLimit = 500 * 1024 * 1024)]
        public IActionResult InspectedList([FromBody] Models.Payloads.LoadCriteria criteria)
        {
            clsOwner objowner = new clsOwner();
            DataTable rst = objowner.InspectedList(criteria.UnitId, criteria.UCId);
            
            if (rst != null&&rst.Rows.Count > 0)
            {
                return Ok(new
                {
                    statusCode = 200,
                    data = new
                    {
                        data = General.Dt2JSON(rst),
                        msg = "Successfully Loaded Data"
                    }
                });
            }
            else
            {
                return Ok(new { statusCode = 400, data = new { data = General.Dt2JSON(rst) } });
            }
        }



        [HttpPost]
        [RequestSizeLimit(500 * 1024 * 1024)]       //unit is bytes => 500Mb
        [RequestFormLimits(MultipartBodyLengthLimit = 500 * 1024 * 1024)]
        public IActionResult InspectionReport ([FromBody] Models.Payloads.LoadCriteria criteria)
        {
            clsOwner objowner = new clsOwner();
            DataTable rst = objowner.InspectedList(criteria.UnitId, criteria.UCId);

            if (rst != null)
            {

                return Ok(new
                {
                    statusCode = 200,
                    data = new
                    {
                        data = General.Dt2JSON(rst),
                        msg = "Successfully Loaded Data"
                    }
                });
            }
            else
            {
                return Ok(new { statusCode = 400, data = new { data = General.Dt2JSON(rst) } });
            }
        }



        [HttpPost]
        [Authorize]
        [RequestSizeLimit(500 * 1024 * 1024)]       //unit is bytes => 500Mb
        [RequestFormLimits(MultipartBodyLengthLimit = 500 * 1024 * 1024)]
        public IActionResult InsertCheckListResults([FromBody] Models.Payloads.CheckListResults checkList)
        {
            clsOwner objowner = new clsOwner(); bool brst = false;
            var UserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "AdminUserID").Value;
            checkList.UserId = int.Parse(UserId);
            checkList.MasterId = checkList.UcId+DateTime.Now.ToString("ddMMyyy");
            checkList.ClassificationTypeId = 0;
            if (checkList.Result == 0)
            {
                brst = false;
            }else if (checkList.Result == 1)
            {
                brst = true;
            }else if (checkList.Result == 2)
            {
                checkList.Remarks = "Not Applicable" + checkList.Remarks;
            }
            var webroot=(string)AppDomain.CurrentDomain.GetData("WebRootPath");
            string path = Path.Combine(webroot, "InspectedImages\\"+checkList.UcId);
            var contentRootPath = (string)AppDomain.CurrentDomain.GetData("ContentRootPath");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (checkList.Image != null || checkList.Image != "")
            {
                checkList.Image = checkList.UcId.ToString()+"/"+ clsOwner.SaveImage(checkList.Image, path, checkList.UcId.ToString(), checkList.IndexCode.ToString(), checkList.UnitId.ToString());
            }
            int rst = objowner.InsertCheckListResults(checkList.UnitId, checkList.PropertyTypeId,
                                                        checkList.ClassificationTypeId,
                                                        checkList.IndexCode,
                                                        brst,
                                                        checkList.MasterId,
                                                        checkList.UserId,
                                                        checkList.UcId,
                                                        checkList.Remarks,
                                                        checkList.Image);

            if (rst > 0)
            {
                return Ok(new { statusCode = 200, data = new { msg = "Successfully Inserted CheckList" } });
            }
            else
            {
                return Ok(new { statusCode = 400, data = new { data = "Invalid Operation." } });
            }
        }

        [HttpPost]
        [Authorize]
        [RequestSizeLimit(500 * 1024 * 1024)]       //unit is bytes => 500Mb
        [RequestFormLimits(MultipartBodyLengthLimit = 500 * 1024 * 1024)]
        public IActionResult CompleteInspection([FromBody] Models.Payloads.SiteCompletion siteCompletion)
        {
            clsOwner objowner = new clsOwner();
            int rst = objowner.SiteVisitCompleted(siteCompletion.UnitId, siteCompletion.Comments, siteCompletion.UCId);
            objowner.updateUnitnotificationadmin(siteCompletion.UnitId, 1);

            if (rst > 0)
            {
                return Ok(new { statusCode = 200, data = new { msg = "Successfully Completed Inspection" } });
            }
            else
            {
                return Ok(new { statusCode = 400, data = new { data = "Invalid Operation." } });
            }
        }








        [HttpPost]
        [Authorize]
        [RequestSizeLimit(500 * 1024 * 1024)]       //unit is bytes => 500Mb
        [RequestFormLimits(MultipartBodyLengthLimit = 500 * 1024 * 1024)]
        public IActionResult DashBoardData()
        {
            clsRenewUnit objRenewUnit = new clsRenewUnit();
            clsUnit objUnit = new clsUnit();
            //Check wheter the user name and type
            // var UType = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UType").Value;

            dynamic rstJSON = new System.Dynamic.ExpandoObject();
            DataSet ds = objRenewUnit.GetAllUnitsForDashboardIncludingRenewRequested();
            rstJSON.CancelRequests = General.Dt2JSON(ds.Tables[4]);
            rstJSON.Amendments = General.Dt2JSON(ds.Tables[5]);
            rstJSON.Requests = General.Dt2JSON(ds.Tables[0]);
            rstJSON.ReInspection = General.Dt2JSON(ds.Tables[6]);

            rstJSON.CancelRequestsCount = ds.Tables[1].Rows[0]["Count"].ToString();
            rstJSON.AmendmentsCount = ds.Tables[2].Rows[0]["Count"].ToString();
            rstJSON.RequestsCount = ds.Tables[0].Rows.Count.ToString();
            rstJSON.ReInspectionCount = ds.Tables[3].Rows[0]["Count"].ToString();

            if (ds != null && ds.Tables.Count > 0)
                return Ok(new { statusCode = 200, data = new { DashBoardData = rstJSON } });
            else
                return Ok(new { statusCode = 400, data = new { data = "No Data found" } });
        }







        [Authorize]
        [HttpPost]
        public IActionResult GetAllAdmins()
        {
            clsAdmin objAdmin = new clsAdmin();
            DataTable dt = new DataTable();
            //Check wheter the user name and type
            // var UType = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UType").Value;

            try
            {

                objAdmin.AdminType = "Admin";
                dt = objAdmin.GetAllAdminsByAdminType();

            }
            catch (Exception ex)
            { }

            dynamic rstJSON = new System.Dynamic.ExpandoObject();
            rstJSON.Admins = General.Dt2JSON(dt);


            if (dt != null && dt.Rows.Count > 0)
                return Ok(new { statusCode = 200, data = new { AdminDatas = rstJSON } });
            else
                return Ok(new { statusCode = 400, data = new { data = "No Data found" } });
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteUser(int UserId)
        {
            clsAdmin objAdmin = new clsAdmin();
            DataTable dt = new DataTable();
            //Check wheter the user name and type
            // var UType = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UType").Value;
            objAdmin.UserID = UserId;
            objAdmin.DeleteAdminUser();
            #region ActivityLog Add

            decimal userid = 0;
            string usertype = string.Empty, strMasterItem = string.Empty;

            var UserID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "AdminUserID").Value;
            var AdminUser = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "AdminUser").Value;
            var AdminType = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "AdminType").Value;

            userid = Convert.ToDecimal(UserID);
            usertype = AdminType.ToString();


            objAdmin.UserID = Convert.ToInt32(UserID.ToString());
            DataTable dtAd = objAdmin.GetAdminUserById();
            if (dtAd != null && dtAd.Rows.Count > 0)
            { strMasterItem = dtAd.Rows[0]["AdminUser"].ToString(); }

            clsActivityLog objActivityLog = new clsActivityLog();
            objActivityLog.Form = "AdminPanel/ManageAdmins";
            objActivityLog.Action = "Delete";
            objActivityLog.Activity = "Admin User Deleted";
            objActivityLog.Description = "Admin User Deleted Success";
            objActivityLog.Status = "Success";
            objActivityLog.Parameters = "User Name:" + strMasterItem + ",Admin Type:Admin";
            objActivityLog.MasterId = Convert.ToInt32(UserID.ToString());
            objActivityLog.MasterItem = strMasterItem;
            objActivityLog.MasterType = "tbl_AdminUsers";
            objActivityLog.UserID = userid;
            objActivityLog.IsAdmin = true;
            objActivityLog.UserType = usertype;
            objActivityLog.InsertActivityLog();

            #endregion//ActivityLog Add



            return Ok(new { statusCode = 200 });
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditUser(int UserId, string UserName, string Password, string IsActive, bool IsInspector)
        {
            clsAdmin objAdmin = new clsAdmin();
            DataTable dt = new DataTable();
            //Check wheter the user name and type
            // var UType = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UType").Value;

            var UserID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "AdminUserID").Value;
            var AdminUser = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "AdminUser").Value;
            var AdminType = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "AdminType").Value;

            objAdmin.UserID = Convert.ToInt32(UserID.ToString());
            objAdmin.UserName = UserName;
            objAdmin.Password = EncryptDecryptHelper.Encrypt(Password);
            objAdmin.IsActive = Convert.ToInt32(IsActive);
            objAdmin.IsInspector = IsInspector;
            objAdmin.AdminType = "Admin";
            int result = objAdmin.UpdateAdminUser();

            if (result > 0)
            {
                #region ActivityLog Add

                clsActivityLog objActivityLog = new clsActivityLog();
                objActivityLog.Form = "AdminPanel/ManageAdmins";
                objActivityLog.Action = "Update";
                objActivityLog.Activity = "Admin User Updated";
                objActivityLog.Description = "Admin User Updated Success";
                objActivityLog.Status = "Success";
                objActivityLog.Parameters = "User Name:" + AdminUser.ToString() + ",Admin Type:Admin";
                objActivityLog.MasterId = Convert.ToInt32(UserID.ToString());
                objActivityLog.MasterItem = UserName;
                objActivityLog.MasterType = "tbl_AdminUsers";
                objActivityLog.UserID = decimal.Parse(UserID.ToString());
                objActivityLog.IsAdmin = true;
                objActivityLog.UserType = AdminType.ToString();
                objActivityLog.InsertActivityLog();

                #endregion//ActivityLog Add
            }



            return Ok(new { statusCode = 200 });
        }

        [Authorize]
        [HttpPost]
        public IActionResult InsertUser(int UserId, string UserName, string Password, string IsActive, bool IsInspector)
        {
            clsAdmin objAdmin = new clsAdmin();
            DataTable dt = new DataTable();
            //Check wheter the user name and type

            var UserID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "AdminUserID").Value;
            var AdminUser = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "AdminUser").Value;
            var AdminType = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "AdminType").Value;


            objAdmin.UserName = UserName;
            objAdmin.Password = EncryptDecryptHelper.Encrypt(Password);
            objAdmin.IsActive = Convert.ToInt32(IsActive);
            objAdmin.IsInspector = IsInspector;
            objAdmin.AdminType = "Admin";
            int result = objAdmin.InsertAdminUser();
            if (result > 0)
            {
                #region ActivityLog Add


                clsActivityLog objActivityLog = new clsActivityLog();
                objActivityLog.Form = "AdminPanel/ManageAdmins";
                objActivityLog.Action = "Save";
                objActivityLog.Activity = "Admin User Saved";
                objActivityLog.Description = "Admin User Saved Success";
                objActivityLog.Status = "Success";
                objActivityLog.Parameters = "User Name:" + AdminUser.ToString() + ",Admin Type:Admin";
                objActivityLog.MasterId = result;
                objActivityLog.MasterItem = AdminUser.ToString();
                objActivityLog.MasterType = "tbl_AdminUsers";
                objActivityLog.UserID = decimal.Parse(UserID.ToString());
                objActivityLog.IsAdmin = true;
                objActivityLog.UserType = AdminType.ToString();
                objActivityLog.InsertActivityLog();

                #endregion//ActivityLog Add

            }


            return Ok(new { statusCode = 200 });
        }

        [Authorize]
        [HttpPost]
        public IActionResult OPApprovalRequests(string Status, string Classifications)
        {
            clsUnit objUnit = new clsUnit();
            DataTable dtunits = new DataTable();
            dtunits = objUnit.GetAllOperatorUnitsByStatus(Status, Classifications);
            dynamic rstJSON = new System.Dynamic.ExpandoObject();
            rstJSON.UserProfile = General.Dt2JSON(dtunits);

            if (dtunits != null && dtunits.Rows.Count > 0)
                return Ok(new { statusCode = 200, data = new { Data = rstJSON } });
            else
                return Ok(new { statusCode = 400, data = new { data = "No Data found" } });
        }

        [Authorize]
        [HttpPost]
        public IActionResult OPApprovalRequestDetails(string UnitID)
        {

            clsUnit objUnit = new clsUnit();
            Email objEmail = new Email();
            string OperatorRootPath = "~/UploadedFiles/OperatorFiles/";

            clsAdmin objAdmin = new clsAdmin();
            clsOwner objOwner = new clsOwner();
            clsActivityLog objActivityLog = new clsActivityLog();
            objActivityLog.LogId = 0;
            objActivityLog.Keyword = string.Empty;
            objActivityLog.MasterId = Convert.ToInt32(UnitID);
            objActivityLog.MasterType = "tbl_Units";
            objActivityLog.UserID = 0;
            objActivityLog.UserType = string.Empty;
            DataTable dtaclog = objActivityLog.GetActivityLogSearch(new DateTime(2015, 1, 1), DateTime.Now.AddDays(1));

            DataTable dtdet = new DataTable();
            objUnit.Unit_ID = Convert.ToInt32(UnitID);
            objUnit.User_Type = "OP";
            dtdet = objUnit.GetUnitDetails();
            dynamic rstJSON = new System.Dynamic.ExpandoObject();
            rstJSON.UnitDetails = General.Dt2JSON(dtdet);
            rstJSON.DataLog = General.Dt2JSON(dtaclog);
            string HplPassportNavigateUrl = OperatorRootPath + dtdet.Rows[0]["User_ID"].ToString() + "/" + UnitID + "/Passport/" + dtdet.Rows[0]["Passport"].ToString();
            string HplEmiDFrNavigateUrl = OperatorRootPath + dtdet.Rows[0]["User_ID"].ToString() + "/" + UnitID + "/EmiratesIDFront/" + dtdet.Rows[0]["EmiratesID_Front"].ToString();
            string HplUnitNavigateUrl = OperatorRootPath + dtdet.Rows[0]["User_ID"].ToString() + "/" + UnitID + "/TitleDeed/" + dtdet.Rows[0]["UnitTitleDeed"].ToString();
            string HplHHANavigateUrl = OperatorRootPath + dtdet.Rows[0]["User_ID"].ToString() + "/" + UnitID + "/HHA/" + dtdet.Rows[0]["HHAgreement"].ToString();
            string HplFEWANavigateUrl = OperatorRootPath + dtdet.Rows[0]["User_ID"].ToString() + "/" + UnitID + "/FEWA/" + dtdet.Rows[0]["UnitFEWABill"].ToString();
            string HplCompLicenseNavigateUrl = OperatorRootPath + dtdet.Rows[0]["User_ID"].ToString() + "/" + UnitID + "/CompLicense/" + dtdet.Rows[0]["CompanyLicense"].ToString();
            rstJSON.HplPassportNavigateUrl = HplPassportNavigateUrl;
            rstJSON.HplEmiDFrNavigateUrl = HplEmiDFrNavigateUrl;
            rstJSON.HplUnitNavigateUrl = HplUnitNavigateUrl;
            rstJSON.HplHHANavigateUrl = HplHHANavigateUrl;
            rstJSON.HplFEWANavigateUrl = HplFEWANavigateUrl;
            rstJSON.HplCompLicenseNavigateUrl = HplCompLicenseNavigateUrl;

            return Ok(new { statusCode = 200, data = new { Data = rstJSON } });
        }

        [Authorize]
        [HttpPost]
        public IActionResult OPApprovalRequestDetailsSubmit(string UnitID, string Status, string Comment, string EmailId, string Name)
        {
            string strVal = "";
            clsUnit objUnit = new clsUnit();
            Email objEmail = new Email();
            int temp;
            int RandomNumber;

            var UserID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "AdminUserID").Value;
            var AdminUser = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "AdminUser").Value;
            var AdminType = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "AdminType").Value;
            do
            {
                Random rand = new Random();
                RandomNumber = rand.Next(10000000, 999999999);
                DataTable dtcheck = new DataTable();
                dtcheck = objUnit.sCheckLicenseExist(RandomNumber.ToString());
                if (dtcheck != null && dtcheck.Rows.Count > 0)
                {
                    temp = 0;
                }
                else
                {
                    temp = 1;
                }
            }
            while (temp == 0);
            string licenseno;
            if (Status == "Approved")
            {
                licenseno = RandomNumber.ToString();
            }
            else
            {
                licenseno = "";
            }
            int result = objUnit.updateUnitApprovalStatus(Convert.ToInt32(UnitID), "", Status, licenseno);
            objUnit.updateUnitnotificationadmin(Convert.ToInt32(UnitID), 1);
            if (Comment != "")
            {
                objUnit.insertComment(Convert.ToInt32(UnitID), Convert.ToInt32(UserID.ToString()), "a", "u", Comment);
            }
            if (result == 1)
            {
                #region ActivityLog Add

                decimal userid = 0;
                string usertype = string.Empty;
                userid = int.Parse(UserID.ToString());
                usertype = AdminType;
                clsActivityLog objActivityLog = new clsActivityLog();
                objActivityLog.Form = "AdminPanel/OPRequestDetails";
                objActivityLog.Action = "Update";
                objActivityLog.Activity = "Reply to Unit Permit Request";
                objActivityLog.Description = "Unit " + Status + "."
                    + (Comment != string.Empty ? ("\nCommented: " + Comment + ".") : string.Empty)
                    + (Status == "Approved" ? ("\nLicense No: " + licenseno + ".") : string.Empty);
                objActivityLog.Status = "Success";
                objActivityLog.Parameters = "Unit No:" + UnitID
                    + (Status != string.Empty ? (",Comment:" + Status) : string.Empty)
                    + ",Status:" + Status
                    + (Status == "Approved" ? (",License No:" + licenseno) : string.Empty);
                objActivityLog.MasterId = Convert.ToInt32(UnitID);
                objActivityLog.MasterItem = UnitID;
                objActivityLog.MasterType = "tbl_Units";
                objActivityLog.UserID = userid;
                objActivityLog.IsAdmin = true;
                objActivityLog.UserType = usertype;
                objActivityLog.InsertActivityLog();

                #endregion//ActivityLog Add

                if (Status == "Approved")
                {
                    strVal = objEmail.SendMailNoti(EmailId, RegisterMailContents(Name, UnitID), "Your request for Holiday Homes has been approved.", "Holiday Homes RAK");
                }
                else
                {
                    if (Comment != "")
                    {
                        strVal = objEmail.SendMailNoti(EmailId, RegisterMailContentsComment(Name, UnitID), "You’ve received a new notification", "Holiday Homes RAK");
                    }
                }

            }

            return Ok(new { statusCode = 200, data = new { msg = "Mail Send Successfully" } });
        }

        [Authorize]
        [HttpPost]
        public IActionResult AssignInspector(int InspectorId, int UnitID)
        {
            clsAdmin Objadmin = new clsAdmin();

            if (Objadmin.AssignInspector(InspectorId, UnitID) > 0)

                return Ok(new { statusCode = 200, data = new {   data = "Assigned to Inspector" } });
            else
                return Ok(new { statusCode = 400, data = new { data = "Not Assigned" } });
        }

        [Authorize]
        [HttpPost]
        public IActionResult GetAllInspectors()
        {
            clsAdmin objAdmin = new clsAdmin();
            DataTable dt = new DataTable();
            //Check wheter the user name and type
            // var UType = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UType").Value;

            try
            {

                objAdmin.AdminType = "Admin";
                dt = objAdmin.GetAllInspectors();

            }
            catch (Exception ex)
            { }

            dynamic rstJSON = new System.Dynamic.ExpandoObject();
            rstJSON.Admins = General.Dt2JSON(dt);


            if (dt != null && dt.Rows.Count > 0)
                return Ok(new { statusCode = 200, data = new { data = rstJSON } });
            else
                return Ok(new { statusCode = 400, data = new { data = "No Data found" } });
        }

        [HttpPost]
        public IActionResult GetPermitTypes()
        {
            clsUnit objUnit = new clsUnit();
            DataTable dtfill = new DataTable();
            dtfill = objUnit.GetAllUnitPermitTypes();

            if (dtfill != null && dtfill.Rows.Count > 0)
                return Ok(new { statusCode = 200, data = new { PermitTypes = General.Dt2JSON(dtfill) } });
            else
                return Ok(new { statusCode = 400, data = new { data = "Permit Types Not Found." } });
        }

        [HttpPost]
        public IActionResult GetClassifications(int PropertyTypeId)
        {
            clsOwner objOwner = new clsOwner();
            DataTable dtfill = new DataTable();
            dtfill = objOwner.GetClassifications(PropertyTypeId);

            if (dtfill != null && dtfill.Rows.Count > 0)
                return Ok(new { statusCode = 200, data = new { Classifications = General.Dt2JSON(dtfill) } });
            else
                return Ok(new { statusCode = 400, data = new { data = "Classifications Types Not Found." } });
        }


        public string RegisterMailContentsComment(string name, string UnitID)
        {
            var contentRootPath = (string)AppDomain.CurrentDomain.GetData("ContentRootPath");
            var webRootPath = (string)AppDomain.CurrentDomain.GetData("WebRootPath");
            string url = new Uri(_config["MySettings:Domain"]) + "/Payunits/" + EncryptDecryptHelper.Encrypt("OP") + "_" + UnitID;

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(BLL.Common.General.MapPath("~/EmailTemplates/CommentMail.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Name}", name);
            body = body.Replace("{LinkUrl}", url);
            return body;
        }
        public string RegisterMailContents(string name, string UnitID)
        {
            string url = new Uri(_config["MySettings:Domain"]) + "/UnitDetails/" + EncryptDecryptHelper.Encrypt("OP") + "_" + UnitID;
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(BLL.Common.General.MapPath("~/EmailTemplates/PermitApproval.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Name}", name);
            body = body.Replace("{LinkUrl}", url);
            return body;
        }







    }
}