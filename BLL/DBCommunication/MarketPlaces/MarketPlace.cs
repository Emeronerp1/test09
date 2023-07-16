using RakTDAApi.BLL.Common;
using RakTDAApi.Models.Payloads;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RakTDAApi.BLL.DBCommunication.MarketPlaces
{
    public class MarketPlace : General
    {
        public static string GetOrderStatus(string AgentReferenceCode, int CompanyId)
        {
            string status = "";
            status = Convert.ToString(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure,
                                        "GetOrderStatus",
                                     new SqlParameter("@AgentReferenceCode", AgentReferenceCode),
                                     new SqlParameter("@CompanyId", CompanyId)));
            return status;

        }

        public static DataTable GetCustomerDetails(string SearchKey)
        {
            DataTable rst = new DataTable();
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure,
                                        "[GetCustomerDetails]",
                                     new SqlParameter("@SearchKey", SearchKey));
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {

                return rst;
            }

        }
        public static Customers AssignCustomers(string SearchKey, String Name)
        {
            DataTable Cust = GetCustomerDetails(SearchKey);
            Customers customer = new Customers();
            int rst = 0;
            if (Cust.Rows.Count > 0)
            {

                customer.AccountLedgerId = Convert.ToInt32(Cust.Rows[0]["AccountLedgerId"].ToString());
                customer.CustomerID = Convert.ToInt32(Cust.Rows[0]["CustomerID"].ToString());
                customer.FirstName = Cust.Rows[0]["FirstName"].ToString();
                customer.mobile = Cust.Rows[0]["mobile"].ToString();

            }
            else
            {
                customer.FirstName = Name;
                customer.mobile = SearchKey;
                customer.CustomerID = CustomerInfo.SaveCustomer(customer.CompanyID, customer.FirstName, customer.FamilyName,
                         customer.mobile, customer.OfficePhone, customer.HomePhone, customer.OtherPhone, customer.email,
                         customer.creditPeriod, customer.creditLimit, customer.Narration, customer.CompanyName, customer.Title);
            }
            return customer;

        }

        public static DataTable GetCustomerAddress(int CustomerId, int City, string Building, string Floor)
        {
            DataSet dataSet = new DataSet();
            DataTable rst = new DataTable();
            dataSet = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure, "[GetCustomerAddress]",
                                     new SqlParameter("@CustomerId", CustomerId),
                                     new SqlParameter("@City", City),
                                     new SqlParameter("@Building", Building),
                                     new SqlParameter("@Floor", Floor));
            if (dataSet.Tables.Count > 0)
            {
                return dataSet.Tables[0];
            }
            else
            {
                return rst;
            }

        }
        public static int getCityId(string City, int CompanyId)
        {
            int rst = 0;
            rst = Convert.ToInt32(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure,
                                        "[getCityId]",
                                     new SqlParameter("@City", City), new SqlParameter("@CompanyId", CompanyId)));
            return rst;
        }
        public static int GetAreaId(string Area, int CompanyId)
        {
            int rst = 0;
            rst = Convert.ToInt32(SqlHelper.ExecuteScalar(dbConStr, CommandType.StoredProcedure,
                                        "[getAreaId]",
                                     new SqlParameter("@Area", Area), new SqlParameter("@CompanyId", CompanyId)));
            return rst;
        }
        public static CustomerDetails AssignCustomerAddress(int CustomerId, string City, string Building, string Floor, int CompanyId,string ApartmentNo,string Area)
        {
            int CityId = getCityId(City, CompanyId);
            int AreaId = GetAreaId(Area, CompanyId);
            DataTable Cust = GetCustomerAddress(CustomerId, CityId, Building, Floor);

            CustomerDetails customerdetails = new CustomerDetails();
           
            if (Cust.Rows.Count > 0)
            {
                customerdetails.CustDetailID = Convert.ToInt32(Cust.Rows[0]["CustDetailId"].ToString());
                customerdetails.CustomerID = Convert.ToInt32(Cust.Rows[0]["CustomerID"].ToString());
                //c
                customerdetails.Floor = Cust.Rows[0]["Floor"].ToString();
                customerdetails.AddressID = Convert.ToInt16(Cust.Rows[0]["AddressID"].ToString());
                customerdetails.isDefault = true;
            }
            else
            {
                customerdetails.CustomerID = CustomerId;
                customerdetails.CityID = CityId;
                customerdetails.Building = Building;
                customerdetails.Floor = Floor;
                customerdetails.AreaID = AreaId;
                customerdetails.Apartment = ApartmentNo;
                customerdetails.isDefault = true;
                customerdetails.CustDetailID = CustomerInfo.SaveCustomerDetails(customerdetails.CustomerID,
                                    customerdetails.CityID, customerdetails.Street, customerdetails.Floor, customerdetails.Apartment,

                                    customerdetails.Building, customerdetails.Near,  customerdetails.AreaID, customerdetails.Zip,
                                     customerdetails.isDefault, customerdetails.AddressID);
            }
            return customerdetails;

        }

        public static DataTable GetPaymentDetails(string Mode, string Type)
        {
            DataTable dt = new DataTable();

            // Mode = CashOnDelivery or CardOnDelivery or OnlinePayment;
            if (Mode == "COD")
            {
                return dt;
            }
            else
            {
                dt = SqlHelper.ExecuteDataset(dbConStr, CommandType.StoredProcedure,
                                        "[GetPayTypeDetails]",
                                     new SqlParameter("@PaymentType", Type)
                                     ).Tables[0];
            }

            return dt;

        }

    }
}
