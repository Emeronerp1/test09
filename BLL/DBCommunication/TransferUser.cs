using RakTDAApi.BLL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RakTDAApi.BLL.DBCommunication
{
    public class TransferUser :General
    {
        public static int POS_UPDTransferOrder(int OrderId, int CompanyId,int UserId)
        {
            int rst = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "[POS_UPDTransferOrder]"
              , new SqlParameter("@OrderID", OrderId)
               , new SqlParameter("@CompanyID", CompanyId)
               , new SqlParameter("@UserID", UserId)
              );
            return rst;          

        }

        public static DataTable GetUserList(int CompanyId)
        {
            DataSet dsUsersList = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "LoadUsers",
               new SqlParameter("@CompanyID", CompanyId));
            return dsUsersList.Tables[0];
        }

        public static DataTable GetUserOpenTables(int Userid, int CompanyId)
        {
            DataSet dsUsersList = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "POS_SEL_OpenOrdersByUser",
                        new SqlParameter("@CompanyID", CompanyId), new SqlParameter("@OrderType", (int)General.OrderType.DineIn),
                        new SqlParameter("@UserID", Userid));
            return dsUsersList.Tables[0];
        }
    }
}
