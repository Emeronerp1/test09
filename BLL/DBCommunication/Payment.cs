using RakTDAApi.BLL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RakTDAApi.BLL.DBCommunication
{
    public class Payment:General
    {

        public static decimal LoadDiscountAmount(int SalesMasterId) 
        {
            decimal rst = Convert.ToDecimal(SqlHelper.ExecuteScalar(dbConStr, CommandType.Text,
                                       "select isnull(sum(Isnull(DiscountAmount * Quantity, 0)), 0) from" +
                                       " tbl_PosOrderDiscounts where salesmasterId = " + SalesMasterId ));
            return rst;
        }

        public static DataTable ItemWiseDiscountAmount(int SalesMasterId,int CompanyId)
        {
            var rst = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure,
                "POS_SEL_SumofItemWiseDiscount",
                 new SqlParameter("@SalesMasterID", SalesMasterId),
                 new SqlParameter("@CompanyID", CompanyId));
            return rst.Tables[0];
        }

        public static DataTable LoadDiscountDetails(int SalesMasterId,int CompanyId)
        {
            DataSet ds = BLL.Common.SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "LoadPaymentDiscountPercentageGrid",
             new SqlParameter("@SalesMasterID", SalesMasterId),
             new SqlParameter("@CompanyID", CompanyId));
            return ds.Tables[0];
        }

        public static decimal LoadDiscountedItems(int SalesMasterId, int CompanyId, bool InclusiveTax)
        {
            decimal ds =Convert.ToDecimal(BLL.Common.SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure, "POS_SEL_DiscountedItems",
             new SqlParameter("@SalesMasterID", SalesMasterId),
             new SqlParameter("@CompanyID", CompanyId),
             new SqlParameter("@InclusiveTax", InclusiveTax)));
            return ds;
        }

        public static DataTable ApplicableDiscountedItems(int SalesMasterId, int CompanyId)
        {
            DataSet ds = BLL.Common.SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "POS_SEL_DiscountedItemsApplicable",
             new SqlParameter("@SalesMasterID", SalesMasterId),
             new SqlParameter("@CompanyID", CompanyId));
            return ds.Tables[0];
        }

        //Load Discount type on By Amount;
        public static DataTable DiscountByAmount()
        {
            DataSet ds = BLL.Common.SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "SPApiPOS_SEL_DiscountedItemsByAmount");
            
            return ds.Tables[0];
        }

        //Load Discount type on By Percentage;
        public static DataTable DiscountByPercentage()
        {
            DataSet ds = BLL.Common.SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "SPApiPOS_SEL_DiscountedItemsByPercentage");

            return ds.Tables[0];
        }

        //Clear Discounts from Payment
        public static int ClearDiscounts(int SalesMasterId,int CompanyId,bool IsInclusive)
        {
            int rst = BLL.Common.SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "ClearDiscountsFromPayments",
                 new SqlParameter("@SalesMasterID", SalesMasterId),
                 new SqlParameter("@CompanyID", CompanyId),
                 new SqlParameter("@IsInclusive",IsInclusive));
            return rst;
        }

        // Load Net Amount function need to check whether grand total value can take from loadvalues ;


        public static DataTable LoadValuesForPayment(int SalesMasterId, int CompanyId)
        {
            DataSet ds = BLL.Common.SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "[SPApiLoadValuesForPayment]",
              new SqlParameter("@SalesMasterID", SalesMasterId),
             new SqlParameter("@CompanyID", CompanyId));
            
            return ds.Tables[0];
        }

        public static DataTable LoadTaxDetails(int SalesMasterId, int CompanyId, int OrderTypeId)
        {
            DataSet ds = BLL.Common.SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "LoadTaxDetails",
             new SqlParameter("@SalesMasterID", SalesMasterId),
             new SqlParameter("@CompanyID", CompanyId),
             new SqlParameter("@OrderTypeID", OrderTypeId));
            return ds.Tables[0];
        }

        // Payment Methods are in CreditCollection

        public static int SetCustomer(int SalesMasterId,int CustomerId, int CompanyId)
        {
            var rst = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "[UpdateSetCustomerinSalesMaster]"
                 , new SqlParameter("@SalesMasterID", SalesMasterId)
                 , new SqlParameter("@CompanyID", CompanyId)
                 , new SqlParameter("@CustomerID", CustomerId)
                );
            int rs = Convert.ToInt32(rst);
            return rs;
        }

        public static int TempSalesMasterEditForSettlementUpdate(int SalesMasterId, int SettledBy,int CompanyId,int CustomerId)
        {

            int rst = Convert.ToInt32(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure,
                                        "SPApiTempSalesMasterEditForSettlementUpdate",
                                      new SqlParameter("@SalesMasterId", SalesMasterId)
                                    , new SqlParameter("@settledby", SettledBy)
                                    , new SqlParameter("@CompanyID", CompanyId)
                                    , new SqlParameter("@ClosedDate", DateTime.Now.ToString("yyyy - MM - dd hh: mm:ss tt"))
                                    , new SqlParameter("@CreditCustomerId", CustomerId)));
            return rst;
        }

        public static int InsertCustomerIntoAccountLedger(int CustomerId, int CompanyId)
        {
            var rst = SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure, "[InsertCustomerIntoAccountLedger]"
                 , new SqlParameter("@CompanyID", CompanyId)
                 , new SqlParameter("@CustomerID", CustomerId)
                );
            int rs = Convert.ToInt32(rst);
            return rs;
        }

        public static int POS_UpdateTips(int SalesMasterId, decimal Tips, int CompanyId)
        {
            var rst = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "[POS_UpdateTips]"
                 , new SqlParameter("@SalesMasterID", SalesMasterId)
                 , new SqlParameter("@CompanyID", CompanyId)
                 , new SqlParameter("@Tips", Tips)
                );
            int rs = Convert.ToInt32(rst);
            return rs;
        }

        public static int SavePaymentDetails(Models.Payloads.Payment payment)
        {
            var rst = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "[TempPaymentDetailsAdd]"
                 , new SqlParameter("@SalesMasterID", payment.salesMasterId)
                 , new SqlParameter("@PaymentMethodID", payment.PaymentMethodId)
                 , new SqlParameter("@due", payment.due)
                 , new SqlParameter("@tendered", payment.tendered)
                 , new SqlParameter("@change", payment.change)
                 , new SqlParameter("@cardno", payment.cardno)
                 , new SqlParameter("@cardname", payment.cardname)
                 , new SqlParameter("@date", DateTime.Now)
                 , new SqlParameter("@userId", payment.userid)
                 , new SqlParameter("@extraDate", DateTime.Now)
                 , new SqlParameter("@CompanyID", payment.CompanyId)
                 , new SqlParameter("@extra2", string.Empty)
                );
            int rs = Convert.ToInt32(rst);
            return rs;
        }

        public static int InsertDiscount(Models.Payloads.POSOrderDiscounts OrderDiscounts,bool IsInclusive)
        {
            var rst = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "[POS_INSOrderDiscount]"
                 , new SqlParameter("@SalesMasterID", OrderDiscounts.SalesMasterID)
                 , new SqlParameter("@CompanyID", OrderDiscounts.CompanyID)
                 , new SqlParameter("@SalesDetailsID", OrderDiscounts.SalesDetailsID)
                 , new SqlParameter("@DiscountID", OrderDiscounts.DiscountID)
                 , new SqlParameter("@DiscountTypeID", OrderDiscounts.DiscountTypeID)
                 , new SqlParameter("@DiscountValue", OrderDiscounts.DiscountValue)
                 , new SqlParameter("@DiscountAmount", OrderDiscounts.DiscountAmount)
                 , new SqlParameter("@Quantity", OrderDiscounts.Quantity)
                 , new SqlParameter("@UserId", OrderDiscounts.UserId)
                 , new SqlParameter("@NetAmount", OrderDiscounts.NetAmount)
                 , new SqlParameter("@IsInclusive", IsInclusive)
                );
            int rs = Convert.ToInt32(rst);
            return rs;
        }

        //using in posmenu form while updating item wise discount;
        public static int DiscountOrderUpdate(Models.Payloads.POSOrderDiscounts OrderDiscounts, bool IsInclusive)
        {
            var rst = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "[POS_UPDOrderDiscount]"
                 , new SqlParameter("@SalesMasterID", OrderDiscounts.SalesMasterID)
                 , new SqlParameter("@CompanyID", OrderDiscounts.CompanyID)
                 , new SqlParameter("@SalesDetailsID", OrderDiscounts.SalesDetailsID)
                 , new SqlParameter("@DiscountID", OrderDiscounts.DiscountID)
                 , new SqlParameter("@DiscountTypeID", OrderDiscounts.DiscountTypeID)
                 , new SqlParameter("@DiscountValue", OrderDiscounts.DiscountValue)
                 , new SqlParameter("@DiscountAmount", OrderDiscounts.DiscountAmount)
                 , new SqlParameter("@Quantity", OrderDiscounts.Quantity)
                 , new SqlParameter("@UserId", OrderDiscounts.UserId)
                 , new SqlParameter("@NetAmount", OrderDiscounts.NetAmount)
                 , new SqlParameter("@IsInclusive", IsInclusive)
                );
            int rs = Convert.ToInt32(rst);
            return rs;
        }

         //
        public static decimal UpdateExclusiveDiscount(int SalesmasterId, int SalesDetailsID, decimal ItemPrice, decimal DiscountedPrice)
        {
            decimal rst = Convert.ToDecimal(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure, "[UpdateExclusiveDiscount]"
                 , new SqlParameter("@SalesMasterID", SalesmasterId)
                 , new SqlParameter("@SalesDetailsID", SalesDetailsID)
                 , new SqlParameter("@ItemPrice", ItemPrice)
                 , new SqlParameter("@DiscountedPrice", DiscountedPrice)));
            return rst;
        }

        public static decimal UpdateInclusiveDiscount(int SalesmasterId, int SalesDetailsID, decimal ItemPrice, decimal DiscountedPrice,int CompanyId)
        {
            decimal rst = Convert.ToDecimal(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure, "[POS_UpdtInclusiveDiscount]"
                 , new SqlParameter("@SalesMasterID", SalesmasterId)
                 , new SqlParameter("@SalesDetailsID", SalesDetailsID)
                 , new SqlParameter("@ItemPrice", ItemPrice)
                 , new SqlParameter("@DiscountedPrice", DiscountedPrice)
                 , new SqlParameter("@CompanyID", CompanyId)));
            return rst;
        }


        public static int CheckInvoiceStatus(int SalesmasterId,int CompanyId)
        {
            int rst = Convert.ToInt32(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure, "[CheckInvoiceStatus]"
                 , new SqlParameter("@SalesMasterID", SalesmasterId)
                 , new SqlParameter("@CompanyID", CompanyId)));
            return rst;
        }

        public static DataTable LoadPaymentTypes()
        {
            DataSet dsSettings = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "POSPaymentTypes");
            return dsSettings.Tables[0];
        }
        public static decimal DiscountInsert(int CompanyId, int SalesMasterId, int SalesdetailsID, int DiscountID,
            int DiscountTypeID, decimal DiscountValue, decimal DiscountAmount, decimal Quantity, int UserID, decimal NetAmount,bool IsInclusive)
        {

            decimal rst = Convert.ToDecimal(SqlHelper.ExecuteScalar(dbConStr,CommandType.StoredProcedure, "[POS_INSOrderDiscount]"
              , new SqlParameter("@CompanyID", CompanyId)
              , new SqlParameter("@SalesMasterID", SalesMasterId)
              , new SqlParameter("@SalesDetailsID", SalesdetailsID)
              , new SqlParameter("@DiscountID", DiscountID)
              , new SqlParameter("@DiscountTypeID", DiscountTypeID)
              , new SqlParameter("@DiscountValue", DiscountValue)
              , new SqlParameter("@DiscountAmount", DiscountAmount)
              , new SqlParameter("@Quantity", Quantity)
              , new SqlParameter("@UserId", UserID)
              , new SqlParameter("@NetAmount", NetAmount)
              , new SqlParameter("@IsInclusive", IsInclusive)));
            return rst;
            }
       

        


        //Calculating ItemPrice
        public static void CalcItemPrice(int SalesmasterId,int CompanyId,decimal CurrentDiscountValue)
        {
           
                decimal SellingPrice, TotalValue, TotalDiscountApplied,
                DistributedValue, DiscoutDistribution, NetItemSellingPrice, TotalTaxApplied, NetItemPrice, TotalItemWiseDiscount = 0;
                DataTable dtApplicableItemsSalesDetailsId = new DataTable();
                dtApplicableItemsSalesDetailsId = ApplicableDiscountedItems(SalesmasterId,CompanyId);
                DataTable dtAppliedItemWiseDiscount = ItemWiseDiscountAmount(SalesmasterId,CompanyId);
                if (dtAppliedItemWiseDiscount.Rows.Count > 1)
                {
                    TotalItemWiseDiscount = Convert.ToDecimal(dtAppliedItemWiseDiscount.Rows[0][0].ToString());
                }
                for (int i = 0; i < dtApplicableItemsSalesDetailsId.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dtApplicableItemsSalesDetailsId.Rows[i]["AffectedItem"]) == 0)
                    {
                    if (Convert.ToInt32(dtApplicableItemsSalesDetailsId.Rows[i]["Discount"]) == 1)
                    {
                        SellingPrice = Convert.ToDecimal(dtApplicableItemsSalesDetailsId.Rows[i]["SellingPrice"].ToString());// * Convert.ToDecimal(dtApplicableItemsSalesDetailsId.Rows[i]["Qty"].ToString());
                        TotalValue = Convert.ToDecimal(dtApplicableItemsSalesDetailsId.Rows[i]["TotalSalesValue"].ToString()) - TotalItemWiseDiscount;
                        TotalDiscountApplied = CurrentDiscountValue - TotalItemWiseDiscount;
                        if (SellingPrice > 0 && TotalValue > 0)
                        {
                            DistributedValue = (SellingPrice / TotalValue);
                            DiscoutDistribution = TotalDiscountApplied * DistributedValue;
                            NetItemSellingPrice = SellingPrice - DiscoutDistribution;
                            TotalTaxApplied = Convert.ToDecimal(dtApplicableItemsSalesDetailsId.Rows[i]["TotalTaxesApplied"].ToString());
                            NetItemPrice = Math.Round(NetItemSellingPrice / TotalTaxApplied, 2);
                            UpdateInclusiveDiscount(SalesmasterId, (int)Convert.ToDecimal(dtApplicableItemsSalesDetailsId.Rows[i]["SalesdetailsID"].ToString()), NetItemPrice, NetItemSellingPrice, CompanyId);
                        }
                    }

                    }
                }

            
        }



        public static void QuickCashSettlement(int CompanyId, int UserId, int SalesMasterId)
        {
            Models.Payloads.Payment payment = new Models.Payloads.Payment();
            payment.userid = UserId; ;
            payment.CompanyId = CompanyId;
            payment.cardname = "";
            payment.cardno = "";
            payment.due = 0;
            payment.PayCategoryID = 1; // CASH
            payment.PaymentMethodId = 1; // CASH
            payment.tendered = 0;
            payment.change = 0;
            payment.salesMasterId = SalesMasterId;
            int SavePaymentDetails = BLL.DBCommunication.Payment.QuickCashPaymentDetails(payment);
            int rstSettled = BLL.DBCommunication.Payment.TempSalesMasterEditForSettlementUpdate(SalesMasterId, UserId, CompanyId,0);
           
        }
        public static void BillSettlement(int CompanyId, int UserId, int SalesMasterId, int PayCategoryID, int PaymentMethodId)
        {
            Models.Payloads.Payment payment = new Models.Payloads.Payment();
            payment.userid = UserId; ;
            payment.CompanyId = CompanyId;
            payment.cardname = "";
            payment.cardno = "";
            payment.due = 0;
            payment.PayCategoryID = PayCategoryID; // CASH
            payment.PaymentMethodId = PaymentMethodId; // CASH
            payment.tendered = 0;
            payment.change = 0;
            payment.salesMasterId = SalesMasterId;
            int SavePaymentDetails = BLL.DBCommunication.Payment.QuickCashPaymentDetails(payment);
            int rstSettled = BLL.DBCommunication.Payment.TempSalesMasterEditForSettlementUpdate(SalesMasterId, UserId, CompanyId, 0);

        }
        public static int QuickCashPaymentDetails(Models.Payloads.Payment payment)
        {
            var rst = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "[SPApiTempPaymentDetailsAdd]"
                 , new SqlParameter("@SalesMasterID", payment.salesMasterId)
                 , new SqlParameter("@PaymentMethodID", payment.PaymentMethodId)
                 , new SqlParameter("@due", payment.due)
                 , new SqlParameter("@tendered", payment.tendered)
                 , new SqlParameter("@change", payment.change)
                 , new SqlParameter("@cardno", payment.cardno)
                 , new SqlParameter("@cardname", payment.cardname)
                 , new SqlParameter("@date", DateTime.Now)
                 , new SqlParameter("@userId", payment.userid)
                 , new SqlParameter("@extraDate", DateTime.Now)
                 , new SqlParameter("@CompanyID", payment.CompanyId)
                 , new SqlParameter("@extra2", string.Empty)
                );
            int rs = Convert.ToInt32(rst);
            return rs;
        }
    }
}
