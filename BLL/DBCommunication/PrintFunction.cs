using RakTDAApi.BLL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RakTDAApi.BLL.DBCommunication
{
    public class PrintFunction : General
    {
        public static DataSet PrintBill(int salesMasterID, int CompanyId)
        {
            DataSet OrderDataSet = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "TempsalesInvoicePrintAfterSaveAPI",
                                        new SqlParameter("@companyId", CompanyId), new SqlParameter("@salesMasterId", salesMasterID));
            OrderDataSet.Tables[0].TableName = "dtblCompany";
            OrderDataSet.Tables[1].TableName = "dtblSalesMaster";
            OrderDataSet.Tables[2].TableName = "dtblSalesDetails";
            OrderDataSet.Tables[3].TableName = "POS_SELOrderDiscountBillReport";
            OrderDataSet.Tables[4].TableName = "POS_SELOrderServicesBillReport";
            OrderDataSet.Tables[5].TableName = "POS_SELOrderTaxesBillReport";
            OrderDataSet.Tables[6].TableName = "POS_SELOrderPaymentBillReport";
            OrderDataSet.Tables[7].TableName = "dtblCustomerDetails";
            OrderDataSet.Tables[8].TableName = "dtblInvoiceMessages";
            OrderDataSet.Tables[9].TableName = "dtblAgentName";
            return OrderDataSet;
        }

        public static int UpdatetempSalesMasterPrintCount(int salesMasterId_, int CompanyID_)
        {
            int rowCount;
            try
            {
                rowCount = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "UpdatetempSalesMasterPrintCount",
                                  new SqlParameter("@salesMasterId", salesMasterId_),
                                  new SqlParameter("@CompanyID", CompanyID_));
                return rowCount;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static DataTable GetBillPrinters(int salesMasterId_, int PosID_, int CompanyID_)
        {
            DataTable OrderPrinters = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "POS_SELPosMachinePrinterSelectAPI",
                                     new SqlParameter("@salesMasterId", salesMasterId_),
                                     new SqlParameter("@PosID", PosID_),
                                     new SqlParameter("@companyId", CompanyID_)).Tables[0];
            return OrderPrinters;

        }
    }
}
