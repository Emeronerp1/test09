using RakTDAApi.BLL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RAKHolidayHomesBL
{
    public class clsPrivilege
    {
        #region Property
        private SqlConnection mSqlConnection;
        private SqlTransaction mTransaction;

        public decimal PrivilegeId { get; set; }
        public decimal PageId { get; set; }
        public decimal AdminUserId { get; set; }
        public bool Access { get; set; }

        ////public decimal PageId { get; set; }
        ////public string Page { get; set; }
        ////public string Title { get; set; }
            
        #endregion

        public int InsertPrivilege()
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[4];
                arrParam[0] = new SqlParameter("@PageId", PageId);
                arrParam[1] = new SqlParameter("@AdminUserId", AdminUserId);
                arrParam[2] = new SqlParameter("@Access", Access);
                arrParam[3] = new SqlParameter("@ID", SqlDbType.Int);
                arrParam[3].Direction = ParameterDirection.InputOutput;
                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);
                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_InsertPrivilege", arrParam);
                mTransaction.Commit();
                int chk = int.Parse(arrParam[3].Value.ToString());
                mSqlConnection.Close();
                return chk;
            }
            catch (Exception ex)
            {
                //Utility.ExceptionHelper.Log(ex);
                return 0;
            }
        }

        public DataTable GetPrivilegeByAdminUserId()
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[1];
                arrParam[0] = new SqlParameter("@AdminUserId", AdminUserId);
                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetPrivilegeByAdminUserId", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int DeletePrivilegeByAdminUserId()
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[2];
                arrParam[0] = new SqlParameter("@AdminUserId", AdminUserId);
                arrParam[1] = new SqlParameter("@ID", SqlDbType.Int);
                arrParam[1].Direction = ParameterDirection.InputOutput;
                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);
                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_DeletePrivilegeByAdminUserId", arrParam);
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

    }
}
