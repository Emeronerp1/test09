using RakTDAApi.BLL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RAKHolidayHomesBL
{
    public class clsUpgradeClassifications
    {
        private SqlConnection mSqlConnection;
        private SqlTransaction mTransaction;


        public int UCId { get; set; }
        public int UnitId { get; set; } = 0;
        public int CurrentClassificationId { get; set; } = 0;
        public int UpgradeClassificationId { get; set; } = 0;
        public DateTime DateOfRequest { get; set; } = DateTime.Now;
        public bool ApprovalStatus { get; set; } = false;
        public DateTime? ApprovedDate { get; set; }
        public int SiteVisitId { get; set; } = 0;
        public string Comments { get; set; } = "";
        public bool IsActive { get; set; } = true;


        public int InsertUpgradeClassifications()
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[10];

                arrParam[0] = new SqlParameter("@UnitId", UnitId);
                arrParam[1] = new SqlParameter("@CurrentClassificationId", CurrentClassificationId);
                arrParam[2] = new SqlParameter("@UpgradeClassificationId", UpgradeClassificationId);
                arrParam[3] = new SqlParameter("@DateOfRequest", DateOfRequest);
                arrParam[4] = new SqlParameter("@ApprovalStatus", ApprovalStatus);
                arrParam[5] = new SqlParameter("@ApprovedDate", ApprovedDate);
                arrParam[6] = new SqlParameter("@SiteVisitId", SiteVisitId);
                arrParam[7] = new SqlParameter("@Comments", Comments);
                arrParam[8] = new SqlParameter("@IsActive", IsActive);
                arrParam[9] = new SqlParameter("@UCId", SqlDbType.Int);

                arrParam[9].Direction = ParameterDirection.InputOutput;

                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);

                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_InsertUpgradeClassifications", arrParam);
                mTransaction.Commit();
                int chk = int.Parse(arrParam[9].Value.ToString());
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
