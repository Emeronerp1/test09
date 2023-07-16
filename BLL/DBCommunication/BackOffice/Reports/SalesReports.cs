using RakTDAApi.BLL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RakTDAApi.BLL.DBCommunication.BackOffice.Reports
{
    public class SalesReports : General
    {
        public static DataTable GetBOF_DailySalesChart(int CompanyId, DateTime DateFrom, DateTime DateTo)
        {
            DataSet dsDailySalesChart = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "BOF_DailySalesChart"
                   , new SqlParameter("@CompanyId", CompanyId), new SqlParameter("@EODDate", DateFrom)
                   , new SqlParameter("@ToDate", DateTo));
            return dsDailySalesChart.Tables[0];
        }

        public static DataTable GetBOF_HourlySalesChartByPrice(int CompanyId, DateTime DateFrom, DateTime DateTo)
        {
            DataSet dsHourlySalesChartByPrice = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "BOF_HourlySalesChartByPrice"
                   , new SqlParameter("@CompanyId", CompanyId), new SqlParameter("@EODDate", DateFrom)
                   , new SqlParameter("@ToDate", DateTo));
            return dsHourlySalesChartByPrice.Tables[0];
        }

        public static DataTable GetBOF_DaysofWeekSalesChartByPrice(int CompanyId, DateTime DateFrom, DateTime DateTo)
        {
            DataSet dsDaysofWeekSalesChart = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "BOF_DayOfWeekSalesChart"
                   , new SqlParameter("@CompanyId", CompanyId), new SqlParameter("@EODDate", DateFrom)
                   , new SqlParameter("@ToDate", DateTo));
            return dsDaysofWeekSalesChart.Tables[0];
        }
    }
}
