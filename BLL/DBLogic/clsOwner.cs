using Microsoft.AspNetCore.Http;
using RakTDAApi.BLL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace RAKHolidayHomesBL
{
    public class clsOwner
    {
        #region Properties
        private SqlConnection mSqlConnection;
        private SqlTransaction mTransaction;

        public int User_ID { get; set; }
        public string User_Type { get; set; }
        public string User_Email { get; set; }
        public int Owner_ID { get; set; }
        public string Owner_Name { get; set; }
        public string Owner_Nationality { get; set; }
        public string Owner_EmID { get; set; }
        public string Owner_Passport { get; set; }
        public string Owner_Email { get; set; }
        public string Owner_UserName { get; set; }
        public string Owner_Password { get; set; }
        public string Owner_Mobile { get; set; }
        public string Owner_OtherMob { get; set; } 
        public string Owner_Address { get; set; }
        public int IsEmailVerified { get; set; }
        public int IsRegFeePaid { get; set; }
        public int PropertyTypeId { get; set; } = 0;
        public int UnitClassificationTypeId { get; set; } = 0;
        public bool ClassificationStatus { get; set; } = false;



        #endregion

        public int InsertOwnerDetails()
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[14];

                arrParam[0] = new SqlParameter("@Owner_Name", Owner_Name);
                arrParam[1] = new SqlParameter("@Owner_Nationality", Owner_Nationality);
                arrParam[2] = new SqlParameter("@Owner_EmID", Owner_EmID);
                arrParam[3] = new SqlParameter("@Owner_Passport", Owner_Passport);
                arrParam[4] = new SqlParameter("@Owner_Email", Owner_Email);
                arrParam[5] = new SqlParameter("@Owner_UserName", Owner_UserName);
                arrParam[6] = new SqlParameter("@Owner_Password", Owner_Password);
                arrParam[7] = new SqlParameter("@Owner_Mobile", Owner_Mobile);
                arrParam[8] = new SqlParameter("@Owner_OtherMob", Owner_OtherMob);
                arrParam[9] = new SqlParameter("@Owner_Address", Owner_Address);
                arrParam[10] = new SqlParameter("@ID", SqlDbType.Int);
                arrParam[11] = new SqlParameter("@PropertyTypeId", PropertyTypeId);
                arrParam[12] = new SqlParameter("@UnitClassificationTypeId", UnitClassificationTypeId);
                arrParam[13] = new SqlParameter("@ClassificationStatus", ClassificationStatus);


                arrParam[10].Direction = ParameterDirection.InputOutput;


                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);

                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_InsertOwner", arrParam);
                mTransaction.Commit();
                int chk = int.Parse(arrParam[10].Value.ToString());
                mSqlConnection.Close();
                return chk;
            }
            catch (Exception ex)
            {
                //Utility.ExceptionHelper.Log(ex);
                return 0;
            }
        }

        public int UpdateOwnerDetails()
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[10];

                arrParam[0] = new SqlParameter("@Owner_ID", Owner_ID);
                arrParam[1] = new SqlParameter("@Owner_Name", Owner_Name);
                arrParam[2] = new SqlParameter("@Owner_Nationality", Owner_Nationality);
                arrParam[3] = new SqlParameter("@Owner_EmID", Owner_EmID);
                arrParam[4] = new SqlParameter("@Owner_Passport", Owner_Passport);
                arrParam[5] = new SqlParameter("@Owner_Email", Owner_Email);
                arrParam[6] = new SqlParameter("@Owner_UserName", Owner_UserName);
                arrParam[7] = new SqlParameter("@Owner_Mobile", Owner_Mobile);
                arrParam[8] = new SqlParameter("@Owner_OtherMob", Owner_OtherMob);
                arrParam[9] = new SqlParameter("@Owner_Address", Owner_Address);


                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);

                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_UpdateOwner", arrParam);
                mTransaction.Commit();
                mSqlConnection.Close();
                return 1;
            }
            catch (Exception ex)
            {
                //Utility.ExceptionHelper.Log(ex);
                return 0;
            }
        }

        public DataTable GetUserDetails()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@User_ID", User_ID);
                arrParam[1] = new SqlParameter("@User_Type", User_Type);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetUserDetails", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable CheckEmailConfirmation()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[3];
                arrParam[0] = new SqlParameter("@User_ID", User_ID);
                arrParam[1] = new SqlParameter("@User_Type", User_Type);
                arrParam[2] = new SqlParameter("@User_Email", User_Email);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_CheckEmailConfirmation", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable CheckEmailandpass()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[3];
                arrParam[0] = new SqlParameter("@User_ID", User_ID);
                arrParam[1] = new SqlParameter("@User_Type", User_Type);
                arrParam[2] = new SqlParameter("@User_Email", User_Email);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_CheckEmailandpass", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable CheckEmailExist(string UserEmail)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[1];
                arrParam[0] = new SqlParameter("@UserEmail", UserEmail);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_CheckEmailExist", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable ForgotPassword(string UserEmail)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[1];
                arrParam[0] = new SqlParameter("@UserEmail", UserEmail);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_ForgotPassword", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable LoginCheck(string UserEmail, string Password)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@UserEmail", UserEmail);
                arrParam[1] = new SqlParameter("@Password", Password);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_LoginCheck", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable AdminLoginCheck(string UserName, string Password)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@UserName", UserName);
                arrParam[1] = new SqlParameter("@Password", Password);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr,
                    CommandType.StoredProcedure, "sp_InspectorLoginCheck", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable LoadAssignedList(int UserId)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[1];
                arrParam[0] = new SqlParameter("@UserId", UserId);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr,
                    CommandType.StoredProcedure, "LoadAssignedList", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable ScheduledList(int UserId)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[1];
                arrParam[0] = new SqlParameter("@userId", UserId);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr,
                    CommandType.StoredProcedure, "LoadScheduledList", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable ReScheduledList(int UserId)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[1];
                arrParam[0] = new SqlParameter("@UserId", UserId);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr,
                    CommandType.StoredProcedure, "LoadAssignedListReSchedule", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable LoadVisitedList(int UserId)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[1];
                arrParam[0] = new SqlParameter("@userId", UserId);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr,
                    CommandType.StoredProcedure, "LoadVisitedList", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable LoadForInspection(int UnitId,int RequestIds)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@UnitId", UnitId);
                arrParam[1] = new SqlParameter("@RequestIds", RequestIds);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr,
                    CommandType.StoredProcedure, "LoadForInspection", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable InspectedList(int UnitId, int RequestIds)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@UnitId", UnitId);
                arrParam[1] = new SqlParameter("@RequestIds", RequestIds);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr,
                    CommandType.StoredProcedure, "InspectedList", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int ScheduleSiteVisit(DateTime dt, int unitId, string Time)
        {
            try
            {
                int rst = 1;
                SqlParameter[] arrParam = new SqlParameter[3];
                arrParam[0] = new SqlParameter("@dt", dt);
                arrParam[1] = new SqlParameter("@unitId", unitId);
                arrParam[2] = new SqlParameter("@ScheduledTime", Time);
                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);
                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
               SqlHelper.ExecuteScalar(mTransaction,CommandType.StoredProcedure, "[ScheduleSiteVisit]", arrParam);
                mTransaction.Commit();
                mSqlConnection.Close();
                return rst;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int InsertCheckListResults(int UnitId, int PermitTypeId, int ClassificationTypeId,
       string IndexCode, bool Result, string MasterId = "", int UserId = 0, int UcId = 0, string Remarks = "", string Image = "")
        {
            try
            {
                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);

                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "[Sp_InsertCheckListResults]",
                    new SqlParameter("@UnitId", UnitId),
                    new SqlParameter("@PermitTypeId", PermitTypeId),
                    new SqlParameter("@ClassificationTypeId", ClassificationTypeId),
                    new SqlParameter("@IndexCode", IndexCode),
                    new SqlParameter("@Result", Result),
                    new SqlParameter("@MasterId", MasterId),
                    new SqlParameter("@InspectorId", UserId), new SqlParameter("@RequestId", UcId)
                    , new SqlParameter("@Remarks", Remarks), new SqlParameter("@Image", Image)
                    );

                mTransaction.Commit();
                mSqlConnection.Close();
                return 1;
            }
            catch (Exception ex)
            {
                //Utility.ExceptionHelper.Log(ex);
                return 0;
            }
        }

        public static string SaveImage(string ImgStr, string Paths,string UcId,string IndexCode,string UnitID)
        {
            String path = Paths;
            string imageName =UnitID+"-"+ UcId+"-"+IndexCode.Replace(".","-") + ".png";
            string imgPath = Path.Combine(path, imageName);
            if (Directory.Exists(imgPath))
            {
                File.Delete(imgPath);
            }
            ImgStr = ImgStr.Replace("data:image/jpeg;base64,", "");
            ImgStr = ImgStr.Replace("data:image/png;base64,", "");
            ImgStr = ImgStr.Replace("data:image/gif;base64,", "");
            ImgStr = ImgStr.Replace("data:image/jpg;base64,", "");
            ImgStr = ImgStr.PadRight(ImgStr.Length + (ImgStr.Length * 3) % 4, '=');
            byte[] imageBytes = Convert.FromBase64String(ImgStr);
            File.WriteAllBytes(imgPath, imageBytes);
            return imageName;
        }

        public static string SaveCapturedImages(string data,int UnitId,string root)
        {
            try
            {

                string fileName = "" + UnitId.ToString() + DateTime.Now.ToString();

                byte[] imageBytes = Convert.FromBase64String(data.Split(',')[1]);

                string filePath = root;
                string Img = data.Replace("data:image/png;base64,", "");
                Img = Img.TrimStart();

                //int result = clsUnit.InsertInspectionImages(Convert.ToInt32(HttpContext.Current.Session["UnitId"].ToString()),
                //    HttpContext.Current.Session["id"].ToString(),
                //    Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString()),
                //    fileName,
                //    filePath, Convert.ToInt32(HttpContext.Current.Session["id"].ToString()), data);


                //File.WriteAllBytes(filePath, imageBytes);
                return Img;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public int SiteVisitCompleted(int UnitId, string Comments, int UcId)
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[3];

                arrParam[0] = new SqlParameter("@UnitId", UnitId);
                arrParam[1] = new SqlParameter("@Comments", Comments);//@UcId
                arrParam[2] = new SqlParameter("@UcId", UcId);
                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);

                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "SiteVisitCompleted", arrParam);
                mTransaction.Commit();
                mSqlConnection.Close();
                return 1;
            }
            catch (Exception ex)
            {
                //Utility.ExceptionHelper.Log(ex);
                return 0;
            }
        }

        public string NotifyAdminByEmail(string name, string Unit, string Matter)
        {

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(("~/EmailTemplates/AdminInspectorNotify.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Name}", name);
            body = body.Replace("{LinkUrl}", Matter);
            return body;
        }
        public int updateUnitnotificationadmin(int UnitID, int IsNotifyAdmin)
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[2];

                arrParam[0] = new SqlParameter("@UnitID", UnitID);
                arrParam[1] = new SqlParameter("@IsNotifyAdmin", IsNotifyAdmin);

                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);

                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_updateUnitnotificationadmin", arrParam);
                mTransaction.Commit();
                mSqlConnection.Close();

                return 1;
            }
            catch (Exception ex)
            {
                //Utility.ExceptionHelper.Log(ex);
                return 0;
            }
        }

        public DataTable CheckPaymentDone()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@User_ID", User_ID);
                arrParam[1] = new SqlParameter("@User_Type", User_Type);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_CheckPayment", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllUserTypes()
        {
            try
            {
                DataSet dsTemp = new DataSet();

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetAllUsertypes");
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetClassifications(int PropertyTypeId)
        {
            try
            {
                DataSet dsTemp = new DataSet();

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetClassificationType",
                    new SqlParameter("@PropertyTypeId", PropertyTypeId));
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int UpdateEmailVerificationStatus(int UserID, string UserType, string UserEmail)
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[3];

                arrParam[0] = new SqlParameter("@UserID", UserID);
                arrParam[1] = new SqlParameter("@UserType", UserType);
                arrParam[2] = new SqlParameter("@UserEmail", UserEmail);

                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);

                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_updateEmailVerificationStatus", arrParam);
                mTransaction.Commit();
                mSqlConnection.Close();
                return int.Parse(obj.ToString());
            }
            catch (Exception ex)
            {
                //Utility.ExceptionHelper.Log(ex);
                return 0;
            }
        }

        public int UpdateRegFeeStatus(int UserID, string UserType, string tranid)
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[3];

                arrParam[0] = new SqlParameter("@UserID", UserID);
                arrParam[1] = new SqlParameter("@UserType", UserType);
                arrParam[2] = new SqlParameter("@PaymentTrnNo", tranid);

                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);

                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_updateRegFeeStatus", arrParam);
                mTransaction.Commit();
                mSqlConnection.Close();
                return 1;
            }
            catch (Exception ex)
            {
                //Utility.ExceptionHelper.Log(ex);
                return 0;
            }
        }


        public int InsertRecoveryDetails(string REmailEnc,string RKey,string Email,string UType)
       {
           try
           {
               SqlParameter[] arrParam = new SqlParameter[4];
               arrParam[0] = new SqlParameter("@REmailEnc", REmailEnc);
               arrParam[1] = new SqlParameter("@RKey", RKey);
               arrParam[2] = new SqlParameter("@REmail", Email);
               arrParam[3] = new SqlParameter("@UType", UType);
               mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);
               mSqlConnection.Open();
               mTransaction = mSqlConnection.BeginTransaction();
               int status = SqlHelper.ExecuteNonQuery(mTransaction, CommandType.StoredProcedure, "sp_InsertRecoveryDetails", arrParam);
               mTransaction.Commit();
               mSqlConnection.Close();
               return status;
           }
           catch (Exception ex)
           {
               return 0;
           }

       }


       public DataTable RecoveryStatusCheck(string REmailEnc, string RKey)
       {
           DataTable dt = new DataTable();
           try
           {
               SqlParameter[] arrParam = new SqlParameter[2];
               arrParam[0] = new SqlParameter("@REmailEnc", REmailEnc);
               arrParam[1] = new SqlParameter("@RKey", RKey);
               DataSet dsTemp = new DataSet();
               dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_RecoveryStatusCheck", arrParam);
               return dsTemp.Tables[0];
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public int UpdateRecoveryStatus(string REmailEnc, string RKey)
       {
           try
           {
               SqlParameter[] arrParam = new SqlParameter[2];
               arrParam[0] = new SqlParameter("@REmailEnc", REmailEnc);
               arrParam[1] = new SqlParameter("@RKey", RKey);
               mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);
               mSqlConnection.Open();
               mTransaction = mSqlConnection.BeginTransaction();
               int status = SqlHelper.ExecuteNonQuery(mTransaction, CommandType.StoredProcedure, "sp_UpdateRecoveryStatus", arrParam);
               mTransaction.Commit();
               mSqlConnection.Close();
               return status;
           }
           catch (Exception ex)
           {
              
               return 0;
           }

       }


       public int UpdatePasswordByEmail(string Email, string Password, string utype)
       {
           try
           {
               SqlParameter[] arrParam = new SqlParameter[3];

               
               arrParam[0] = new SqlParameter("@Email", Email);
               arrParam[1] = new SqlParameter("@Password", Password);
               arrParam[2] = new SqlParameter("@utype", utype);
               mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);
               mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
               SqlHelper.ExecuteNonQuery(mTransaction, CommandType.StoredProcedure, "sp_updatePasswordByEmail", arrParam);
               mTransaction.Commit();
               mSqlConnection.Close();
               return 1;
           }
           catch (Exception ex)
           {
               //Utility.ExceptionHelper.Log(ex);
               return 0;
           }
       }

       public DataTable GetAllUsersByType()
       {
           try
           {
               DataSet dsTemp = new DataSet();
               SqlParameter[] arrParam = new SqlParameter[1];
               arrParam[0] = new SqlParameter("@User_Type", User_Type);

               dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetAllUsersByType", arrParam);
               return dsTemp.Tables[0];
           }
           catch (Exception ex)
           {
               return null;
           }
       }

    }
}
