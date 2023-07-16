using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using RakTDAApi.BLL.Common;

namespace RAKHolidayHomesBL
{
    public class clsActivityLog
    {
        #region Property
        private SqlConnection mSqlConnection;
        private SqlTransaction mTransaction;

        public decimal LogId { get; set; }
        public string Form { get; set; }
        public string Action { get; set; }
        public string Activity { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Parameters { get; set; }
        public decimal MasterId { get; set; }
        public string MasterItem { get; set; }
        public string MasterType { get; set; }
        public decimal UserID { get; set; }
        public bool IsAdmin { get; set; }
        public string UserType { get; set; }
        public DateTime DoneOn { get; set; }

        public string Keyword { get; set; }
        #endregion

        public int InsertActivityLog()
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[12];
                arrParam[0] = new SqlParameter("@Form", Form);
                arrParam[1] = new SqlParameter("@Action", Action);
                arrParam[2] = new SqlParameter("@Activity", Activity);
                arrParam[3] = new SqlParameter("@Description", Description);
                arrParam[4] = new SqlParameter("@Status", Status);
                arrParam[5] = new SqlParameter("@Parameters", Parameters);
                arrParam[6] = new SqlParameter("@MasterId", MasterId);
                arrParam[7] = new SqlParameter("@MasterItem", MasterItem);
                arrParam[8] = new SqlParameter("@MasterType", MasterType);
                arrParam[9] = new SqlParameter("@UserID", UserID);
                arrParam[10] = new SqlParameter("@IsAdmin", IsAdmin);
                arrParam[11] = new SqlParameter("@UserType", UserType);

                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);
                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_InsertActivityLog", arrParam);
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

        public DataTable GetActivityLogSearch(DateTime dtDoneOnFrom, DateTime dtDoneOnTo)
        {
            try
            {
                DataSet dsTemp = new DataSet();
                SqlParameter[] arrParam = new SqlParameter[8];
                arrParam[0] = new SqlParameter("@DoneOnFrom", dtDoneOnFrom);
                arrParam[1] = new SqlParameter("@DoneOnTo", dtDoneOnTo);
                arrParam[2] = new SqlParameter("@LogId", LogId);
                arrParam[3] = new SqlParameter("@Keyword", Keyword);
                arrParam[4] = new SqlParameter("@MasterId", MasterId);
                arrParam[5] = new SqlParameter("@MasterType", MasterType);
                arrParam[6] = new SqlParameter("@UserID", UserID);
                arrParam[7] = new SqlParameter("@UserType", UserType);

                dsTemp = SqlHelper.ExecuteDataset(RakTDAApi.BLL.Common.General.dbConStr, CommandType.StoredProcedure, "sp_GetActivityLogSearch", arrParam);
                return dsTemp.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
