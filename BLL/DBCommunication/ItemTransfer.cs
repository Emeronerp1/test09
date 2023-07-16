using RakTDAApi.BLL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RakTDAApi.BLL.DBCommunication
{
    public class ItemTransfer : General
    {
      

        public static DataTable TempSalesMasterView(int SalesMasterId, int CompanyId)
        {
            DataSet rst = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "TempPOSSalesMasterViewBySalesMasterId"
                , new SqlParameter("@SalesMasterID", SalesMasterId)
                 , new SqlParameter("@CompanyID", CompanyId)
                );
            return rst.Tables[0];
        }

        public static DataTable TempSalesDetailsView(int SalesMasterId, int CompanyId)
        {
            DataSet rst = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "TempSalesDetailsViewBySalesMasterId"
                , new SqlParameter("@SalesMasterID", SalesMasterId)
                 , new SqlParameter("@CompanyID", CompanyId)
                );
            return rst.Tables[0];
        }


        public static int DeleteSales(int SalesMasterId, int CompanyId)
        {
            int rst = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "[TempSalesMasterDelete]"
              , new SqlParameter("@salesMasterID", SalesMasterId)
               , new SqlParameter("@companyId", CompanyId)
              );
            return rst;

        }

        public static decimal GetSalesMasterId(decimal TableNo, int CompanyId)
        {
            decimal rst =Convert.ToDecimal( SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure, "GetSalesMasterIDforDineIn"
              , new SqlParameter("@OrderNo", TableNo)
               , new SqlParameter("@CompanyID", CompanyId))
              );
            return rst;

        }
        public static decimal GetTotalAmount(int SalesMasterId, int CompanyId)
        {
            decimal rst = Convert.ToDecimal(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure, "[SpApiGetTotalAmount]"
              , new SqlParameter("@SalesMasterId", SalesMasterId)
               , new SqlParameter("@CompanyID", CompanyId))
              );
            return rst;

        }
        public static int UpdateTempSalesDetails(int SalesMasterId, int CompanyId, int SalesDetailsId,decimal Qty)
        {
            int rst = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "[UpdateTempSalesDetails]"
              , new SqlParameter("@salesMasterId", SalesMasterId)
               , new SqlParameter("@CompanyID", CompanyId),
               new SqlParameter("@SalesDetailsId", SalesDetailsId),
                 new SqlParameter("@Qty", Qty)
              );
            return rst;

        }

        public static int ArrangeSlNoTempSalesDetails(int SalesMasterId, int CompanyId)
        {
            int rst = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "[ArrangeSlNoTempSalesDetails]"
              , new SqlParameter("@SalesMasterId", SalesMasterId)
               , new SqlParameter("@CompanyID", CompanyId)
              );
            return rst;

        }

        public static decimal CheckDiscountExists(decimal SalesMasterId, int CompanyId)
        {
            decimal rst = Convert.ToDecimal(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure, "[CheckDiscountExists]"
              , new SqlParameter("@SalesMasterId", SalesMasterId)
               , new SqlParameter("@CompanyID", CompanyId))
              );
            return rst;

        }


    }
}
