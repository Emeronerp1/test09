using RakTDAApi.BLL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RAKHolidayHomesBL
{
    public class clsRenewUnit
    {
        #region Property
        private SqlConnection mSqlConnection;
        private SqlTransaction mTransaction;

        public int RenewRequestId { get; set; }
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

        public string ApprovalStatus { get; set; }
        public string Comment { get; set; }
        public DateTime ApprovalDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string PaymentTrnNo { get; set; }
        public DateTime PaymentDate { get; set; }
        public int IsNotify { get; set; }
        public int IsNotifyAdmin { get; set; }
        public decimal RenewCount { get; set; }
        public DateTime PreviousExpiryDate { get; set; }

        #endregion

        public int RenewRequestUnit()
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@Unit_ID", Unit_ID);
                arrParam[1] = new SqlParameter("@ID", SqlDbType.Int);
                arrParam[1].Direction = ParameterDirection.InputOutput;
                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);
                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_RenewRequestUnit", arrParam);
                mTransaction.Commit();
                int chk = int.Parse(arrParam[1].Value.ToString());
                mSqlConnection.Close();
                return chk;
            }
            catch (Exception ex)
            {
                //Utility.ExceptionHelper.Log(ex);
                return 0;
            }
        }

        public DataTable GetAllOperatorRenewUnitsByStatus(string AStatus)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[1];
                arrParam[0] = new SqlParameter("@AStatus", AStatus);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetAllOperatorRenewUnitsByStatus", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetRenewUnitDetails()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@RenewRequestId", RenewRequestId);
                arrParam[1] = new SqlParameter("@User_Type", User_Type);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetRenewUnitDetails", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int updateRenewUnitApprovalStatus(int RenewRequestId, string Comment, string AStatus)
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[3];
                arrParam[0] = new SqlParameter("@RenewRequestId", RenewRequestId);
                arrParam[1] = new SqlParameter("@Comment", Comment);
                arrParam[2] = new SqlParameter("@AStatus", AStatus);
                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);
                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_updateRenewUnitApprovalStatus", arrParam);
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

        public int insertRenewComment(int RenewRequestId, int UserID, string Comfrom, string Comto, string CommentText)
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[5];
                arrParam[0] = new SqlParameter("@RenewRequestId", RenewRequestId);
                arrParam[1] = new SqlParameter("@UserID", UserID);
                arrParam[2] = new SqlParameter("@Comfrom", Comfrom);
                arrParam[3] = new SqlParameter("@Comto", Comto);
                arrParam[4] = new SqlParameter("@CommentText", CommentText);
                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);
                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_insertRenewComment", arrParam);
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

        public DataTable getAllRenewCommentsbyRequestID(int RequestID)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[1];
                arrParam[0] = new SqlParameter("@RenewRequestId", RequestID);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_getAllRenewCommentsbyRequestID", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetRenewUnitDetailsByUnitId()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[1];
                arrParam[0] = new SqlParameter("@Unit_ID", Unit_ID);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetRenewUnitDetailsByUnitId", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int UpdateRenewUnitDetails()
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[12];
                arrParam[0] = new SqlParameter("@RenewRequestId", RenewRequestId);
                arrParam[1] = new SqlParameter("@Unit_ID", Unit_ID);
                arrParam[2] = new SqlParameter("@User_Type", User_Type);
                arrParam[3] = new SqlParameter("@User_ID", User_ID);
                arrParam[4] = new SqlParameter("@Plot_Number", Plot_Number);
                arrParam[5] = new SqlParameter("@Unit_Number", Unit_Number);
                arrParam[6] = new SqlParameter("@Floor_Number", Floor_Number);
                arrParam[7] = new SqlParameter("@Area_Name", Area_Name);
                arrParam[8] = new SqlParameter("@Street_Name", Street_Name);
                arrParam[9] = new SqlParameter("@Location", Location);
                arrParam[10] = new SqlParameter("@FEWA_AccountNo", FEWA_AccountNo);
                arrParam[11] = new SqlParameter("@UnitPermitType", UnitPermitType);
                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);
                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_UpdateRenewUnit", arrParam);
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

        public DataSet GetAllUnitsForDashboardIncludingRenewRequested()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetAllUnitsForDashboardIncludingRenewRequested");
                return dsTemp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int updateRenewUnitnotification(int RenewRequestId, int IsNotify)
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@RenewRequestId", RenewRequestId);
                arrParam[1] = new SqlParameter("@IsNotify", IsNotify);
                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);
                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_updateRenewUnitnotification", arrParam);
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

        public int updateRenewUnitnotificationadmin(int RenewRequestId, int IsNotifyAdmin)
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@RenewRequestId", RenewRequestId);
                arrParam[1] = new SqlParameter("@IsNotifyAdmin", IsNotifyAdmin);
                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);
                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_updateRenewUnitnotificationadmin", arrParam);
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

        public DataTable GetAllOWnerRenewUnitsByStatus(string AStatus)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[1];
                arrParam[0] = new SqlParameter("@AStatus", AStatus);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetAllOwnerRenewUnitsByStatus", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int updateRenewUnitPaymentStatus(int RenewRequestId, string tranid)
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@RenewRequestId", RenewRequestId);
                arrParam[1] = new SqlParameter("@PaymentTrnNo", tranid);
                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);
                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_updateRenewUnitPaymentStatus", arrParam);
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

    }
}
