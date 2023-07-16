using RakTDAApi.BLL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RakTDAApi.BLL.DBCommunication
{
    public class PosMenu:General
    {
        public static DataTable GetMenuItems(int CompanyID, int ScreenID, int ScreenItemType,bool IsInclusiveTax)
        { 
              // initilally the value of ScreenId=1 and ScreenItemType=3;
             
               DataSet ds= BLL.Common.SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "SPApiPOS_SELScreens",
                new SqlParameter("@BranchID", CompanyID),
                new SqlParameter("@ParentID", ScreenID), 
                new SqlParameter("@ItemTypeID", ScreenItemType),
                new SqlParameter("@IsInclusiveTax", IsInclusiveTax));
            return ds.Tables[0];
        }

        public static DataTable GetCategories(int OrderTypeId,int CompanyId,int UserId)
        {
            DataSet ds = BLL.Common.SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "[SPApiGetCategories]",
                 new SqlParameter("@UserId", UserId),
                new SqlParameter("@OrderTypeId", OrderTypeId),
                new SqlParameter("@CompanyId", CompanyId) );
            return ds.Tables[0];
        }

        public static DataTable GetModifiers(int Modifier, int ProductId, int CompanyID,int PriceMode,int OrderTypeId,bool TaxInclusive)
        {
            DataSet ds = BLL.Common.SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "[SPApiPOSProductModifierView]",
            new SqlParameter("@Modifier", Modifier),
            new SqlParameter("@CompanyId", CompanyID),
             new SqlParameter("@ProductId", ProductId));
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                DataTable dtSalesPrices = new DataTable();
                decimal SalesRate = BLL.DBCommunication.PosMenu.GetSalesRate(int.Parse(dr["productId"].ToString()), PriceMode, CompanyID);
                dtSalesPrices = BLL.DBCommunication.PosMenu.InclusiveItemPrice(CompanyID, OrderTypeId, int.Parse(dr["productId"].ToString()), SalesRate, TaxInclusive);
                dr["SalesRateBeforeTaxes"] = Convert.ToDecimal(dtSalesPrices.Rows[0]["ItemPriceWithoutTaxes"].ToString());
                dr["SalesRateAfterTaxes"] = Convert.ToDecimal(dtSalesPrices.Rows[0]["ItemPriceWithTaxes"].ToString());
                dr["TotalTaxesApplied"] = Convert.ToDecimal(dtSalesPrices.Rows[0]["TotalTaxesApplied"].ToString());
                dr["Rate"] = Convert.ToDecimal(dtSalesPrices.Rows[0]["ItemPriceWithoutTaxes"].ToString());
                dr["SellingPrice"] = Convert.ToDecimal(dtSalesPrices.Rows[0]["ItemPriceWithTaxes"].ToString());
            }
            return ds.Tables[0];
        }

        public static DataTable GetPrinters(int ProductId, int POSId, int CompanyId)
        {
            DataSet ds = BLL.Common.SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "spapiKitchenPrinters",
            new SqlParameter("@ProductId", ProductId), new SqlParameter("@PosId", POSId),
            new SqlParameter("@CompanyId", CompanyId));
            return ds.Tables[0];
        }


        public static DataTable OpenOrderMasterLoad(int SalesMasterId,int CompanyId)
        {            
             DataSet ds = BLL.Common.SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "TempPOSSalesMasterViewBySalesMasterId",
             new SqlParameter("@salesMasterId", SalesMasterId),
             new SqlParameter("@companyId", CompanyId));
             return ds.Tables[0];
        }
        public static DataTable OpenOrderDetailsLoad(int SalesMasterId,int CompanyId)
        {
            DataSet ds = BLL.Common.SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "TempSalesDetailsViewBySalesMasterId",
             new SqlParameter("@salesMasterId", SalesMasterId),
             new SqlParameter("@companyId", CompanyId));
            return ds.Tables[0];
        }
        public static DataTable OpenOrderDiscountDetailsLoad(int SalesMasterId, int CompanyId)
        {
            DataSet ds = BLL.Common.SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "LoadDiscountedItemsOnPOSMenu",
             new SqlParameter("@SalesMasterID", SalesMasterId),
             new SqlParameter("@CompanyID", CompanyId));
            return ds.Tables[0];
        }

        public static int GetTableNo(int SalesMasterId, int CompanyId)
        {
            var rst = SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure, "[GetTableNo]"
                , new SqlParameter("@SalesMasterID", SalesMasterId)
                 , new SqlParameter("@CompanyID", CompanyId)
                );
            int rs = Convert.ToInt32(rst);
            return rs;
        }

        public static bool IsModifierOrRemarks(int ProductId, int CompanyId)
        {
            var rst = SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure, "SpApiCheckModifierOrRemark"
                , new SqlParameter("@productId", ProductId)
                 , new SqlParameter("@companyId", CompanyId)
                );
            bool rs = Convert.ToBoolean(rst);
            return rs;
        }

        public static void LockTable(decimal TableNo,int CompanyId,int UserId)
        {

            int rst= Convert.ToInt32(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure,
                                        "POS_LOCKTABLE_Insert",
                                     new SqlParameter("@TableNo", TableNo)
                                    , new SqlParameter("@LockStatus", true)
                                    , new SqlParameter("@CompanyID", CompanyId)
                                    , new SqlParameter("@UserID", UserId),
                                     new SqlParameter("@OrderTypeID",1)));

            //return rst;
        }
        public static void UnLockTable(decimal TableNo, int CompanyId)
        {

            int rst = Convert.ToInt32(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure,
                                        "POS_LOCKTABLE_Delete",
                                     new SqlParameter("@TableNo", TableNo)
                                    , new SqlParameter("@CompanyID", CompanyId)));            
        }

        public static int GetPriceMode(int OrderTypeId,int CompanyId)
        {
            int rst = Convert.ToInt32(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure,
                                        "[SPApiPriceMode]",
                                     new SqlParameter("@OrderTypeId", OrderTypeId)
                                    , new SqlParameter("@CompanyId", CompanyId)));
            return rst;
        }

        public static decimal GetSalesRate(int ProductId,int PriceMode,int CompanyId) // for checking happy hours and getting price
        {
            decimal rst = Convert.ToDecimal(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure,
                                        "[SPApiGetSalesRate]",
                                     new SqlParameter("@ProductId", ProductId)
                                    , new SqlParameter("@CompanyId", CompanyId)
                                     , new SqlParameter("@PriceMode", PriceMode)
                                     ));
            return rst;
        }

        public static DataTable InclusiveItemPrice(int CompanyId, int OrderTypeId, int ProductId,decimal SalesRate,bool IsInclusive) // for checking happy hours and getting price
        {
            DataSet rst = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure,
                                        "[SPApiInclusivePrice]",
                                        new SqlParameter("@ProductId", ProductId)
                                    ,   new SqlParameter("@CompanyId", CompanyId)
                                     , new SqlParameter("@OrderTypeId", OrderTypeId)
                                     , new SqlParameter("@ProductPrice", SalesRate)
                                     , new SqlParameter("@IsInclusive", IsInclusive)
                                     );
            return rst.Tables[0];
        }

        //public static void SetFireOrHoldStatus(int SalesDetailsId,int Case)
        //{
        //    if (SalesDetailsId>0 && Case == 1)
        //    {
        //        SqlHelper.ExecuteNonQuery
        //       (dbConStr, CommandType.Text,
        //       "Update tbl_Tempsalesdetails set FireOrHold='FIRE' where salesdetailsId=" + SalesDetailsId);
        //    }
        //    else if(SalesDetailsId>0 && Case==2)
        //    {
        //        SqlHelper.ExecuteNonQuery
        //       (dbConStr, CommandType.Text,
        //       "Update tbl_Tempsalesdetails set FireOrHold='HOLD' where salesdetailsId=" + SalesDetailsId);
        //    }
        //}

        public static void InsertTaxesAndServices(int CompanyId,int OrderTypeId,int SalesMasterId,bool TaxInclusive)
        {
            SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure,
                                        "[SPApiInsertTaxesAndServices]",
                                      new SqlParameter("@CompanyId", CompanyId)
                                    , new SqlParameter("@OrderTypeId", OrderTypeId)
                                    //, new SqlParameter("@TotalTaxableAmount", TotalTaxableAmount)
                                    , new SqlParameter("@SalesMasterId", SalesMasterId)
                                    , new SqlParameter("@TaxInclusive",TaxInclusive));
        }


        public static int UpdateSalesDetailItemPrint(int salesDetailsId_, int CompanyID_)
        {
            int rowCount;
            try
            {
                rowCount = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "tempSalesDetailPrintKOT",
                                   new SqlParameter("@salesDetailsId", salesDetailsId_),
                                   new SqlParameter("@CompanyID", CompanyID_));
                return rowCount;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static int UpdateKitchenRemarks(int salesMasterId_, string KitchenRemarks_, int CompanyID_)
        {
            int rowCount;
            try
            {
                rowCount = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "UpdatetempSalesMasterKitchenRemarks",
                                   new SqlParameter("@salesMasterId", salesMasterId_),
                                   new SqlParameter("@narration", KitchenRemarks_),
                                   new SqlParameter("@CompanyID", CompanyID_));
                return rowCount;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static int UpdateInvoiceRemarks(int salesMasterId_, string invoiceRemark_, int CompanyID_)
        {
            int rowCount;
            try
            {
                rowCount = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "UpdatetempSalesMasterInvoiceRemarks",
                                  new SqlParameter("@salesMasterId", salesMasterId_),
                                  new SqlParameter("@invoiceRemark", invoiceRemark_),
                                  new SqlParameter("@CompanyID", CompanyID_));
                return rowCount;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static DataTable GetDeliveryCharges(int AgentId_, int CompanyId_) // 
        {
            DataTable rst = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "POS_SELDeliveryCharges",
                                       new SqlParameter("@AgentId", AgentId_)
                                     , new SqlParameter("@companyId", CompanyId_)
                                     ).Tables[0];
            return rst;
        }

        public static DataTable GetAgentDeliveryCharges(int AgentId_, int CompanyId_) // 
        {
            DataTable rst = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "POS_SELAgentDeliveryCharges",
                                       new SqlParameter("@AgentId", AgentId_)
                                     , new SqlParameter("@companyId", CompanyId_)
                                     ).Tables[0];
            return rst;
        }

        public static int ChangeCustomerAddress(int CustomerID_, int CustDetailId_, int salesMasterId_, bool isDefault_, int CompanyID_)
        {
            int rowCount;
            try
            {
                rowCount = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "ChangeCustomerAddressAPI",
                     new SqlParameter("@CustomerID", CustomerID_), new SqlParameter("@CustDetailId", CustDetailId_),
                    new SqlParameter("@salesMasterId", salesMasterId_), new SqlParameter("@isDefault", isDefault_),
                                  new SqlParameter("@CompanyID", CompanyID_));
                return rowCount;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static int InsertItemDiscount(int CompanyID_, int salesMasterId_, int salesDetailId_, double qty_, decimal amount_, int userID_, bool taxInclusive_)
        {
            int rowCount;
            try
            {
                rowCount = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "POS_INSItemDiscountAPI",
                                                     new SqlParameter("@CompanyID", CompanyID_),
                                                     new SqlParameter("@salesMasterId", salesMasterId_),
                                                     new SqlParameter("@SalesDetailsID", salesDetailId_),
                                                     new SqlParameter("@Quantity", qty_),
                                                     new SqlParameter("@DiscountAmount", amount_),
                                                     new SqlParameter("@UserId", userID_),
                                                     new SqlParameter("@IsInclusive", taxInclusive_));
                return rowCount;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static int DistributeItemDiscount(int CompanyID_, int salesMasterId_, bool taxInclusive_)
        {
            int rowCount;
            try
            {
                rowCount = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "POS_DistributeItemDiscountAPI",
                                                     new SqlParameter("@CompanyID", CompanyID_),
                                                     new SqlParameter("@salesMasterId", salesMasterId_),
                                                     new SqlParameter("@IsInclusive", taxInclusive_));
                return rowCount;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        public static int SetFireOrHoldStatus(int SalesDetailsId,int CompanyId,int Case)
        {

            // Case 0 = HOLD
            // Case 1 FIRE
            int rowCount;
            try
            {
                rowCount = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "[SetFireOrHoldStatus]",
                                                     new SqlParameter("@SalesDetailsId", SalesDetailsId),
                                                     new SqlParameter("@CompanyId", CompanyId),
                                                     new SqlParameter("@Case", Case));
                return rowCount;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static int SetGuest(int SalesMasterId, int CustomerId, int CompanyId)
        {

            int rowCount;
            try
            {
                rowCount = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "[SetGuest]",
                                                     new SqlParameter("@SalesMasterId", SalesMasterId),
                                                     new SqlParameter("@CustomerId", CustomerId),
                                                     new SqlParameter("@CompanyId", CompanyId));
                return rowCount;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static int UpdateGuestNo(int SalesMasterId, decimal GuestNo, int CompanyId)
        {

            int rowCount;
            try
            {
                rowCount = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "[UpdateGuestNo]",
                                                     new SqlParameter("@SalesMasterID", SalesMasterId),
                                                     new SqlParameter("@GuestNo", GuestNo),
                                                     new SqlParameter("@CompanyID", CompanyId));
                return rowCount;
            }
            catch (Exception)
            {
                return 0;
            }
        }

    }
}
