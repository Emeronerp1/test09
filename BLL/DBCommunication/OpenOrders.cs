using RakTDAApi.BLL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RakTDAApi.BLL.DBCommunication
{
    public class OpenOrders : General
    {
        public static DataTable LoadOpenOrders(int CompanyId)
        {
            DataSet dsSettings = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "SpApiLoadOpenOrdersByOrderType"
                , new SqlParameter("@CompanyID", CompanyId));
           
            return dsSettings.Tables[0];
        }

        public static int CheckLockTableExist(decimal TableNo, int CompanyId, int UserId)
        {
            int CheckLockTableExist = Convert.ToInt32(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure,
                                        "CheckLockTableExist",
                                     new SqlParameter("@TableNo", TableNo)
                                    , new SqlParameter("@CompanyID", CompanyId)
                                    , new SqlParameter("@UserID", UserId)));

            return CheckLockTableExist;
        }
    }
}
