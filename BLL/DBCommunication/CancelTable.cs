using RakTDAApi.BLL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RakTDAApi.BLL.DBCommunication
{
    public class CancelTable:General
    {
        public static int CheckForExistingDining(decimal CancelOrderNo,int CompanyId)
        {
            int rst = Convert.ToInt32(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure, "SPApiCheckForExistingDining"
                , new SqlParameter("@OrderNo", CancelOrderNo)
                 , new SqlParameter("@CompanyID", CompanyId)
                ));
          
            return rst;
        }

    

        public static DataTable LoadVoidReasonList()
        {
            DataSet dsSettings = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "POS_SELVoidReasonList");
            return dsSettings.Tables[0];
        }

        public static int CancelTableOrder(Models.Payloads.CancelTable cancelTable)
        {
            var rst = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "POS_CancelTable"
                , new SqlParameter("@CancelTableNo", cancelTable.CancelTableNo)
                , new SqlParameter("@SalesMasterID", cancelTable.SalesMasterID)
                , new SqlParameter("@UserID", cancelTable.UserID)
                , new SqlParameter("@CompanyID", cancelTable.CompanyId)
                , new SqlParameter("@VoidID", cancelTable.VoidId)
                , new SqlParameter("@POSID", cancelTable.PosId)
                );
            int rs = Convert.ToInt32(rst);
            return rs;
        }

    }
}
