using RakTDAApi.BLL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RAKHolidayHomesBL
{
    public class clsAttachment
    {
        #region Property
        private SqlConnection mSqlConnection;
        private SqlTransaction mTransaction;

        public int Unit_ID { get; set; }
        public string Passport { get; set; }
        public string EmiratesID_Front { get; set; }
        public string EmiratesID_Back { get; set; }
        public string UnitTitleDeed { get; set; }
        public string HHAgreement { get; set; }
        public string CompanyLicense { get; set; }
        public string Tenancy { get; set; } 
        public string UnitFEWABill { get; set; }
        #endregion


        public int InsertOperatorUnitAttachment() 
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[8];

                arrParam[0] = new SqlParameter("@Unit_ID", Unit_ID);
                arrParam[1] = new SqlParameter("@Passport", Passport);
                arrParam[2] = new SqlParameter("@EmiratesID_Front", EmiratesID_Front);
                arrParam[3] = new SqlParameter("@EmiratesID_Back", EmiratesID_Back);
                arrParam[4] = new SqlParameter("@UnitTitleDeed", UnitTitleDeed);
                arrParam[5] = new SqlParameter("@HHAgreement", HHAgreement);
                arrParam[6] = new SqlParameter("@CompanyLicense", CompanyLicense);
                arrParam[7] = new SqlParameter("@UnitFEWABill", UnitFEWABill);


                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);

                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_InsertOperatorUnitAtt", arrParam);
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

        public int UpdateOperatorUnitAttachment() 
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[8];

                arrParam[0] = new SqlParameter("@Unit_ID", Unit_ID);
                arrParam[1] = new SqlParameter("@Passport", Passport);
                arrParam[2] = new SqlParameter("@EmiratesID_Front", EmiratesID_Front);
                arrParam[3] = new SqlParameter("@EmiratesID_Back", EmiratesID_Back);
                arrParam[4] = new SqlParameter("@UnitTitleDeed", UnitTitleDeed);
                arrParam[5] = new SqlParameter("@HHAgreement", HHAgreement);
                arrParam[6] = new SqlParameter("@CompanyLicense", CompanyLicense);
                arrParam[7] = new SqlParameter("@UnitFEWABill", UnitFEWABill);


                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);

                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_UpdateOperatorUnitAtt", arrParam);
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

        public int InsertOwnerUnitAttachment() 
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[8];

                arrParam[0] = new SqlParameter("@Unit_ID", Unit_ID);
                arrParam[1] = new SqlParameter("@Passport", Passport);
                arrParam[2] = new SqlParameter("@EmiratesID_Front", EmiratesID_Front);
                arrParam[3] = new SqlParameter("@EmiratesID_Back", EmiratesID_Back);
                arrParam[4] = new SqlParameter("@UnitTitleDeed", UnitTitleDeed);
                arrParam[5] = new SqlParameter("@HHAgreement", HHAgreement);
                arrParam[6] = new SqlParameter("@Tenancy", Tenancy);
                arrParam[7] = new SqlParameter("@UnitFEWABill", UnitFEWABill);


                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);

                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_InsertOwnerUnitAtt", arrParam);
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

        public int UpdateOwnerUnitAttachment()
        {
            try
            {
                SqlParameter[] arrParam = new SqlParameter[8];

                arrParam[0] = new SqlParameter("@Unit_ID", Unit_ID);
                arrParam[1] = new SqlParameter("@Passport", Passport);
                arrParam[2] = new SqlParameter("@EmiratesID_Front", EmiratesID_Front);
                arrParam[3] = new SqlParameter("@EmiratesID_Back", EmiratesID_Back);
                arrParam[4] = new SqlParameter("@UnitTitleDeed", UnitTitleDeed);
                arrParam[5] = new SqlParameter("@HHAgreement", HHAgreement);
                arrParam[6] = new SqlParameter("@Tenancy", Tenancy);
                arrParam[7] = new SqlParameter("@UnitFEWABill", UnitFEWABill);


                mSqlConnection = new SqlConnection(RakTDAApi.BLL.Common.General.dbConStr);

                mSqlConnection.Open();
                mTransaction = mSqlConnection.BeginTransaction();
                object obj = SqlHelper.ExecuteScalar(mTransaction, CommandType.StoredProcedure, "sp_UpdateOwnerUnitAtt", arrParam);
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
