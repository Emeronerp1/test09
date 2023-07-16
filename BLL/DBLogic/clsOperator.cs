using RakTDAApi.BLL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RAKHolidayHomesBL
{
    public class clsOperator
    {
        #region Properties
        private SqlConnection mSqlConnection;
        private SqlTransaction mTransaction;

        public int Operator_Id { get; set; }
        public string Company_Name { get; set; }
        public string Manager_Name { get; set; }
        public string Manager_Nationality { get; set; }
        public string Company_LicenseNo { get; set; }
        public DateTime Company_LicenseExpDate { get; set; }
        public string Company_Email { get; set; }
        public string Company_UserName { get; set; }
        public string Company_Password { get; set; }
        public string Company_Mobile { get; set; }
        public string Company_OtherMob { get; set; } 
        public string Company_Address { get; set; }
        public int IsEmailVerified { get; set; }
        public int IsRegFeePaid { get; set; }
        public int PropertyTypeId { get; set; } = 0;
        public int UnitClassificationTypeId { get; set; } = 0;
        public bool ClassificationStatus { get; set; } = false;

        #endregion

        public int InsertOperatorDetails()
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[15];

                arrParam[0] = new SqlParameter("@Company_Name", Company_Name);
                arrParam[1] = new SqlParameter("@Manager_Name", Manager_Name);
                arrParam[2] = new SqlParameter("@Manager_Nationality", Manager_Nationality);
                arrParam[3] = new SqlParameter("@Company_LicenseNo", Company_LicenseNo);
                arrParam[4] = new SqlParameter("@Company_LicenseExpDate", Company_LicenseExpDate);
                arrParam[5] = new SqlParameter("@Company_Email", Company_Email);
                arrParam[6] = new SqlParameter("@Company_UserName", Company_UserName);
                arrParam[7] = new SqlParameter("@Company_Password", Company_Password);
                arrParam[8] = new SqlParameter("@Company_Mobile", Company_Mobile);
                arrParam[9] = new SqlParameter("@Company_OtherMob", Company_OtherMob);
                arrParam[10] = new SqlParameter("@Company_Address", Company_Address);
                arrParam[11] = new SqlParameter("@ID", SqlDbType.Int);
                arrParam[12] = new SqlParameter("@PropertyTypeId", PropertyTypeId);
                arrParam[13] = new SqlParameter("@UnitClassificationTypeId", UnitClassificationTypeId);
                arrParam[14] = new SqlParameter("@ClassificationStatus", ClassificationStatus);
                arrParam[11].Direction = ParameterDirection.InputOutput;


                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);

                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_InsertOperator", arrParam);
                mTransaction.Commit();
                int chk = int.Parse(arrParam[11].Value.ToString());
                mSqlConnection.Close();
                return chk;
            }
            catch (Exception ex)
            {
                //Utility.ExceptionHelper.Log(ex);
                return 0;
            }
        }

        public int UpdateOperatorDetails() 
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[11];

                arrParam[0] = new SqlParameter("@Operator_ID", Operator_Id);
                arrParam[1] = new SqlParameter("@Company_Name", Company_Name);
                arrParam[2] = new SqlParameter("@Manager_Name", Manager_Name);
                arrParam[3] = new SqlParameter("@Manager_Nationality", Manager_Nationality);
                arrParam[4] = new SqlParameter("@Company_LicenseNo", Company_LicenseNo);
                arrParam[5] = new SqlParameter("@Company_LicenseExpDate", Company_LicenseExpDate);
                arrParam[6] = new SqlParameter("@Company_Email", Company_Email);
                arrParam[7] = new SqlParameter("@Company_UserName", Company_UserName);
                arrParam[8] = new SqlParameter("@Company_Mobile", Company_Mobile);
                arrParam[9] = new SqlParameter("@Company_OtherMob", Company_OtherMob);
                arrParam[10] = new SqlParameter("@Company_Address", Company_Address);


                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);

                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_UpdateOperator", arrParam);
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
