using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
using System.Data;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Configuration;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;

namespace RAKHolidayHomesBL
{
    public static class clsUtilities
    {
        public static string RemoveHtml(string Htmltext)
        {
            Regex regex = new Regex("\\<[^\\>]*\\>");
            return regex.Replace(Htmltext, String.Empty);
        }
        public static bool IsNumeric(this string s)
        {
            float output;
            return float.TryParse(s, out output);
        }
        public static string RemoveSpecialChars(string str)
        {

            string[] chars = new string[] { ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "_", "(", ")", ":", "|", "[", "]", " " };


            for (int i = 0; i < chars.Length; i++)
            {
                if (str.Contains(chars[i]))
                {
                    switch (chars[i])
                    {
                        case " ":                   // -spc-
                            str = str.Replace(chars[i], "-");
                            break;
                        case ",":                   // -spc-
                            str = str.Replace(chars[i], "-");
                            break;
                        case ".":                   // -spd-
                            str = str.Replace(chars[i], "-");
                            break;
                        case "/":                   // -spsl-  
                            str = str.Replace(chars[i], "-");
                            break;
                        case "!":                   // -spe-
                            str = str.Replace(chars[i], "-");
                            break;
                        case "@":                   // -spa-
                            str = str.Replace(chars[i], "-");
                            break;
                        case "#":                   // -sph-
                            str = str.Replace(chars[i], "-");
                            break;
                        case "$":                   // -spdo-
                            str = str.Replace(chars[i], "-");
                            break;
                        case "%":                   // -spp-
                            str = str.Replace(chars[i], "-");
                            break;
                        case "^":                   // -spn-
                            str = str.Replace(chars[i], "-");
                            break;
                        case "&":                   // -span-
                            str = str.Replace(chars[i], "-");
                            break;
                        case "*":                   // -spst-
                            str = str.Replace(chars[i], "-");
                            break;
                        case "'":                   // -spct-
                            str = str.Replace(chars[i], "-");
                            break;
                        case "\"":                  // -spfs-
                            str = str.Replace(chars[i], "-");
                            break;
                        case ";":                   // -spsm-
                            str = str.Replace(chars[i], "-");
                            break;
                        case "_":                   // -spdc-
                            str = str.Replace(chars[i], "-");
                            break;
                        case "(":                   // -spbo-
                            str = str.Replace(chars[i], "-");
                            break;
                        case ")":                   //  -spbc-
                            str = str.Replace(chars[i], "-");
                            break;
                        case ":":                   // -spcn-
                            str = str.Replace(chars[i], "-");
                            break;
                        case "|":                   //  -spor-
                            str = str.Replace(chars[i], "-");
                            break;
                        case "[":                   // -spso-
                            str = str.Replace(chars[i], "-");
                            break;
                        case "]":                   // -spsc-
                            str = str.Replace(chars[i], "-");
                            break;
                    }
                }
            }
            return str;
        }
        public static string RemoveSpecialCharacter(string contents)
        {
            return Regex.Replace(contents, "[^a-zA-Z0-9_.]+", "-", RegexOptions.Compiled);
        }
        public static string GenerateAlphanumeric(int length = 8)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }
        public static string ShowMessage(string str)
        {
            return System.Configuration.ConfigurationManager.AppSettings[str].ToString();
        }
        //  This function is used to Clear the controls within a Parent Control like Panel/Form
        //public static void clearControls(Control parent)
        //{
        //    foreach (Control c in parent.Controls)
        //    {
        //        if ((c.Controls.Count > 0))
        //        {
        //            clearControls(c);
        //        }
        //        else
        //        {
        //            if (c is TextBox)
        //            {
        //                ((TextBox)c).Text = "";
        //                ((TextBox)c).Enabled = true;
        //            }
        //            else if (c is DropDownList)
        //            {
        //                ((DropDownList)c).SelectedIndex = 0;
        //            }
        //            else if (c is CheckBox)
        //            {
        //                ((CheckBox)c).Checked = false;
        //            }
        //        }
        //    }
        //}
        //public static void SearchInGridView(GridView gv, DropDownList ddl, string keyword)
        //{
        //    System.Data.DataTable dt = new System.Data.DataTable();

        //    for (int colIndex = 0; colIndex <= gv.HeaderRow.Cells.Count - 3; colIndex++)
        //    {
        //        System.Data.DataColumn dc = new System.Data.DataColumn(gv.HeaderRow.Cells[colIndex].Text.Trim(), typeof(String));
        //        dt.Columns.Add(dc);
        //    }

        //    foreach (GridViewRow gvr in gv.Rows)
        //    {
        //        if (gvr.Cells[Convert.ToInt32(ddl.SelectedValue)].Text.Trim().ToUpper().Contains(keyword.Trim().ToUpper()))
        //        {
        //            System.Data.DataRow dr = dt.NewRow();
        //            for (int colIndex = 0; colIndex <= gv.HeaderRow.Cells.Count - 3; colIndex++)
        //                dr[colIndex] = gvr.Cells[colIndex].Text.Trim();

        //            dt.Rows.Add(dr);
        //        }
        //    }
        //    gv.DataSource = dt;
        //    gv.DataBind();
        //}


        //public static void SearchInGridView(GridView gv, DropDownList ddl, string keyword, string[] cmdArguments)
        //{
        //    System.Data.DataTable dt = new System.Data.DataTable();

        //    for (int colIndex = 0; colIndex <= gv.HeaderRow.Cells.Count - 3; colIndex++)
        //    {
        //        System.Data.DataColumn dc = new System.Data.DataColumn(gv.HeaderRow.Cells[colIndex].Text.Trim(), typeof(String));
        //        dt.Columns.Add(dc);
        //    }

        //    // Creating colums for command argument values( if exists)
        //    if (cmdArguments.Length > 0)
        //    {
        //        for (int cnt = 0; cnt < cmdArguments.Length; cnt++)
        //        {
        //            System.Data.DataColumn dc = new System.Data.DataColumn(cmdArguments[cnt].ToString(), typeof(String));
        //            dt.Columns.Add(dc);
        //        }

        //    }

        //    foreach (GridViewRow gvr in gv.Rows)
        //    {
        //        if (gvr.Cells[Convert.ToInt32(ddl.SelectedValue)].Text.Trim().ToUpper().Contains(keyword.Trim().ToUpper()))
        //        {
        //            System.Data.DataRow dr = dt.NewRow();
        //            for (int colIndex = 0; colIndex <= gv.HeaderRow.Cells.Count - 3; colIndex++)
        //                dr[colIndex] = gvr.Cells[colIndex].Text.Trim();


        //            // Creating data for command argument values( if exists)
        //            if (cmdArguments.Length > 0)
        //            {
        //                for (int cnt = 0; cnt < cmdArguments.Length; cnt++)
        //                {
        //                    string[] args = (((ImageButton)gvr.FindControl("imgEdit")).CommandArgument.Split('$'));
        //                    dr[(gv.HeaderRow.Cells.Count - 2 + cnt)] = args[cnt + 1];
        //                }
        //            }
        //            dt.Rows.Add(dr);
        //        }
        //    }

        //    gv.DataSource = dt;
        //    gv.DataBind();
        //}

        //public static Boolean CheckDuplicateEntry(GridView gv, string fielName, int columnIndex)
        //{
        //    foreach (GridViewRow gvr in gv.Rows)
        //    {
        //        if (gvr.Cells[columnIndex].Text.Trim().ToUpper() == fielName.Trim().ToUpper())
        //        {
        //            //  clsGeneralBLL.CodeFlag = false;
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //public static string GetRootPath()
        //{
        //    string[] appPath = HttpContext.Current.Request.PhysicalApplicationPath.Split('\\');
        //    string path = "";
        //    for (int cnt = 0; cnt < appPath.Length - 1; cnt++)
        //        path += appPath[cnt].ToString() + "\\";
        //    return path;
        //}

        //public static void comboFill(DropDownList ddl, DataTable dt)
        //{

        //    ddl.Items.Clear();
        //    ddl.Items.Add(new System.Web.UI.WebControls.ListItem("--- Select ---", "0"));

        //    foreach (DataRow row in dt.Rows)
        //    {
        //        ddl.Items.Add(new System.Web.UI.WebControls.ListItem(row[1].ToString().Trim(), row[0].ToString().Trim()));
        //    }


        //}

        //public static void comboFill(DropDownList ddl, DataTable dt, String text, String value)
        //{

        //    ddl.Items.Clear();
        //    ddl.Items.Add(new System.Web.UI.WebControls.ListItem("--- Select ---", "0"));

        //    foreach (DataRow row in dt.Rows)
        //    {
        //        ddl.Items.Add(new System.Web.UI.WebControls.ListItem(row[text].ToString().Trim(), row[value].ToString().Trim()));
        //    }


        //}


        public static String EncryptPassword(String Password)
        {
            string keystring = "!@#$%^&*";
            Byte[] key = Encoding.UTF8.GetBytes(keystring);
            Byte[] pwdarray = Encoding.UTF8.GetBytes(Password + keystring);

            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            Byte[] pwdhasharray = sha1.ComputeHash(pwdarray);
            string hashstring = Convert.ToBase64String(pwdhasharray);

            return hashstring;


        }

        public static string BitlyEncrypt2(string pUrl)
        {
            string uri = "http://api.bit.ly/shorten?version=2.0.1&format=txt" +
                "&longUrl=" + HttpUtility.UrlEncode(pUrl) +
                "&login=" + HttpUtility.UrlEncode(ConfigurationSettings.AppSettings["bituser"].ToString()) +
                "&apiKey=" + HttpUtility.UrlEncode(ConfigurationSettings.AppSettings["bitkey"].ToString());

            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ServicePoint.Expect100Continue = false;
            request.ContentLength = 0;

            return (new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd());
        }



        //public static void SetGridViewRows(GridView gv)
        //{
        //    int i = 0;
        //    foreach (GridViewRow r in gv.Rows)
        //    {

        //        // r.Attributes.Add("onmouseover", "this.style.background='#f3f3f3'");
        //        if (gv.SelectedIndex != r.RowIndex)
        //        {
        //            // r.Attributes.Add("onmouseover", "this.className='grid-row-hover'");
        //            if (i % 2 == 0)
        //            {
        //                r.CssClass = "grid-row-1";
        //                //  r.Attributes.Add("onmouseout", "this.className='grid-row-1'");
        //            }
        //            else
        //            {
        //                r.CssClass = "grid-row-2";
        //                //  r.Attributes.Add("onmouseout", "this.className='grid-row-2'");

        //            }
        //        }
        //        else
        //        {
        //            r.CssClass = "grid-row-select";
        //            // r.Attributes.Remove("onmouseover");
        //            // r.Attributes.Remove("onmouseout");

        //        }
        //        r.Attributes.Add("onclick", "sp('selrow','" + i + "');");
        //        i++;
        //    }
        //}


        //static public void BuildNoRecords(GridView gridView, DataSet ds)
        //{
        //    try
        //    {
        //        if (ds.Tables[0].Rows.Count == 0)
        //        {
        //            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
        //            gridView.DataSource = ds;
        //            gridView.DataBind();
        //            int columnCount = gridView.Rows[0].Cells.Count;
        //            gridView.Rows[0].Cells.Clear();
        //            gridView.Rows[0].Cells.Add(new TableCell());
        //            gridView.Rows[0].Cells[0].ColumnSpan = columnCount;
        //            gridView.Rows[0].Cells[0].Text = "No Records Found.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        //static public void BuildNoRecords(GridView gridView)
        //{
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        DataTable dt = new DataTable();
        //        ds.Tables.Add(dt);

        //        if (ds.Tables[0].Rows.Count == 0)
        //        {
        //            int columnCount = gridView.Columns.Count;
        //            for (int i = 0; i < columnCount; i++)
        //            {
        //                dt.Columns.Add(gridView.Columns[i].HeaderText);
        //            }
        //            DataRow r = dt.NewRow();
        //            r[0] = "hi";
        //            ds.Tables[0].Rows.Add(r);
        //            gridView.DataSource = ds;
        //            //   gridView.DataBind();
        //            int j = gridView.Rows[0].Controls.Count;
        //            gridView.Rows[0].Cells.Clear();
        //            gridView.Rows[0].Cells.Add(new TableCell());
        //            gridView.Rows[0].Cells[0].ColumnSpan = columnCount;
        //            gridView.Rows[0].Cells[0].Text = "No Records Found.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        static public string CreateJsonParameters(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();

            //Exception Handling
            if (dt != null && dt.Rows.Count > 0)
            {
                // jsonString.Append("{ ");
                // jsonString.Append("");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // jsonString.Append("");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            jsonString.Append(dt.Rows[i][j].ToString());
                        }
                        else if (j == dt.Columns.Count - 1)
                        {
                            jsonString.Append(dt.Rows[i][j].ToString());
                        }
                    }

                    /*end Of String*/
                    if (i == dt.Rows.Count - 1)
                    {
                        //   jsonString.Append("");
                    }
                    else
                    {
                        jsonString.Append(",");
                    }
                }

                // jsonString.Append("");
                return jsonString.ToString();
            }
            else
            {
                return null;
            }
        }


        //public static void ExportToExcel(Control cl, string heading)
        //{
        //    string attachment = "attachment; filename=" + heading + ".xls";
        //    HttpContext.Current.Response.ClearContent();
        //    HttpContext.Current.Response.AddHeader("content-disposition", attachment);
        //    HttpContext.Current.Response.ContentType = "application/ms-excel";
        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter htw = new HtmlTextWriter(sw);
        //    cl.RenderControl(htw);
        //    HttpContext.Current.Response.Write("<center><b><font size=2>" + DateTime.Now.ToString("dd-MMM-yyyy") + "</font></b></center>");
        //    HttpContext.Current.Response.Write("<center><b> <font size=4>" + heading + "</font></b></center>");
        //    HttpContext.Current.Response.Write(sw.ToString());
        //    HttpContext.Current.Response.End();
        //}
        //public static void ExportToPdf(Control cl, string heading)
        //{
        //    string attachment = "attachment; filename=" + heading + ".pdf";
        //    HttpContext.Current.Response.ContentType = "application/pdf";
        //    HttpContext.Current.Response.AddHeader("content-disposition", attachment);
        //    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter hw = new HtmlTextWriter(sw);
        //    cl.RenderControl(hw);
        //    StringReader sr = new StringReader(sw.ToString());
        //    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 40f, 0f);
        //    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //    PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
        //    pdfDoc.Open();
        //    htmlparser.Parse(sr);
        //    pdfDoc.Close();
        //    HttpContext.Current.Response.Write("<center><b><font size=2>" + DateTime.Now.ToString("dd-MMM-yyyy") + "</font></b></center>");
        //    HttpContext.Current.Response.Write("<center><b> <font size=4>" + heading + "</font></b></center>");
        //    HttpContext.Current.Response.Write(pdfDoc);
        //    HttpContext.Current.Response.End();
        //}
    }
}
