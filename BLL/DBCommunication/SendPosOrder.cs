using RakTDAApi.BLL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RakTDAApi.BLL.DBCommunication
{
    public class SendPosOrder : General
    {
        public static void DeleteExistingTax(int SalesMasterID, int CompanyID)
        {
            SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "DeleteExistingPOSOrderTaxService",
                                     new SqlParameter("@CompanyID", CompanyID),
                                     new SqlParameter("@OrderID", SalesMasterID));
        }


        public static int UpdateDiscountedItemsStatus(int SalesMasterID, int SalesDetailsID, int CompanyID)
        {
            int result;
            try
            {
                result = Convert.ToInt32(SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "UpdateDiscountedItemsStatus",
                          new SqlParameter("@SalesmasterID", SalesMasterID),
                          new SqlParameter("@SalesDetailsID", SalesDetailsID),
                          new SqlParameter("@CompanyID", CompanyID)));
                return result;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static int DiscountInsert(int CompanyID, int SalesMasterID, int SalesdetailsID, int DiscountID, int DiscountTypeID,
            double DiscountValue, double DiscountAmount, double Quantity, int UserID, double NetAmount, bool TaxInclusive)
        {
            int result;
            try
            {
                result = Convert.ToInt32(SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "POS_INSOrderDiscount",
                          new SqlParameter("@CompanyID", CompanyID),
                          new SqlParameter("@SalesMasterID", SalesMasterID),
                          new SqlParameter("@SalesDetailsID", SalesdetailsID),
                          new SqlParameter("@DiscountID", DiscountID),
                          new SqlParameter("@DiscountTypeID", DiscountTypeID),
                          new SqlParameter("@DiscountValue", DiscountValue),
                          new SqlParameter("@SalesMasterID", DiscountAmount),
                          new SqlParameter("@SalesDetailsID", Quantity),
                          new SqlParameter("@DiscountID", UserID),
                          new SqlParameter("@DiscountTypeID", NetAmount),
                          new SqlParameter("@IsInclusive", TaxInclusive)));
                return result;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static int ExchangerateViewByCurrencyId(decimal decCurrencyId, int CompanyId)
        {
            try
            {
                
              int  decExchangerateId = Convert.ToInt32(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure, "ExchangerateViewByCurrencyId",
                          new SqlParameter("@currencyId", decCurrencyId),
                          new SqlParameter("@CompanyID", CompanyId)).ToString());
                return decExchangerateId;
            }
            catch(Exception)
            {
                return 0;
            }
           
        }

        public static int UpdateTempSalesMaster(int salesMasterId_, decimal totalAmount_, string invoiceRemark_, string narration_, int CompanyID_)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "tbl_TempSalesMaster_UpdateAPI",
                                   new SqlParameter("@salesMasterId", salesMasterId_), new SqlParameter("@totalAmount", totalAmount_),
                                    new SqlParameter("@invoiceRemark", invoiceRemark_), new SqlParameter("@narration", narration_),
                                   new SqlParameter("@CompanyID", CompanyID_));
                return salesMasterId_;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static int SaveTempSalesMaster(int CounterID_, int voucherNo_, int invoiceNo_, Single OrderNo_, string billNo_, string invoiceStatus_,
            int orderTypeId_, int OpenedBy_, int NoOfCustomers_, decimal grandTotal_, decimal TotalDiscount_, decimal TotalService_, decimal taxAmount_,
            decimal totalAmount_, decimal TaxableAmount_, int VoidID_, int ledgerId_, int creditPeriod_, int employeeId_,
            int salesAccount_, string narration_, int exchangeRateId_, int financialYearId_, string status_, DateTime time_, DateTime OpeningDate_,
            int areaId_, int routeId_, int DeliveryDriverID_, string Statusofdelivery_, int DeliveryAddressID_, DateTime DeliveryOrderDateTime_, int sessionID_, int initId_, string InvoiceRemark_, int GuestNo_,
            int PrintCount_, int CompanyID_, bool Voided_, Single TotalAmountPaid_, Single Tips_, bool TableLock_, int CustomerID_, string PayOrderBy_,
            int PosMachineID_, bool TaxInclusive_, decimal DeliveryCharges_,int AgentId,string AgentReferenceCode)

        {
           
            try
            {
                 int returnSalesMasterID = Convert.ToInt32(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure, "tbl_TempSalesMaster_Insert",
                                  new SqlParameter("@CounterID", CounterID_),
                                  new SqlParameter("@voucherNo", voucherNo_),
                                  new SqlParameter("@invoiceNo", invoiceNo_),
                                  new SqlParameter("@OrderNo", OrderNo_),
                                  new SqlParameter("@billNo", billNo_),
                                  new SqlParameter("@invoiceStatus", invoiceStatus_),
                                  new SqlParameter("@orderTypeId", orderTypeId_),
                                  new SqlParameter("@OpenedBy", OpenedBy_),
                                  new SqlParameter("@NoOfCustomers", NoOfCustomers_),
                                  new SqlParameter("@grandTotal", grandTotal_),
                                  new SqlParameter("@TotalDiscount", TotalDiscount_),
                                  new SqlParameter("@TotalService", TotalService_),
                                  new SqlParameter("@taxAmount", taxAmount_),
                                  new SqlParameter("@totalAmount", totalAmount_),
                                  new SqlParameter("@TaxableAmount", TaxableAmount_),
                                  new SqlParameter("@VoidID", VoidID_),
                                  new SqlParameter("@ledgerId", ledgerId_),
                                  new SqlParameter("@creditPeriod", creditPeriod_),
                                  new SqlParameter("@employeeId", employeeId_),
                                  new SqlParameter("@salesAccount", salesAccount_),
                                  new SqlParameter("@narration", narration_),
                                  new SqlParameter("@exchangeRateId", exchangeRateId_),
                                  new SqlParameter("@financialYearId", financialYearId_),
                                  new SqlParameter("@status", status_),
                                  new SqlParameter("@time", time_),
                                  new SqlParameter("@OpeningDate", OpeningDate_),
                                  new SqlParameter("@areaId", areaId_),
                                  new SqlParameter("@routeId", routeId_),
                                  new SqlParameter("@DeliveryDriverID", DeliveryDriverID_),
                                   new SqlParameter("@statusofdelivery", Statusofdelivery_),
                                  new SqlParameter("@DeliveryAddressID", DeliveryAddressID_),
                                  new SqlParameter("@DeliveryOrderDateTime", DeliveryOrderDateTime_),
                                  new SqlParameter("@sessionID", sessionID_),
                                  new SqlParameter("@initId", initId_),
                                  new SqlParameter("@InvoiceRemark", InvoiceRemark_),
                                  new SqlParameter("@GuestNo", GuestNo_),
                                  new SqlParameter("@PrintCount", PrintCount_),
                                  new SqlParameter("@CompanyID", CompanyID_),
                                  new SqlParameter("@Voided", Voided_),
                                  new SqlParameter("@TotalAmountPaid", TotalAmountPaid_),
                                  new SqlParameter("@Tips", Tips_),
                                  new SqlParameter("@TableLock", TableLock_),
                                  new SqlParameter("@CustomerID", CustomerID_),
                                  new SqlParameter("@PayOrderBy", PayOrderBy_),
                                  new SqlParameter("@PosMachineID", PosMachineID_),
                                  new SqlParameter("@TaxInclusive", TaxInclusive_),
                                  new SqlParameter("@DeliveryCharges", DeliveryCharges_),
                                  new SqlParameter("@AgentId", AgentId),
                                  new SqlParameter("@AgentReferenceCode", AgentReferenceCode)));
                return returnSalesMasterID;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static int SaveTempSalesDetails(int salesMasterId_, int productId_, Single qty_, Single rate_,
                      decimal discount_, int taxId_, decimal taxAmount_, decimal grossAmount_, decimal netAmount_, decimal amount_,
                      int slNo_, DateTime extraDate_, bool printed_, string specialInstructions_, Single NoOfCustomers_,
                      Single OrderNo_, int GuestNo_, string KitchenRemark_, string OrderStatus_, string FireOrHold_,
                      int AffectedItem_, int CompanyID_, decimal SellingPrice_, Single TotalTaxesApplied_, int VoidID_,
                      int employeeId_, int PosID_, int ItemReference_ = 0,  int Transfer = 0)
        {
            int returnSalesDetailID = 0;
            try
            {
                if (VoidID_ == 0)
                {
                     returnSalesDetailID = Convert.ToInt32(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure, "TempSalesDetailsAdd",
                                      new SqlParameter("@salesMasterId", salesMasterId_),
                                      new SqlParameter("@productId", productId_),
                                      new SqlParameter("@qty", qty_),
                                      new SqlParameter("@rate", rate_),
                                      new SqlParameter("@discount", discount_),
                                      new SqlParameter("@taxId", taxId_),
                                      new SqlParameter("@taxAmount", taxAmount_),
                                      new SqlParameter("@grossAmount", grossAmount_),
                                      new SqlParameter("@netAmount", netAmount_),
                                      new SqlParameter("@amount", amount_),
                                      new SqlParameter("@slNo", slNo_),
                                      new SqlParameter("@extraDate", extraDate_),
                                      new SqlParameter("@printed", printed_),
                                      new SqlParameter("@specialInstructions", specialInstructions_),
                                      new SqlParameter("@NoOfCustomers", NoOfCustomers_),
                                      new SqlParameter("@OrderNo", OrderNo_),
                                      new SqlParameter("@GuestNo", GuestNo_),
                                      new SqlParameter("@KitchenRemark", KitchenRemark_),
                                      new SqlParameter("@OrderStatus", OrderStatus_),
                                      new SqlParameter("@FireOrHold", FireOrHold_),
                                      new SqlParameter("@AffectedItem", AffectedItem_),
                                      new SqlParameter("@CompanyID", CompanyID_),
                                      new SqlParameter("@SellingPrice", SellingPrice_),
                                      new SqlParameter("@TotalTaxesApplied", TotalTaxesApplied_),
                                      new SqlParameter("@VoidId", VoidID_),
                                      new SqlParameter("@employeeId", employeeId_),
                                      new SqlParameter("@ItemReference", ItemReference_),
                                      new SqlParameter("@PosID", PosID_),
                                      new SqlParameter("@Transfer", Transfer)));
                }
                else
                {
                    returnSalesDetailID = Convert.ToInt32(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure, "TempSalesDetailsAdd",
                                      new SqlParameter("@salesMasterId", salesMasterId_),
                                      new SqlParameter("@productId", productId_),
                                      new SqlParameter("@qty", qty_),
                                      new SqlParameter("@rate", rate_),
                                      new SqlParameter("@discount", discount_),
                                      new SqlParameter("@taxId", taxId_),
                                      new SqlParameter("@taxAmount", taxAmount_),
                                      new SqlParameter("@grossAmount", grossAmount_),
                                      new SqlParameter("@netAmount", netAmount_),
                                      new SqlParameter("@amount", amount_),
                                      new SqlParameter("@slNo", slNo_),
                                      new SqlParameter("@extraDate", extraDate_),
                                      new SqlParameter("@printed", printed_),
                                      new SqlParameter("@specialInstructions", specialInstructions_),
                                      new SqlParameter("@NoOfCustomers", NoOfCustomers_),
                                      new SqlParameter("@OrderNo", OrderNo_),
                                      new SqlParameter("@GuestNo", GuestNo_),
                                      new SqlParameter("@KitchenRemark", KitchenRemark_),
                                      new SqlParameter("@OrderStatus", OrderStatus_),
                                      new SqlParameter("@FireOrHold", FireOrHold_),
                                      new SqlParameter("@AffectedItem", AffectedItem_),
                                      new SqlParameter("@CompanyID", CompanyID_),
                                      new SqlParameter("@SellingPrice", SellingPrice_),
                                      new SqlParameter("@TotalTaxesApplied", TotalTaxesApplied_),
                                      new SqlParameter("@VoidId", VoidID_),
                                      new SqlParameter("@VoidDate", DateTime.Now),
                                      new SqlParameter("@employeeId", employeeId_),
                                      new SqlParameter("@PosID", PosID_),
                                      new SqlParameter("@ItemReference", ItemReference_),
                                      new SqlParameter("@Transfer", Transfer)));
                }
                return returnSalesDetailID;
            }
            catch (Exception)
            {
                return 0;
            }
        }

       

        public static int VoucherNumberGeneration(int CompanyID)
        {
            int result;
            try
            {
                result = Convert.ToInt32(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure, "TempSalesMasterVoucherMax",
                                        new SqlParameter("@CompanyID", CompanyID)).ToString());
                return result;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static float GetMaxNo(int OrderTypeID, int CompanyID)
        {
            float result;
            try
            {
                result = float.Parse(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure, "GetMaxNo",
                                   new SqlParameter("@OrderTypeID", OrderTypeID),
                                   new SqlParameter("@CompanyID", CompanyID)).ToString());
                return result;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}

