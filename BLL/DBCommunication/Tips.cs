using RakTDAApi.BLL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RakTDAApi.BLL.DBCommunication
{
    public class Tips : General
    {

        public static DataTable LoadTips(int CompanyId)
        {
            DataSet dtTips = new DataSet();
            dtTips = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "POS_SELTips",
            new SqlParameter("@CompanyID", CompanyId));
            return dtTips.Tables[0];

        }

        public static int CheckInvoiceNo(int CompanyId, int InvoiceNo)
        {

            int SalesMasterID = Convert.ToInt32(SqlHelper.ExecuteScalar(dbConStr,
                                    CommandType.StoredProcedure, "POS_SELSalesMasterID",
                                    new SqlParameter("@InvoiceNo", InvoiceNo),
                                    new SqlParameter("@CompanyID", CompanyId)));
            if (SalesMasterID <= 0)
            {
                return 0;
            }
            else
            {
                return SalesMasterID;
            }
        }

        public static int SaveTips(int CompanyId, int InvoiceNo, decimal TipAmount, int PaymentTypeId, string Narration,int UserId)
        {
            int rst = 0;
           int SalesMasterID = CheckInvoiceNo(CompanyId, InvoiceNo);
            if (SalesMasterID > 0)
            {
                rst = SqlHelper.ExecuteNonQuery(dbConStr,
                                        CommandType.StoredProcedure, "[POS_INSTips]",
                                        new SqlParameter("@InvoiceNo", InvoiceNo),
                                        new SqlParameter("@UserId", UserId),
                                        new SqlParameter("@PaymentTypeId", PaymentTypeId),
                                        new SqlParameter("@TipAmount", TipAmount),
                                        new SqlParameter("@SalesMasterID", SalesMasterID),
                                        new SqlParameter("@CompanyID", CompanyId),
                                        new SqlParameter("@PaymentId", 0),
                                        new SqlParameter("@Narration", Narration));
            }
            return rst;
        }

        public static int UpdateTips(int CompanyId, int InvoiceNo,int SalesMasterID ,decimal TipAmount, int PaymentTypeId, string Narration, int UserId,int PaymentDetailsId)
        {
            int rst = 0;
            if (SalesMasterID > 0)
            {
                rst = SqlHelper.ExecuteNonQuery(dbConStr,
                                        CommandType.StoredProcedure, "[POS_UpdTips]",
                                        new SqlParameter("@InvoiceNo", InvoiceNo),
                                        new SqlParameter("@UserId", UserId),
                                        new SqlParameter("@PaymentTypeId", PaymentTypeId),
                                        new SqlParameter("@TipAmount", TipAmount),
                                        new SqlParameter("@SalesMasterID", SalesMasterID),
                                        new SqlParameter("@CompanyID", CompanyId),
                                        new SqlParameter("@PaymentId", PaymentDetailsId),
                                        new SqlParameter("@Narration", Narration));
            }
            return rst;
        }
    }
}
