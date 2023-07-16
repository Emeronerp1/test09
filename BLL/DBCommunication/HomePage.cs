using RakTDAApi.BLL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RakTDAApi.BLL.DBCommunication
{
    public class HomePage : General
    {
        public static DataTable LoadDatatableValuesForOrderTypeTax(int OrderType, int CompanyId)
        {
            //getting all the values
            DataTable dtOrderTypeOtherTax = BLL.Common.UserInfo.LoadOrderTypeTaxes(CompanyId);
            DataView dv = new DataView(dtOrderTypeOtherTax);
            dv.RowFilter = string.Format("OrderTypeID='{0}'", OrderType);
            return dv.ToTable();

        }
        public static DataTable LoadDatatableValuesForOrderTypeServices(int OrderType, int CompanyId)
        {
            //getting all the values
            DataTable dtOrderTypeServicesList = BLL.Common.UserInfo.LoadOrderTypeServices(CompanyId);
            DataView dv = new DataView(dtOrderTypeServicesList);
            dv.RowFilter = string.Format("OrderTypeID='{0}'", OrderType);
            return dv.ToTable();
         
        }
        public static DataTable LoadDatatableValuesForOrderTypeServicesTax(int OrderType, int CompanyId)
        {
            //getting all the values
            DataTable dtOrderTypeServicesTaxList = BLL.Common.UserInfo.LoadOrderTypeServicesTaxes(CompanyId);
            DataView dv = new DataView(dtOrderTypeServicesTaxList);
            dv.RowFilter = string.Format("OrderTypeID='{0}'", OrderType);
            return dv.ToTable();
        }

        public static int OpenOrdersExist(int CompanyId)
        {

            DataTable dataTable = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "OpenOrdersCountForPOS",
                      new SqlParameter("@companyId", CompanyId)).Tables[0];

            return int.Parse(dataTable.Rows[0][0].ToString());
        }

        public static bool EndDayOperation(int companyID)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "EndDayOperation", new SqlParameter("@CompanyID", companyID));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        //Sales Preview Section
        public static DataTable LoadSalesPreview(int CompanyId)
        {
            DataTable dtSalesPreview = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "LoadSalesPreview ",
                      new SqlParameter("@CompanyID", CompanyId)).Tables[0];
            return dtSalesPreview;
        }

        public static DataTable SpApiLoadSalesPreview(int CompanyId)
        {
            DataTable dtSalesPreview = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "SPApiLoadSalesPreview ",
                      new SqlParameter("@CompanyID", CompanyId)).Tables[0];
            return dtSalesPreview;
        }
        public static DataTable LoadSalesPaymentDetails(int CompanyId, int SalesMasterId)
        {
            DataTable dtSalesPaymentDetails = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "LoadSalesPaymentDetails ",
                       new SqlParameter("@SalesMasterID", SalesMasterId),
                       new SqlParameter("@CompanyID", CompanyId)).Tables[0];
            return dtSalesPaymentDetails;
        }

        public static void VoidBill(int voidId, string VoidReason, int SalesMasterId,int UserId, int CompanyId) 
        {
            decimal rst = Convert.ToDecimal(SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure,
                                        "[POS_VoidBill]",
                                     new SqlParameter("@salesMasterId", SalesMasterId)
                                    , new SqlParameter("@userID", UserId)
                                    , new SqlParameter("@VoidID", voidId)
                                    , new SqlParameter("@VoidReason", VoidReason)
                                    , new SqlParameter("@CompanyID", CompanyId)
                                     ));
           
        }

        public static decimal CheckSalesMasterExistForVoid(int SalesMasterId , int CompanyId)
        {
            decimal rst = Convert.ToDecimal(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure,
                                        "[POS_SalesMasterCheckForVoid]",
                                     new SqlParameter("@SalesMasterID", SalesMasterId)
                                    , new SqlParameter("@CompanyID", CompanyId)
                                     ));

            return rst;

        }

        public static void PaymentTypeUpdate(int SalesMasterId, int PaymentMethodID , int PaymentID)
        {
           SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure,
                                        "[PaymentTypeUpdate]",
                                     new SqlParameter("@salesMasterId", SalesMasterId)
                                    , new SqlParameter("@PaymentMethodID", PaymentMethodID)
                                    , new SqlParameter("@PaymentID", PaymentID)
                                     );

        }


        public static DataTable LoadPOSUserReadingConfiguration(int CompanyId_, int UserID_)
        {
            //getting all the values
            DataTable dataTable = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "POS_SELUserReadingConfigurationAPI",
                   new SqlParameter("@UserID", UserID_), new SqlParameter("@companyId", CompanyId_)).Tables[0];
            return dataTable;

        }

        public static DataSet POS_SEL_ReadingSummary(int CompanyId_, int UserID_, DateTime? EODDate_ = null, DateTime? ToDate_ = null)
        {
            //getting all the values
            DataSet ds = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "POS_SEL_ReadingSummaryAPI",
                   new SqlParameter("@companyId", CompanyId_), new SqlParameter("@EODDate", EODDate_),
                   new SqlParameter("@ToDate", ToDate_), new SqlParameter("@UserID", UserID_));
            ds.Tables[0].TableName = "POS_SEL_ReadingSummary";
            ds.Tables[1].TableName = "POS_SEL_ReadingTaxes";
            ds.Tables[2].TableName = "POS_SEL_ReadingServices";
            ds.Tables[3].TableName = "POS_SELReadingTipsTotal";
            ds.Tables[4].TableName = "POS_SEL_ReadingCategories";
            ds.Tables[5].TableName = "POS_SEL_ReadingPayments";
            ds.Tables[6].TableName = "POS_SEL_ReadingDiscount";
            ds.Tables[7].TableName = "POS_SEL_ReadingWorkstation";
            ds.Tables[8].TableName = "POS_SELReadingCheckList";
            ds.Tables[9].TableName = "POS_SELReadingMenu";
            ds.Tables[10].TableName = "POS_SEL_ReadingSalesByWaiter";
            ds.Tables[11].TableName = "POS_SEL_ReadingSalesByItem";
            ds.Tables[12].TableName = "POS_SEL_ReadingVoidSummary";
            ds.Tables[13].TableName = "POS_SELReadingVoidedItems";
            ds.Tables[14].TableName = "POS_SELReadingCreditCollection";
            ds.Tables[15].TableName = "POS_SEL_ReadingSalesByDivision";
            return ds;

        }

        public static DataTable GetDefaultPrinters(int PosID_, int CompanyID_)
        {
            DataTable defaultPrinters = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "POS_SELPosMachineDefaultPrinterAPI",
                                     new SqlParameter("@PosID", PosID_),
                                     new SqlParameter("@companyId", CompanyID_)).Tables[0];
            return defaultPrinters;

        }

    }
}
