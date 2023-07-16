using RakTDAApi.BLL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RAKHolidayHomesBL
{
    public class clsUnit
    {
        #region Property
        private SqlConnection mSqlConnection;
        private SqlTransaction mTransaction;

        public int Unit_ID { get; set; }
        public string User_Type { get; set; }
        public int User_ID { get; set; }
        public string Plot_Number { get; set; }
        public string Unit_Number { get; set; }
        public string Floor_Number { get; set; }
        public string Area_Name { get; set; }
        public string Street_Name { get; set; }
        public string Location { get; set; }
        public string FEWA_AccountNo { get; set; }
        public string UnitPermitType { get; set; }

        public decimal HandlingUserId { get; set; }
        public DateTime HandlingStartedOn { get; set; }

        public string IssuedPermitHoldType { get; set; }
        public DateTime HoldOn { get; set; }
        public decimal HoldByUserId { get; set; }
        public string HoldReason { get; set; }
        public DateTime UnHoldOn { get; set; }
        public decimal UnHoldByUserId { get; set; }
        public string UnHoldReason { get; set; }

        public int PropertyTypeId { get; set; } = 0;
        public int UnitClassificationTypeId { get; set; } = 0;
        public bool ClassificationStatus { get; set; } = false;
        public bool IsCancelRequest { get; set; } = false;
        public string CancelRequestStatus { get; set; }
        public string CancelRequestReason { get; set; } = "";

        public bool IsReInspection { get; set; }
        public string ReInspectionRequestReason { get; set; }

        public string CommercialName { get; set; }

        public string RequestType { get; set; } = "NewUnit";


        #endregion

        public int InsertUnitDetails()
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[15];

                arrParam[0] = new SqlParameter("@User_Type", User_Type);
                arrParam[1] = new SqlParameter("@User_ID", User_ID);
                arrParam[2] = new SqlParameter("@Plot_Number", Plot_Number);
                arrParam[3] = new SqlParameter("@Unit_Number", Unit_Number);
                arrParam[4] = new SqlParameter("@Floor_Number", Floor_Number);
                arrParam[5] = new SqlParameter("@Area_Name", Area_Name);
                arrParam[6] = new SqlParameter("@Street_Name", Street_Name);
                arrParam[7] = new SqlParameter("@Location", Location);
                arrParam[8] = new SqlParameter("@FEWA_AccountNo", FEWA_AccountNo);
                arrParam[9] = new SqlParameter("@UnitPermitType", UnitPermitType);


                arrParam[10] = new SqlParameter("@PropertyTypeId", PropertyTypeId);
                arrParam[11] = new SqlParameter("@UnitClassificationTypeId", UnitClassificationTypeId);
                arrParam[12] = new SqlParameter("@ClassificationStatus", ClassificationStatus);
                arrParam[13] = new SqlParameter("@ID", SqlDbType.Int);
                arrParam[14] = new SqlParameter("@CommercialName", CommercialName);
                arrParam[13].Direction = ParameterDirection.InputOutput;


                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);

                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_InsertUnit", arrParam);
                mTransaction.Commit();
                int chk = int.Parse(arrParam[13].Value.ToString());
                mSqlConnection.Close();
                return chk;
            }
            catch (Exception ex)
            {
                //Utility.ExceptionHelper.Log(ex);
                return 0;
            }
        }

        public int UpdateUnitDetails()
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[12];

                arrParam[0] = new SqlParameter("@Unit_ID", Unit_ID);
                arrParam[1] = new SqlParameter("@User_Type", User_Type);
                arrParam[2] = new SqlParameter("@User_ID", User_ID);
                arrParam[3] = new SqlParameter("@Plot_Number", Plot_Number);
                arrParam[4] = new SqlParameter("@Unit_Number", Unit_Number);
                arrParam[5] = new SqlParameter("@Floor_Number", Floor_Number);
                arrParam[6] = new SqlParameter("@Area_Name", Area_Name);
                arrParam[7] = new SqlParameter("@Street_Name", Street_Name);
                arrParam[8] = new SqlParameter("@Location", Location);
                arrParam[9] = new SqlParameter("@FEWA_AccountNo", FEWA_AccountNo);
                arrParam[10] = new SqlParameter("@UnitPermitType", UnitPermitType);
                arrParam[11] = new SqlParameter("@CommercialName", CommercialName);

                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);

                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_UpdateUnit", arrParam);
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


        //[sp_GetUnitDetailsForCancelPermit]


        public DataTable GetUnitDetailsForCancelPermit()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Unit_ID", Unit_ID);
                arrParam[1] = new SqlParameter("@User_Type", User_Type);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetUnitDetailsForCancelPermit", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetUnitDetails()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Unit_ID", Unit_ID);
                arrParam[1] = new SqlParameter("@User_Type", User_Type);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetUnitDetails", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetUnitDetailsByUnitId()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[1];
                arrParam[0] = new SqlParameter("@Unit_ID", Unit_ID);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetUnitDetailsByUnitId", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetUnitDetailstoPrintOW()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[1];
                arrParam[0] = new SqlParameter("@Unit_ID", Unit_ID);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetUnitDetailstoPrintOW", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetUnitDetailstoPrintOP()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[1];
                arrParam[0] = new SqlParameter("@Unit_ID", Unit_ID);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetUnitDetailstoPrintOP", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllUnitsByUser()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@User_Type", User_Type);
                arrParam[1] = new SqlParameter("@User_ID", User_ID);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetAllUnitsByUser", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllUnitsByStatus(string AStatus)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[1];
                arrParam[0] = new SqlParameter("@AStatus", AStatus);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetAllUnitsByStatus", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllOWnerUnitsByStatus(string AStatus, string ClassificationTypeId)
        {
            //@ClassificationTypeId
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@AStatus", AStatus); //@ClassificationTypeId
                arrParam[1] = new SqlParameter("@ClassificationTypeId", ClassificationTypeId);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetAllOwnerUnitsByStatus", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllCancelRequestUnitsOW()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "[sp_GetAllCancelRequestUnitsOW]");
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetAllOperatorUnitsByStatus(string AStatus, string ClassificationTypeId)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@AStatus", AStatus);
                arrParam[1] = new SqlParameter("@ClassificationTypeId", ClassificationTypeId);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetAllOperatorUnitsByStatus", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetHolidayHomesCheckList()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "SP_HolidayHomesCheckListView");
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetHolidayFarmsCheckList()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "SP_HolidayFarmsCheckListView");
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllCancelRequestUnitsOP()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "[sp_GetAllCancelRequestUnitsOP]");
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllReInspectionRequestsOP()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "[sp_GetAllReInspectionRequestsOP]");
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetAllReInspectionRequestsOW()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "[sp_GetAllReInspectionRequestsOW]");
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllUnitPermitTypes()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetAllUnitPermitTypes");
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int updateUnitApprovalStatus(int UnitID, string Comment, string AStatus, string LicenseNo)
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[4];

                arrParam[0] = new SqlParameter("@UnitID", UnitID);
                arrParam[1] = new SqlParameter("@Comment", Comment);
                arrParam[2] = new SqlParameter("@AStatus", AStatus);
                arrParam[3] = new SqlParameter("@LicenseNo", LicenseNo);

                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);

                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_updateUnitApprovalStatus", arrParam);
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

        public int updateUnitPaymentStatus(int UnitID, string tranid)
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[2];

                arrParam[0] = new SqlParameter("@UnitID", UnitID);
                arrParam[1] = new SqlParameter("@PaymentTrnNo", tranid);

                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);

                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_updateUnitPaymentStatus", arrParam);
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

        public int updateUnitnotification(int UnitID, int IsNotify)
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[2];

                arrParam[0] = new SqlParameter("@UnitID", UnitID);
                arrParam[1] = new SqlParameter("@IsNotify", IsNotify);

                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);

                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_updateUnitnotification", arrParam);
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

        public DataTable sCheckLicenseExist(string LicenseNo)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[1];
                arrParam[0] = new SqlParameter("@LicenseNo", LicenseNo);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_CheckLicenseExist", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int insertComment(int UnitID, int UserID, string Comfrom, string Comto, string CommentText)
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[5];

                arrParam[0] = new SqlParameter("@UnitID", UnitID);
                arrParam[1] = new SqlParameter("@UserID", UserID);
                arrParam[2] = new SqlParameter("@Comfrom", Comfrom);
                arrParam[3] = new SqlParameter("@Comto", Comto);
                arrParam[4] = new SqlParameter("@CommentText", CommentText);


                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);

                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_insertComment", arrParam);
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

        public DataTable getAllCommentsbyUnit(int UnitID)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[1];
                arrParam[0] = new SqlParameter("@UnitID", UnitID);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_getAllCommentsbyUnit", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllOperatorUnitsSearch(string AStatus, string Keyword, int User_ID)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[3];
                arrParam[0] = new SqlParameter("@AStatus", AStatus);
                arrParam[1] = new SqlParameter("@Keyword", Keyword);
                arrParam[2] = new SqlParameter("@User_ID", User_ID);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetAllOperatorUnitsSearch", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllOWnerUnitsSearch(string AStatus, string Keyword, int User_ID)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[3];
                arrParam[0] = new SqlParameter("@AStatus", AStatus);
                arrParam[1] = new SqlParameter("@Keyword", Keyword);
                arrParam[2] = new SqlParameter("@User_ID", User_ID);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetAllOwnerUnitsSearch", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllRevenue(DateTime PaymentDateFrom, DateTime PaymentDateTo, string User_Type)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[3];
                arrParam[0] = new SqlParameter("@PaymentDateFrom", PaymentDateFrom);
                arrParam[1] = new SqlParameter("@PaymentDateTo", PaymentDateTo);
                arrParam[2] = new SqlParameter("@User_Type", User_Type);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetAllRevenue", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllExpired(DateTime ExpiryDateFrom, DateTime ExpiryDateTo, string User_Type)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[3];
                arrParam[0] = new SqlParameter("@ExpiryDateFrom", ExpiryDateFrom);
                arrParam[1] = new SqlParameter("@ExpiryDateTo", ExpiryDateTo);
                arrParam[2] = new SqlParameter("@User_Type", User_Type);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetAllExpired", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable UpdateLockStatusForPermitRequest()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Unit_ID", Unit_ID);
                arrParam[1] = new SqlParameter("@HandlingUserId", HandlingUserId);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_UpdateLockStatusForPermitRequest", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetLockStatusForPermitRequest()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[1];
                arrParam[0] = new SqlParameter("@Unit_ID", Unit_ID);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetLockStatusForPermitRequest", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllRenewed(DateTime RenewedDateFrom, DateTime RenewedDateTo, string User_Type)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[3];
                arrParam[0] = new SqlParameter("@RenewedDateFrom", RenewedDateFrom);
                arrParam[1] = new SqlParameter("@RenewedDateTo", RenewedDateTo);
                arrParam[2] = new SqlParameter("@User_Type", User_Type);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetAllRenewed", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int UpdateUnitHoldType()
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[5];
                arrParam[0] = new SqlParameter("@Unit_ID", Unit_ID);
                arrParam[1] = new SqlParameter("@IssuedPermitHoldType", IssuedPermitHoldType);
                arrParam[2] = new SqlParameter("@HoldByUserId", HoldByUserId);
                arrParam[3] = new SqlParameter("@HoldReason", HoldReason);
                arrParam[4] = new SqlParameter("@Operation", IssuedPermitHoldType);
                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);
                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_UpdateUnitHoldType", arrParam);
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


        public int RequestCancelPermit()
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[3];
                arrParam[0] = new SqlParameter("@Unit_ID", Unit_ID);
                arrParam[1] = new SqlParameter("@CancelRequestStatus", CancelRequestStatus);
                arrParam[2] = new SqlParameter("@CancelRequestReason", CancelRequestReason);
                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);
                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_RequestCancelPermit", arrParam);
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


        public int ReInspectionRequest()
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Unit_ID", Unit_ID);
                arrParam[1] = new SqlParameter("@ReInspectionRequestReason", ReInspectionRequestReason);
                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);
                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "[sp_ReInspectionRequest]", arrParam);
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

        public int UnholdUnit(string Operation)
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[4];
                arrParam[0] = new SqlParameter("@Unit_ID", Unit_ID);
                arrParam[1] = new SqlParameter("@UnHoldByUserId", UnHoldByUserId);
                arrParam[2] = new SqlParameter("@UnHoldReason", UnHoldReason);
                arrParam[3] = new SqlParameter("@Operation", Operation);
                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);
                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_UnholdUnit", arrParam);
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

        public DataTable GetAllUnitsByHoldType(DateTime HoldOnFrom, DateTime HoldOnTo)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[4];
                arrParam[0] = new SqlParameter("@HoldOnFrom", HoldOnFrom);
                arrParam[1] = new SqlParameter("@HoldOnTo", HoldOnTo);
                arrParam[2] = new SqlParameter("@IssuedPermitHoldType", IssuedPermitHoldType);
                arrParam[3] = new SqlParameter("@User_Type", User_Type);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetAllUnitsByHoldType", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllApprovedUnitsByDate(DateTime HoldOnFrom, DateTime HoldOnTo,string UserType)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[4];
                arrParam[0] = new SqlParameter("@FromDate", HoldOnFrom);
                arrParam[1] = new SqlParameter("@ToDate", HoldOnTo);
                arrParam[3] = new SqlParameter("@UserType", UserType);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetAllApprovedUnitsByDate", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllCancelledUnitsByDate(DateTime HoldOnFrom, DateTime HoldOnTo, string UserType)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[4];
                arrParam[0] = new SqlParameter("@FromDate", HoldOnFrom);
                arrParam[1] = new SqlParameter("@ToDate", HoldOnTo);
                arrParam[3] = new SqlParameter("@UserType", UserType);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "[sp_GetAllCancelledUnitsByDate]", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }




        public DataTable GetAllPaidUnitsByDate(DateTime HoldOnFrom, DateTime HoldOnTo, string UserType)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[4];
                arrParam[0] = new SqlParameter("@FromDate", HoldOnFrom);
                arrParam[1] = new SqlParameter("@ToDate", HoldOnTo);
                arrParam[3] = new SqlParameter("@UserType", UserType);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "[sp_GetAllPaidUnitsByDate]", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllPendingUnitsByDate(DateTime HoldOnFrom, DateTime HoldOnTo, string UserType)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[4];
                arrParam[0] = new SqlParameter("@FromDate", HoldOnFrom);
                arrParam[1] = new SqlParameter("@ToDate", HoldOnTo);
                arrParam[3] = new SqlParameter("@UserType", UserType);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "[sp_GetAllPendingUnitsByDate]", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllRejectedUnitsByDate(DateTime HoldOnFrom, DateTime HoldOnTo, string UserType)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[4];
                arrParam[0] = new SqlParameter("@FromDate", HoldOnFrom);
                arrParam[1] = new SqlParameter("@ToDate", HoldOnTo);
                arrParam[3] = new SqlParameter("@UserType", UserType);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "[sp_GetAllRejectedUnitsByDate]", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllUpgradedUnitsByDate(DateTime HoldOnFrom, DateTime HoldOnTo, string UserType)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[4];
                arrParam[0] = new SqlParameter("@FromDate", HoldOnFrom);
                arrParam[1] = new SqlParameter("@ToDate", HoldOnTo);
                arrParam[3] = new SqlParameter("@UserType", UserType);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "[sp_GetAllUpgradedUnitsByDate]", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetRevenueComparison(decimal decYear1, decimal decYear2)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Year1", decYear1);
                arrParam[1] = new SqlParameter("@Year2", decYear2);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetRevenueComparison", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int InsertCheckListResults(int UnitId, int PermitTypeId, int ClassificationTypeId, string IndexCode, bool Result)
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
                    new SqlParameter("@Result", Result)
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


        public int UpdateUpgradeRequest(int UnitID,int ClassificationId)
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[2];

                arrParam[0] = new SqlParameter("@UnitID", UnitID);
                arrParam[1] = new SqlParameter("@ClassificationTo", ClassificationId);
                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);

                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "UpgradeRequest", arrParam);
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


        public DataTable GetAllUpgradeRequestUnitsOW()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "[sp_GetAllUpgradeRequestUnitsOW]");
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllUpgradeRequestUnitsOP()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetAllUpgradeRequestUnitsOP");
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable GetAllInspectionScheduledListOP(string UserType = "OP")
        {
            try
            {
                DataSet dsTemp = new DataSet();
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "GetAllInspectionScheduledListOP",
                    new SqlParameter("@User_Type", UserType));
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public DataTable GetAllInspectionAssignedListOP(string UserType="OP")
        {
            try
            {
                DataSet dsTemp = new DataSet();
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "GetAllInspectionAssignedList",
                    new SqlParameter("@User_Type", UserType));
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable GetAllInspectionAssignedListOW(string UserType = "OW")
        {
            try
            {
                DataSet dsTemp = new DataSet();
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "GetAllInspectionAssignedList",
                    new SqlParameter("@User_Type", UserType));
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataSet GetUserProfileDetails(int UserId, string UserType = "OW")
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@UserId", UserId);
                arrParam[1] = new SqlParameter("@UserType", UserType);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr,
                    CommandType.StoredProcedure, "GetUserProfileDetails", arrParam);
                return dsTemp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable GetStatement(string UserType = "OW",int UserId=0)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "GetStatement",
                    new SqlParameter("@UserId", UserId),
                    new SqlParameter("@UserType", UserType));
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllUnitsByUserForCancelPermits()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@User_Type", User_Type);
                arrParam[1] = new SqlParameter("@User_ID", User_ID);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "GetAllUnitsByUserForCancelPermits", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllUnitsByUserForReInspectionRequests()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@User_Type", User_Type);
                arrParam[1] = new SqlParameter("@User_ID", User_ID);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "GetAllUnitsByUserForReInspectionRequests", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllUnitsByUserForUpgradeRequests()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@User_Type", User_Type);
                arrParam[1] = new SqlParameter("@User_ID", User_ID);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "GetAllUnitsByUserForUpgradeRequests", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

       public bool checkpayment(int UserId,string UserType)
        {
            clsOwner objOwner = new clsOwner();
            DataTable dtcheck = new DataTable();
            objOwner.User_ID = UserId;
            objOwner.User_Type = UserType;
            dtcheck = objOwner.CheckPaymentDone();
            if (dtcheck != null && dtcheck.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



    }
}
