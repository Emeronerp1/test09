using RakTDAApi.BLL.Common;
using RakTDAApi.Models.Payloads;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RakTDAApi.BLL.DBCommunication
{
    public class TableSwitch : General
    {
        public static decimal GetSalesMasterId(decimal OrderNo, int CompanyId)
        {
            decimal GetSalesMasterId = Convert.ToDecimal(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure,
                                        "GetSalesMasterIDforDineIn",
                                     new SqlParameter("@OrderNo", OrderNo)
                                    , new SqlParameter("@CompanyID", CompanyId)));

            return GetSalesMasterId;
        }

        // update for merging
        public static decimal UpdateSwitchtable(decimal FromTable, decimal ToTable, int CompanyId)
        {
            decimal UpdateSwitchtable = Convert.ToDecimal(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure,
                                        "[SPApiUpdateSwitchtable]",
                                      new SqlParameter("@FromTableSalesMasterID", FromTable)
                                    , new SqlParameter("@ToTableSalesMasterID", ToTable)
                                    , new SqlParameter("@CompanyID", CompanyId)));

            return UpdateSwitchtable;
        }

        // getting salesdetails
        public static DataTable UpdateItemTransferTablefromSwitchTable(int CompanyID, decimal OrderNo)
        {
            DataSet ds = BLL.Common.SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "SPApiUpdateItemTransferTablefromTableSwitch",
             new SqlParameter("@CompanyID", CompanyID),
             new SqlParameter("@OrderNo", OrderNo));
            return ds.Tables[0];
        }

        // updating for Switching
        public static decimal UpdateSwitchtableForNew(decimal FromTableSalesMasterId, decimal ToTable, int CompanyId)
        {
            decimal UpdateSwitchtable = Convert.ToDecimal(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure,
                                        "SPApiUpdateSwitchtableForNew",
                                      new SqlParameter("@FromTableSalesMasterID", FromTableSalesMasterId)
                                    , new SqlParameter("@ToTableNo", ToTable)
                                    , new SqlParameter("@CompanyID", CompanyId)));

            return UpdateSwitchtable;
        }

        //logic 
        //checking whter exist fromtable;
        // getting to table
        //if to table is occupied asking for merge or exit
        // for merge calling following fucntions
        // getting both tableno salesmasterid,deleteing existing tax of fromtable,updateswitchtable
        //insert into transfer table

        public static void UpdateItemTransfer(int CompanyId,
                                                int UserId,decimal FromTable,decimal ToTable,bool Taxinclusive,int Type)
        { 
            decimal rst = 0;
            //int Rst = BLL.DBCommunication.CancelTable.CheckForExistingDining(FromTable, CompanyId);
            
            int FromTableSalesMasterId = Convert.ToInt32(GetSalesMasterId(FromTable, CompanyId));
            decimal ToTableSalesMasterId = GetSalesMasterId(ToTable, CompanyId);
            if (Type == Convert.ToInt32(General.TransferType.TableMerger))
            {
                SendPosOrder.DeleteExistingTax(FromTableSalesMasterId, CompanyId);
                rst = UpdateSwitchtable(FromTableSalesMasterId, ToTableSalesMasterId, CompanyId);
            }
            else if(Type == Convert.ToInt32(General.TransferType.TableTransfer))
            {
                rst = UpdateSwitchtableForNew(FromTableSalesMasterId, ToTable, CompanyId);
            }
            BLL.DBCommunication.PosMenu.InsertTaxesAndServices(CompanyId, Convert.ToInt32(General.OrderType.DineIn),Convert.ToInt32(ToTableSalesMasterId), Taxinclusive);
            ItemTransfer(CompanyId, UserId,ToTable,FromTable, Type);
        }



        public static int POSItemTransfer_Insert(ItemTransferInfo InfoObjItemTransfer)
        {
            int rowCount = 0;
            try
            {
                rowCount = SqlHelper.ExecuteNonQuery(dbConStr, CommandType.StoredProcedure, "POSItemTransfer_Insert",
                                  new SqlParameter("@CompanyID", InfoObjItemTransfer.CompanyID),
                                  new SqlParameter("@UserID", InfoObjItemTransfer.UserID),
                                  new SqlParameter("@ItemID", InfoObjItemTransfer.ItemID),
                                  new SqlParameter("@OldTable", InfoObjItemTransfer.OldTable),
                                  new SqlParameter("@NewTable", InfoObjItemTransfer.NewTable),
                                  new SqlParameter("@Quantity", InfoObjItemTransfer.Quantity),
                                  new SqlParameter("@Amount", InfoObjItemTransfer.Amount),
                                  new SqlParameter("@Type", InfoObjItemTransfer.Type));
                return rowCount;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static void ItemTransfer(int CompanyId,int UserId,decimal ToTable,decimal FromTable,int Type)
        {
            ItemTransferInfo InfoObjItemTransfer = new ItemTransferInfo();
            DataTable dt = UpdateItemTransferTablefromSwitchTable(CompanyId, ToTable);
            int DatatableRowCount = dt.Rows.Count;
            for (int i = 0; i < DatatableRowCount; i++)
            {
                InfoObjItemTransfer.ItemID = Convert.ToInt32(dt.Rows[i]["productId"].ToString());
                InfoObjItemTransfer.CompanyID = CompanyId;
                InfoObjItemTransfer.Date = DateTime.Now;
                InfoObjItemTransfer.NewTable = ToTable;
                InfoObjItemTransfer.OldTable = FromTable;
                InfoObjItemTransfer.UserID = UserId;
                decimal value1 = Convert.ToDecimal(dt.Rows[i]["qty"].ToString()) * Convert.ToDecimal(dt.Rows[i]["rate"].ToString());
                InfoObjItemTransfer.Amount = (float)value1;
                InfoObjItemTransfer.Type = Type;
                // Convert.ToInt32(General.TransferType.TableMerger);
                InfoObjItemTransfer.Quantity = Convert.ToDouble((dt.Rows[i]["qty"].ToString()));
                int ExecuteResult = POSItemTransfer_Insert(InfoObjItemTransfer);

            }
        }


      

    }
}
