using RakTDAApi.BLL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RakTDAApi.BLL.DBCommunication
{
    public class AssignandSettleDrivers : General
    {
        public static DataTable LoadDrivers(int CompanyId)
        {
            DataSet dsSettings = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "EmployeeViewByDesignationAndStatus"
                , new SqlParameter("@designationName", "Driver")
                , new SqlParameter("@status", "Active")
                , new SqlParameter("@companyId", CompanyId));
            return dsSettings.Tables[0];
        }

        public static DataTable LoadOpenOrdersPendingType(int OrderType,int CompanyId)
        {
            DataSet dsSettings = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "LoadOpenOrdersPendingType"
               , new SqlParameter("@OrderTypeID", OrderType)
               , new SqlParameter("@CompanyID", CompanyId));
            return dsSettings.Tables[0];
        }

        public static DataTable AssignedOrders(int CompanyId)
        {
            DataSet dsSettings = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "LoadAssignedOrders"
               , new SqlParameter("@CompanyID", CompanyId));
            return dsSettings.Tables[0];
        }

        public static int DriverAssign(Models.Payloads.AssignDrivers assignDrivers)
        {
            int rst = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "TempSalesMasterEditForDriverAssign"
                , new SqlParameter("@tempsalesMasterId", assignDrivers.SalesMasterId)
                , new SqlParameter("@DeliverydriverID", assignDrivers.DriverId)
                , new SqlParameter("@statusofdelivery", assignDrivers.StatusofDelivery)
                , new SqlParameter("@TimeDeliveryStart", assignDrivers.TimeDeliveryStart)
                , new SqlParameter("@TimeDeliveryFinish", assignDrivers.TimeDeliveryFinish)
                 , new SqlParameter("@CompanyID", assignDrivers.CompanyId)
                );
            return rst;
        }

        public static DataTable LoadDriverOrderReport(int CompanyId)
        {
            DataSet dsSettings = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "LoadDriverOrderReportAll"
               , new SqlParameter("@CompanyID", CompanyId));
            return dsSettings.Tables[0];
        }

        public static decimal GetNetTotalAmount(int SalesMasterId,int CompanyId)
        {
            decimal rst =Convert.ToDecimal(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure, "[GetNetTotalAmount]"
              , new SqlParameter("@CompanyId", CompanyId)
               , new SqlParameter("@SalesMasterId", SalesMasterId)
              ));
            return rst;

        }
        public static int DeliveryDriverUpdateForSettlement (int SalesMasterId,int DriverId,int CompanyId)
        {
            int rst = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "[DeliveryDriverUpdateForSettlement]"
                , new SqlParameter("@SalesMasterId", SalesMasterId)
                , new SqlParameter("@DriverId", DriverId)
                 , new SqlParameter("@CompanyID", CompanyId)
                );
            return rst;
        }

    }
}
