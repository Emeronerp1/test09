using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using RakTDAApi.BLL.Common;
namespace RAKHolidayHomesBL
{
    public class clsAdmin
    {
        #region Properties
        private SqlConnection mSqlConnection;
        private SqlTransaction mTransaction;
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public int IsActive { get; set; }
        public string AdminType { get; set; }

        public bool IsInspector { get; set; }
        #endregion

        public DataTable AdminLoginCheck()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@UserName", UserName);
                arrParam[1] = new SqlParameter("@Password", Password);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_AdminLoginCheck", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int InsertAdminUser()
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[6];
                arrParam[0] = new SqlParameter("@AdminUser", UserName);
                arrParam[1] = new SqlParameter("@AdminPassword", Password);
                arrParam[2] = new SqlParameter("@IsActive", IsActive);
                arrParam[3] = new SqlParameter("@AdminType", AdminType);
                arrParam[4] = new SqlParameter("@IsInspector", @IsInspector);
                arrParam[5] = new SqlParameter("@ID", SqlDbType.Int);
                arrParam[5].Direction = ParameterDirection.InputOutput;
                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);
                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_InsertAdminUser", arrParam);
                mTransaction.Commit();
                int chk = int.Parse(arrParam[5].Value.ToString());
                mSqlConnection.Close();
                return chk;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int UpdateAdminUser()
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[6];
                arrParam[0] = new SqlParameter("@AdminUserID", UserID);
                arrParam[1] = new SqlParameter("@AdminUser", UserName);
                arrParam[2] = new SqlParameter("@AdminPassword", Password);
                arrParam[3] = new SqlParameter("@IsActive", IsActive);
                arrParam[4] = new SqlParameter("@AdminType", AdminType);
                arrParam[5] = new SqlParameter("@IsInspector", @IsInspector);
                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);
                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_UpdateAdminUser", arrParam);
                mTransaction.Commit();
                mSqlConnection.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public void DeleteAdminUser()
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[1];
                arrParam[0] = new SqlParameter("@AdminUserID", UserID);
                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);
                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_DeleteAdminUser", arrParam);
                mTransaction.Commit();
                mSqlConnection.Close();
            }
            catch (Exception ex)
            { }
        }

        public DataTable GetAdminUserById()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[1];
                arrParam[0] = new SqlParameter("@AdminUserID", UserID);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetAdminUserById", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllAdmins()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetAllAdmins");
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllAdminsByAdminType()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[1];
                arrParam[0] = new SqlParameter("@AdminType", AdminType);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetAllAdminsByAdminType", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable CheckListView(int UnitId)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[3];
                arrParam[0] = new SqlParameter("@ClassificationTypeId", 0);
                arrParam[1] = new SqlParameter("@PermitTypeId", 0);
                arrParam[2] = new SqlParameter("@UnitId", UnitId);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "CheckListView", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable GetAllInspectors()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "GetAllInspectors");
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // Update Assigned Inspector
        public int AssignInspector(int UserId,int UnitId)
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@UserId", UserId);
                arrParam[1] = new SqlParameter("@UnitId", UnitId);
                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);
                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "[AssignInspector]", arrParam);
                mTransaction.Commit();
                mSqlConnection.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public int AssignInspectorForReInspection(int UserId, int UnitId)
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@UserId", UserId);
                arrParam[1] = new SqlParameter("@UnitId", UnitId);
                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);
                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "[AssignInspectorForReInspection]", arrParam);
                mTransaction.Commit();
                mSqlConnection.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int ScheduleSiteVisit(DateTime dt,int unitId)
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@dt", dt);
                arrParam[1] = new SqlParameter("@unitId", unitId);
                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);
                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "[ScheduleSiteVisit]", arrParam);
                mTransaction.Commit();
                mSqlConnection.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


    }
}
