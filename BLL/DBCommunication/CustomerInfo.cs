using RakTDAApi.BLL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RakTDAApi.BLL.DBCommunication
{
    public class CustomerInfo : General
    {
        public static DataTable CustomerMasterSearch(string strSearch, int CompanyId)
        {
            DataSet dsCustomerMaster = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "CustomerMasterSearch",
               new SqlParameter("@SearchKey", strSearch), new SqlParameter("@companyId", CompanyId));
            return dsCustomerMaster.Tables[0];
        }

        public static DataTable CustomerRead(int CustId, int CompanyId)
        {
            DataSet dsCustomer = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "tbl_Customers_View",
               new SqlParameter("@CustomerID", CustId), new SqlParameter("@companyId", CompanyId));
            return dsCustomer.Tables[0];
        }

        public static DataTable CustomerDetailsRead(int CustId)
        {
            DataSet dsCustomerDetails = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "tbl_CustomerDetails_ViewaAPI",
               new SqlParameter("@CustomerID", CustId));
            return dsCustomerDetails.Tables[0];
        }

        public static DataTable FindLastSales(int CustId, int CompanyId)
        {
            DataSet dsLastSales = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "FindLastSalesDetails",
               new SqlParameter("@LedgerId", CustId), new SqlParameter("@companyId", CompanyId));
            return dsLastSales.Tables[0];
        }

        public static DataTable GetAreaList(int CompanyId)
        {
            DataSet dsAreaList = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "AreafillInCustomerAPi",
              new SqlParameter("@companyId", CompanyId));
            return dsAreaList.Tables[0];
        }

        public static DataTable GetCityList(int CompanyId)
        {
            DataSet dsCityList = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "CityfillInCustomerAPI",
               new SqlParameter("@companyId", CompanyId));
            return dsCityList.Tables[0];
        }

        public static DataTable GetRouteList(int CompanyId)
        {
            DataSet dsRouteList = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "RouteViewAllAPI",
               new SqlParameter("@companyId", CompanyId));
            return dsRouteList.Tables[0];
        }

        public static DataTable GetCustomerDefaultAddressDetails(int CustomerID)
        {
            DataSet dsDefaultAddress = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "tbl_CustomerDetails_DefaultAddressAPI",
               new SqlParameter("@CustomerID", @CustomerID));
            return dsDefaultAddress.Tables[0];
        }

        public static int SaveCustomer(int CompanyID_, string FirstName_, string FamilyName_, string mobile_, string OfficePhone_,
                      string HomePhone_, string OtherPhone_, string email_, int creditperiod, decimal creditlimit,
                      string Narration_, string CompanyName_, string Title_)
        {
            
            int returnCustomerID = Convert.ToInt32(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure, "tbl_Customers_Insert",
                               new SqlParameter("@CompanyID", CompanyID_), new SqlParameter("@FirstName", FirstName_),
                               new SqlParameter("@FamilyName", FamilyName_), new SqlParameter("@mobile", mobile_),
                               new SqlParameter("@OfficePhone", OfficePhone_), new SqlParameter("@HomePhone", HomePhone_),
                               new SqlParameter("@OtherPhone", OtherPhone_), new SqlParameter("@email", email_),
                               new SqlParameter("@creditPeriod", creditperiod), new SqlParameter("@creditLimit", creditlimit),
                               new SqlParameter("@Narration", Narration_), new SqlParameter("@CompanyName", CompanyName_),
                               new SqlParameter("@Title", Title_)));
            return returnCustomerID;
        }

        public static int UpdateCustomer(int CustomerId_, int CompanyID_, string FirstName_, string FamilyName_, string mobile_, string OfficePhone_,
                      string HomePhone_, string OtherPhone_, string email_, string Narration_, string CompanyName_, string Title_)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "tbl_Customers_UpdateAPI", new SqlParameter("@CustomerID", CustomerId_),
                                   new SqlParameter("@CompanyID", CompanyID_), new SqlParameter("@FirstName", FirstName_),
                                   new SqlParameter("@FamilyName", FamilyName_), new SqlParameter("@mobile", mobile_),
                                   new SqlParameter("@OfficePhone", OfficePhone_), new SqlParameter("@HomePhone", HomePhone_),
                                   new SqlParameter("@OtherPhone", OtherPhone_), new SqlParameter("@email", email_),
                                   new SqlParameter("@Narration", Narration_), new SqlParameter("@CompanyName", CompanyName_),
                                    new SqlParameter("@Title", Title_));
                return CustomerId_;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        public static int SaveCustomerDetails(int CustomerID_, int City_, string Street_, string Floor_,
                      string Apartment_, string Building_, string Near_, int AreaID_, string Zip_, bool isDefault_, int AddressID_)
        {
            int returnCustDetailID = Convert.ToInt32(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure, "tbl_CustomerDetails_Insert",
                                        new SqlParameter("@CustomerID", CustomerID_), new SqlParameter("@Apartment", Apartment_),
                                        new SqlParameter("@City", City_), new SqlParameter("@Street", Street_),
                                        new SqlParameter("@Floor", Floor_), new SqlParameter("@Building", Building_),
                                        new SqlParameter("@Near", Near_),
                                        new SqlParameter("@AreaID", AreaID_), new SqlParameter("@Zip", Zip_),
                                        new SqlParameter("@isDefault", isDefault_), new SqlParameter("@AddressID", AddressID_)));
            return returnCustDetailID;
        }

        public static int UpdateCustomerDetails(int CustDetailID_, int CustomerID_, int City_, string Street_, string Floor_, string Apartment_,
                    string Building_, string Near_, int AreaID_, string Zip_, bool isDefault_)
        {
            
            try
            {
                SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "tbl_CustomerDetails_Update",
                                        new SqlParameter("@CustDetailID", CustDetailID_), new SqlParameter("@CustomerID", CustomerID_),
                                        new SqlParameter("@Apartment", Apartment_),
                                        new SqlParameter("@City", City_), new SqlParameter("@Street", Street_),
                                        new SqlParameter("@Floor", Floor_), new SqlParameter("@Building", Building_),
                                        new SqlParameter("@Near", Near_), 
                                        new SqlParameter("@AreaID", AreaID_), new SqlParameter("@Zip", Zip_),
                                        new SqlParameter("@isDefault", isDefault_));
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public static int SalesExists(int CustId, int CompanyId)
        {
            int salescount;
            DataSet dsSalesExists = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "TotalOrder",
             new SqlParameter("@CustomerID", CustId), new SqlParameter("@companyId", CompanyId));
            if (dsSalesExists.Tables[0].Rows[0][0] == DBNull.Value)
                salescount = 0;
            else
                salescount = Convert.ToInt32(dsSalesExists.Tables[0].Rows[0][0].ToString());
            return salescount;
        }

        public static string FirstOrderDate(int CustId, int CompanyId)
        {
            string firstOrderdt;
            DataTable dataTable = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "FindFirstOrder",
             new SqlParameter("@LedgerId", CustId), new SqlParameter("@companyId", CompanyId)).Tables[0];
            if (dataTable.Rows[0][0] == DBNull.Value)
                firstOrderdt = "";
            else
                firstOrderdt = dataTable.Rows[0][0].ToString();

            return firstOrderdt;
        }

        public static string LastOrderDate(int CustId, int CompanyId)
        {
            string firstOrderdt;
            DataTable dataTable = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "LastOrderDate",
             new SqlParameter("@LedgerId", CustId), new SqlParameter("@companyId", CompanyId)).Tables[0];
            if (dataTable.Rows[0][0] == DBNull.Value)
                firstOrderdt = "";
            else
                firstOrderdt = dataTable.Rows[0][0].ToString();

            return firstOrderdt;
        }

        public static string TotalAmount(int CustId, int CompanyId)
        {
            string amount = "0";
            DataTable dataTable = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "TotalAmountofLastSales",
            new SqlParameter("@LedgerId", CustId), new SqlParameter("@companyId", CompanyId)).Tables[0];
            if (dataTable.Rows.Count > 0)
            {
                amount = dataTable.Rows[0][0].ToString();
                if (amount == "" || amount == string.Empty || amount == null)
                {
                    amount = "0";
                }
            }
            return amount;
        }

        public static int CheckExistingOrderID(int CustomerId, int CompanyId)
        {
            int SalesMasterId = 0;
            DataTable dataTable = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "POS_SELOpenOrdersByCustomer",
            new SqlParameter("@CustomerID", CustomerId), new SqlParameter("@companyId", CompanyId)).Tables[0];
            if (dataTable.Rows.Count > 0)
            {
                SalesMasterId = int.Parse(dataTable.Rows[0][0].ToString());
            }
            return SalesMasterId;
        }

        public static DataTable GetPOSPaymentCreditDeliveryTypes()
        {
            DataSet dsPaymentDeliveryType = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "POSPaymentCreditDeliveryTypes");
            return dsPaymentDeliveryType.Tables[0];
        }

        public static DataTable GetPOSDeliveryAgents(int CompanyId)
        {
            DataSet POSDeliveryAgents = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "[GetPOSDeliveryAgents]",
                 new SqlParameter("@CompanyId", CompanyId) );
            return POSDeliveryAgents.Tables[0];
        }

    }
}
