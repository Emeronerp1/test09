
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RakTDAApi.BLL.Common
{
    public class General
    {
        public static string dbConStr = string.Empty;

        private readonly IHostEnvironment _env;

        public General(IHostEnvironment env)
        {
            _env = env;
        }

     
        public static string MapPath(string path)
        {
            return Path.Combine(
                (string)AppDomain.CurrentDomain.GetData("ContentRootPath"),
                path);
        }
       
        public static string Dt2JSON(DataTable dtTemp)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(dtTemp);
            return JSONString;

        }


        public static string DataTableToJsonWithStringBuilder(DataTable table)
		{
			var jsonString = new StringBuilder();
			if (table.Rows.Count > 0)
			{
				jsonString.Append("[");
				for (int i = 0; i < table.Rows.Count; i++)
				{
					jsonString.Append("{");
					for (int j = 0; j < table.Columns.Count; j++)
					{
						if (j < table.Columns.Count - 1)
						{
							jsonString.Append("\"" + table.Columns[j].ColumnName.ToString()
											  + "\":" + "\""
											  + table.Rows[i][j].ToString() + "\",");
						}
						else if (j == table.Columns.Count - 1)
						{
							jsonString.Append("\"" + table.Columns[j].ColumnName.ToString()
											  + "\":" + "\""
											  + table.Rows[i][j].ToString() + "\"");
						}
					}
					if (i == table.Rows.Count - 1)
					{
						jsonString.Append("}");
					}
					else
					{
						jsonString.Append("},");
					}
				}
				jsonString.Append("]");
			}
			return jsonString.ToString();
		}
        static public string DataTableToJSON(DataTable dataTable, bool readableformat = true)
        {
            string JSONString = "[";
            string JSONRow;
            string colVal;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (JSONString != "[") { JSONString += ","; }
                JSONRow = "";
                if (readableformat) { JSONRow += "\r\n"; }
                JSONRow += "{";

                foreach (DataColumn col in dataTable.Columns)
                {
                    colVal = dataRow[col].ToString();
                    colVal = colVal.Replace("\"", "\\\"");
                    colVal = colVal.Replace("'", "\\\'");
                    if (JSONRow != "{" && JSONRow != "\r\n{")
                    {

                        JSONRow += ",";

                    }
                    JSONRow += "\"" + col.ColumnName + "\":\"" + colVal + "\"";

                }
                JSONRow += "}";
                JSONString += JSONRow;
            }
            JSONString += "\r\n]";
            return JSONString;
        }

    }
}
