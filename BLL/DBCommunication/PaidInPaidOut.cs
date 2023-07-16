using RakTDAApi.BLL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RakTDAApi.BLL.DBCommunication
{
    public class PaidInPaidOut : General
    {
        
        public static int SaveTransaction(Models.Payloads.PaidInPaidOut paidInPaidOut)
        {
            // types are Paid In and Paid Out
            int rst = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "AddPaidInOutTransaction"
                , new SqlParameter("@type", paidInPaidOut.type)
                , new SqlParameter("@amount", paidInPaidOut.amount)
                , new SqlParameter("@narration", paidInPaidOut.narration)
                , new SqlParameter("@date", DateTime.Now)
                , new SqlParameter("@sessionid", 1)
                , new SqlParameter("@cashdrawerID", 1)
                , new SqlParameter("@time", DateTime.Now.ToString("hh:mm:ss tt"))
                , new SqlParameter("@CompanyID", paidInPaidOut.CompanyId)
                , new SqlParameter("@extra2", string.Empty)
                , new SqlParameter("@initid", 1)
                , new SqlParameter("@POSID", paidInPaidOut.POSID)
                , new SqlParameter("@UserID", paidInPaidOut.UserId)
                );
            return rst;
        }
    }
}
