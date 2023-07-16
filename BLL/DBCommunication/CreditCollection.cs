using RakTDAApi.BLL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RakTDAApi.BLL.DBCommunication
{
    public class CreditCollection:General
    {
        public static DataTable CustomerSearch(string Search,int CompanyId) // ok
        {
            DataSet dsSettings = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "CustomerMasterSearch"
                                  , new SqlParameter("@SearchKey", Search)
                                  , new SqlParameter("@CompanyID", CompanyId));
            return dsSettings.Tables[0];
        }

        public static DataTable LoadDiscountOnCreditCollection(int CustomerId, int CompanyId) // okay
        {
            DataSet dsSettings = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "LoadDiscountOnCreditCollection"
                                  , new SqlParameter("@CustomerID", CustomerId)
                                  , new SqlParameter("@CompanyID", CompanyId));
            return dsSettings.Tables[0];
        }

        public static int CalculateDiscount(Models.Payloads.CreditCollection creditCollection)
        {
            var rst = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "POS_UPDCustomerCollections",
                  new SqlParameter("@CompanyID", creditCollection.CompanyId),
                  new SqlParameter("@CustomerID", creditCollection.CustomerId),
                  new SqlParameter("@Amount", creditCollection.Amount),
                  new SqlParameter("@date", creditCollection.dt),
                  new SqlParameter("@PayTypeId", 1),
                  new SqlParameter("@PayType", creditCollection.PayType),
                  new SqlParameter("@DiscountID", creditCollection.DiscountID),
                  new SqlParameter("@DiscountTypeID", creditCollection.DiscountTypeID),
                  new SqlParameter("@DiscountValue", creditCollection.DiscountValue),
                  new SqlParameter("@ReferenceNo", "Discount"),
                  new SqlParameter("@EmployeeID", creditCollection.UserId));

            int rs = Convert.ToInt32(rst);
            return rs;
        }

        public static int ClearDiscount(int CompanyId, int CustomerId)
        {
            var rst = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "POS_UPDCustomerCollectionsAPI",
                  new SqlParameter("@CompanyID", CompanyId),
                  new SqlParameter("@CustomerID", CustomerId),
                  new SqlParameter("@Amount", 0),
                  new SqlParameter("@date", DateTime.Now),
                  new SqlParameter("@PayTypeId", 1),
                  new SqlParameter("@PayType", 3),
                  new SqlParameter("@DiscountID", 0),
                  new SqlParameter("@DiscountValue", 0),
                  new SqlParameter("@ReferenceNo", "Discount"),
                  new SqlParameter("@EmployeeID", 0));

            int rs = Convert.ToInt32(rst);
            return rs;
        }

        public static int SavePayment(int CompanyId, int CustomerId, int PayTypeID, double Amount,  int Userid)
        {
            var rst = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "POS_UPDCustomerCollectionsAPI",
                  new SqlParameter("@CompanyID", CompanyId),
                  new SqlParameter("@CustomerID", CustomerId),
                  new SqlParameter("@Amount", Amount),
                  new SqlParameter("@date", DateTime.Now),
                  new SqlParameter("@PayTypeId", PayTypeID),
                  new SqlParameter("@PayType", 1),
                  new SqlParameter("@DiscountID", 0),
                  new SqlParameter("@DiscountValue", 0),
                  new SqlParameter("@ReferenceNo", "Payment"),
                  new SqlParameter("@EmployeeID", Userid));

            int rs = Convert.ToInt32(rst);
            return rs;
        }

        public static int SaveDiscount(int CompanyId, int CustomerId, int DiscountID, double DiscountValue, int Userid)
        {
            var rst = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "POS_UPDCustomerCollectionsAPI",
                  new SqlParameter("@CompanyID", CompanyId),
                  new SqlParameter("@CustomerID", CustomerId),
                  new SqlParameter("@Amount", 0),
                  new SqlParameter("@date", DateTime.Now),
                  new SqlParameter("@PayTypeId", 1),
                  new SqlParameter("@PayType", 2),
                  new SqlParameter("@DiscountID", DiscountID),
                  new SqlParameter("@DiscountValue", DiscountValue),
                  new SqlParameter("@ReferenceNo", "Discount"),
                  new SqlParameter("@EmployeeID", Userid));

            int rs = Convert.ToInt32(rst);
            return rs;
        }


        public static DataTable LoadPaymentMethods(string OpenMode ="CreditCollection") // ok
        {
            if (OpenMode == "CreditCollection")
            {
                DataSet dsSettings = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "POSPaymentColloectionTypes");
                return dsSettings.Tables[0];
            }else
            {
                DataSet dsSettings = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "POSPaymentTypes");
                return dsSettings.Tables[0];
            }

        }

        public static DataTable LoadDiscountByAmount() // ok
        {
            DataSet dsSettings = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "POS_SEL_DiscountedItemsByAmount");
            return dsSettings.Tables[0];
        }

        public static double GetDueAmount(int CompanyId, int CustomerId)  // ok
        {
            double DueAmount = Convert.ToDouble(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure,
                    "POS_SEL_CustomerCreditAmount",
                     new SqlParameter("@CompanyID", CompanyId),
                     new SqlParameter("@CustomerID", CustomerId)));
            return DueAmount;
        }

        public static DataTable StatementOfAccount(int CompanyId, int CustomerId, DateTime begdate, DateTime enddate)
        {
            DataTable dtStatement = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "POS_StatementOfAccount",
               new SqlParameter("@companyId", CompanyId),
               new SqlParameter("@CustomerID", CustomerId),
               new SqlParameter("@begdate", begdate),
               new SqlParameter("@enddate", enddate)).Tables[0];


            if (dtStatement.Rows.Count > 0)
            {
                dtStatement.Rows[0]["balance"] = Convert.ToDouble(dtStatement.Rows[0]["balance"].ToString()) + Convert.ToDouble(dtStatement.Rows[0]["Debit"].ToString()) - Convert.ToDouble(dtStatement.Rows[0]["Credit"].ToString()) - Convert.ToDouble(dtStatement.Rows[0]["DiscountValue"].ToString());    // ds.Stat_Account_sub(0)(9)
                if (dtStatement.Rows.Count > 1)
                {
                    int j;
                    var loopTo = dtStatement.Rows.Count - 1;
                    for (j = 1; j <= loopTo; j++)
                        dtStatement.Rows[j]["balance"] = Convert.ToDouble(dtStatement.Rows[j - 1]["balance"].ToString()) + Convert.ToDouble(dtStatement.Rows[j]["debit"].ToString()) - Math.Abs(Convert.ToDouble(dtStatement.Rows[j]["Credit"].ToString())) - Convert.ToDouble(dtStatement.Rows[j]["DiscountValue"].ToString());
                }
            }
            return dtStatement;
        }

    }
}
