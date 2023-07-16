using RakTDAApi.BLL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RakTDAApi.BLL.DBCommunication.BackOffice.Reports
{
    public class DashBoardReports :General
    {
       
        public static DataTable GetDashboard_TodaysSales(int CompanyId)
        {
            DataSet dsTodaysSales = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "Dashboard_TodaysSales"
                   , new SqlParameter("@CompanyId", CompanyId));
            return dsTodaysSales.Tables[0];
        }

        public static DataTable GetDashboard_SalesBySevenDays(int CompanyId)
        {
            DataSet dsSalesBySevenDays = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "Dashboard_DailySalesBySevenDays"
                                   , new SqlParameter("@CompanyId", CompanyId));
            return dsSalesBySevenDays.Tables[0];
        }

        public static DataSet GetDashboard_CurrentVSLastMonthSales(int CompanyId)
        {
            DataSet dsSalesByCurLastMonth = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "Dashboard_CurrentAndLastMonthSales"
                                   , new SqlParameter("@CompanyId", CompanyId));
            dsSalesByCurLastMonth.Tables[0].TableName = "dtblCurrentMonthSales";
            dsSalesByCurLastMonth.Tables[1].TableName = "dtblLastMonthSales";

            return dsSalesByCurLastMonth;
        }


        public static DataTable GetDashboard_TodaysSalesByOrder(int CompanyId)
        {
            DataSet dsTodaysSalesByOrder = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "Dashboard_TodaysSalesByOrder"
                                   , new SqlParameter("@CompanyId", CompanyId));
            return dsTodaysSalesByOrder.Tables[0];
        }


        public static DataTable GetDashboard_TodaysPaymentType(int CompanyId)
        {
            DataSet dsTodaysPayType = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "Dashboard_TodaysPaymentTypes"
                   , new SqlParameter("@CompanyId", CompanyId));
            return dsTodaysPayType.Tables[0];
        }

        public static DataTable GetDashboard_Top5SellingItems(int CompanyId)
        {
            DataSet dsTop5SellingItems = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "Dashboard_Top5SellingProducts"
                                   , new SqlParameter("@CompanyId", CompanyId));
            return dsTop5SellingItems.Tables[0];
        }

        public static DataTable GetDashboard_Top5GrossingItems(int CompanyId)
        {
            DataSet dsTop5GrossingItems = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "Dashboard_Top5GrossingProducts"
                   , new SqlParameter("@CompanyId", CompanyId));
            return dsTop5GrossingItems.Tables[0];
        }

        public static DataTable GetDashboard_Top5SellingGroups(int CompanyId)
        {
            DataSet dsTop5SellingGroups = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "Dashboard_Top5SellingGroups"
                                   , new SqlParameter("@CompanyId", CompanyId));
            return dsTop5SellingGroups.Tables[0];
        }
              

        public static DataTable GetDashboard_Top5GrossingGroups(int CompanyId)
        {
            DataSet dsTop5GrossingGroups = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "Dashboard_Top5GrossingGroups"
                   , new SqlParameter("@CompanyId", CompanyId));
            return dsTop5GrossingGroups.Tables[0];
        }

        public static DataTable LoadSalesInfo(int CompanyId,int SalesMasterId)
        {
            DataSet dsSettings = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "[LoadSalesInfo]"
                 , new SqlParameter("@SalesMasterId", SalesMasterId)
                  , new SqlParameter("@CompanyId", CompanyId));
            return dsSettings.Tables[0];
        }
        public static DataTable LoadPaytermsInfo(int CompanyId,int SalesMasterId)
        {
            DataSet dsSettings = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "[LoadPayterms]"
                  , new SqlParameter("@CompanyId", CompanyId)
                  , new SqlParameter("@SalesMasterId", SalesMasterId));
            return dsSettings.Tables[0];
        }
        //public static DataTable LoadCustomersInfo(int CompanyId, int SalesMasterId)
        //{
        //    DataSet dsSettings = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "[LoadCustomersInfo]"
        //          , new SqlParameter("@SalesMasterId", SalesMasterId));
        //    return dsSettings.Tables[0];
        //}
        public static string GetOrderTypeName(int OrderTypeId)
        {
            string rst = Convert.ToString(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure, "[GetOrderTypeName]"
                 , new SqlParameter("@OrderTypeId", OrderTypeId)
                   ));
            return rst;
        }

        public static DataTable GetCompanyList()
        {
            DataSet dsCompanyList = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "CompanyViewList");
            return dsCompanyList.Tables[0];
        }





    }
}
