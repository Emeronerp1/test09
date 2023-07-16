using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace RakHolidayHomes
{
    public class VPCRequest
    {
        Uri _address;
        SortedList<String, String> _requestFields = new SortedList<String, String>(new VPCStringComparer());
        String _rawResponse;
        SortedList<String, String> _responseFields = new SortedList<String, String>(new VPCStringComparer());
        String _secureSecret;
        String _proxyhost;
        String _proxyuser;
        String _proxypassword;
        String _proxydomain;
        String _merchantID;
        String _returnURL;
        String _accessCode;
        String _vpcCommand;
        String _vpcVersion;



        private readonly IConfiguration _config;

        public VPCRequest(IConfiguration config)
        {
            _config = config;
            RakTDAApi.BLL.Common.General.dbConStr = _config.GetConnectionString("DBConnection").ToString();
        }
        private readonly IWebHostEnvironment _env;

        public VPCRequest(IWebHostEnvironment env)
        {
            _env = env;
        }


        public VPCRequest()
        {
            _address = new Uri(_config["MySettings:PaymentServerURL"]);
            _merchantID = _config["MySettings:vpc_Merchant"];
            _accessCode = _config["MySettings:vpc_AccessCode"];
            _vpcCommand = _config["MySettings:vpc_Command"];
            _vpcVersion = _config["MySettings:vpc_Version"];
            _returnURL = _config["MySettings:vpc_ReturnURL"];
            _secureSecret = _config["MySettings:SecureSecret"];
        }


        public String MerchantID { get { return _merchantID; } }

        public String AccessCode { get { return _accessCode; } }

        public String Command { get { return _vpcCommand; } }

        public String Version { get { return _vpcVersion; } }

        public String SecureSecret { get { return _config["MySettings:SecureSecret"]; } }

        public void SetProxyHost(String URI)
        {
            _proxyhost = URI;
        }

        public void SetProxyUser(String Username)
        {
            _proxyuser = Username;
        }

        public void SetProxyPassword(String Password)
        {
            _proxypassword = Password;
        }

        public void SetProxyDomain(String Domain)
        {
            _proxydomain = Domain;
        }


        public void SetSecureSecret(String secret)
        {
            _secureSecret = secret;
        }

        public void AddDigitalOrderField(String key, String value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                _requestFields.Add(key, value);
            }
        }

        public String GetResultField(String key, String defValue)
        {
            String value;
            if (_responseFields.TryGetValue(key, out value))
            {
                return value;
            }
            else
            {
                return defValue;
            }
        }

        public String GetResultField(String key)
        {
            return GetResultField(key, "");
        }

        /// <summary>
        /// Returns a return URL with host and port information based on the page's current execution environment.
        /// This is important when running this example code inside Visual Studio
        /// </summary>
        /// <param name="host">The hostname where the example code is running</param>
        /// <param name="port">The port being currently used to access the example code</param>
        /// <returns>The value to be passed in vpc_ReturnURL</returns>
        public string FormatReturnURL(string scheme, string host, int port, string appPath)
        {
            StringBuilder returnURL = new StringBuilder();
            returnURL.Append(scheme);
            returnURL.Append("://");
            returnURL.Append(host);
            returnURL.Append(":");
            returnURL.Append(port);
            returnURL.Append(appPath);
            returnURL.Append("/");
            returnURL.Append(_returnURL);
            return returnURL.ToString();
        }

        public String FormatDate(String month, String year)
        {
            // We have the expiry month and year, but the Payment Client requires
            // a 4 digit expiry year/month (YYMM). We construct this from the
            // information we already have.

            if (year != null && year.Length > 2)
            {
                year = year.Substring(year.Length - 2);
            }
            else if (year != null && year != "" && year.Length < 2)
            {
                year = year.PadLeft(2, '0');
            }

            if (month != null && month.Length > 2)
            {
                month = month.Substring(month.Length - 2);
            }
            else if (month != null && month != "" && month.Length < 2)
            {
                month = month.PadLeft(2, '0');
            }

            return year + month;
        }

        private String GetRequestRaw()
        {
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in _requestFields)
            {
                if (!String.IsNullOrEmpty(kvp.Value))
                {
                    data.Append(kvp.Key + "=" + HttpUtility.UrlEncode(kvp.Value) + "&");
                }
            }
            //remove trailing & from string
            if (data.Length > 0)
                data.Remove(data.Length - 1, 1);
            return data.ToString();
        }

        public String GetResponseRaw()
        {
            if (!String.IsNullOrEmpty(_rawResponse))
            {
                return _rawResponse;
            }
            else
            {
                return "";
            }
        }

        //_____________________________________________________________________________________________________
        // Three-Party order transaction processing with hidden fields when sending CC info

        public String GetHiddenRequestForm()
        {
            StringBuilder data = new StringBuilder();

            // Construct the form to send the request to the server
            data.Append("\n<form id=\"PaymentRedirectForm\" name=\"PaymentRedirectForm\" method=\"post\" action=\"" + _address + "\">\n");

            foreach (KeyValuePair<string, string> kvp in _requestFields)
            {
                if (!String.IsNullOrEmpty(kvp.Value))
                {
                    data.Append("<input type=\"hidden\" name=\"" + kvp.Key + "\" value=\"" + kvp.Value + "\" />\n");
                }
            }
            data.Append("<input type=\"hidden\" name=\"vpc_SecureHash\" value=\"" + CreateSHA256Signature(true) + "\" />\n");
            data.Append("<input type=\"hidden\" name=\"vpc_SecureHashType\" value=\"SHA256\" />\n");
            data.Append("</form>\n");
            return data.ToString();
        }

        //_____________________________________________________________________________________________________
        // Two-Party order transaction processing

        public void SendRequest()
        {
            // Setup proxy if needed
            if (!String.IsNullOrEmpty(_proxyhost))
            {
                WebProxy proxy = new WebProxy(_proxyhost, true);
                if (!String.IsNullOrEmpty(_proxyuser))
                {
                    if (String.IsNullOrEmpty(_proxypassword))
                    {
                        _proxypassword = "";
                    }

                    if (String.IsNullOrEmpty(_proxydomain))
                    {
                        proxy.Credentials = new NetworkCredential(_proxyuser, _proxypassword);
                    }
                    else
                    {
                        proxy.Credentials = new NetworkCredential(_proxyuser, _proxypassword, _proxydomain);
                    }
                }
                WebRequest.DefaultWebProxy = proxy;

            }

            // Create the web request  
            HttpWebRequest request = WebRequest.Create(_address) as HttpWebRequest;

            // Set type to POST  
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            // If vpc_SecureSecret defined, use secure hashing as best practice
            if (!String.IsNullOrEmpty(_secureSecret))
            {
                _requestFields.Add("vpc_SecureHash", CreateSHA256Signature(true));
                _requestFields.Add("vpc_SecureHashType", "SHA256");
            }

            // Create a byte array of the data we want to send  
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(GetRequestRaw());

            // Set the content length in the request headers  
            request.ContentLength = byteData.Length;

            // Write data  
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }

            // Get response  
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                // Get the response stream  
                StreamReader reader = new StreamReader(response.GetResponseStream());

                // Console application output
                _rawResponse = reader.ReadToEnd();
                String[] responses = _rawResponse.Split('&');
                foreach (String responseField in responses)
                {
                    String[] field = responseField.Split('=');
                    _responseFields.Add(field[0], HttpUtility.UrlDecode(field[1]));
                }
            }

            // If vpc_SecureSecret defined, there needs to be a hash returned since we hashed
            // on VPC submission
            if (!String.IsNullOrEmpty(_secureSecret))
            {
                try
                {
                    // Retrieve and remove hash info from results
                    string secureHash = _responseFields["vpc_SecureHash"];
                    string secureHashType = _responseFields["vpc_SecureHashType"];
                    _responseFields.Remove("vpc_SecureHash");
                    _responseFields.Remove("vpc_SecureHashType");

                    // Check if hash returned correctly
                    if (String.IsNullOrEmpty(secureHash))
                    {
                        throw new Exception("Secure Hash not returned from VPC");
                    }
                    else if (!secureHash.Equals(CreateSHA256Signature(false)))
                    {
                        throw new Exception("Secure Hash returned from VPC does not match");
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        public string GetTxnResponseCode()
        {
            return GetResultField("vpc_TxnResponseCode");
        }

        //_____________________________________________________________________________________________________
        // Three-Party order transaction processing

        public String Create3PartyQueryString()
        {
            StringBuilder url = new StringBuilder();
            //Payment Server URL
            url.Append(_address);
            url.Append("?");
            //Create URL Encoded request string from request fields 
            url.Append(GetRequestRaw());
            //Hash the request fields
            url.Append("&vpc_SecureHash=");
            url.Append(CreateSHA256Signature(true));
            //Designate the hash type
            url.Append("&vpc_SecureHashType=SHA256");
            return url.ToString();
        }

        public void Process3PartyResponse(System.Collections.Specialized.NameValueCollection nameValueCollection)
        {
            foreach (string item in nameValueCollection)
            {
                if (!item.Equals("vpc_SecureHash") && !item.Equals("vpc_SecureHashType"))
                {
                    _responseFields.Add(item, nameValueCollection[item]);
                }

            }

            if (!nameValueCollection["vpc_TxnResponseCode"].Equals("0") && !String.IsNullOrEmpty(nameValueCollection["vpc_Message"]))
            {
                if (!String.IsNullOrEmpty(nameValueCollection["vpc_SecureHash"]))
                {
                    if (!CreateSHA256Signature(false).Equals(nameValueCollection["vpc_SecureHash"]))
                    {
                        throw new Exception("Secure Hash does not match");
                    }
                    return;
                }
                return;
            }


            if (String.IsNullOrEmpty(nameValueCollection["vpc_SecureHash"]))
            {
                throw new Exception("No Secure Hash included in response");
            }
            if (!CreateSHA256Signature(false).Equals(nameValueCollection["vpc_SecureHash"]))
            {
                throw new Exception("Secure Hash does not match");
            }
        }



        //_____________________________________________________________________________________________________

        private class VPCStringComparer : IComparer<String>
        {
            /*
             <summary>Customised Compare Class</summary>
             <remarks>
             <para>
             The Virtual Payment Client need to use an Ordinal comparison to Sort on 
             the field names to create the SHA256 Signature for validation of the message. 
             This class provides a Compare method that is used to allow the sorted list 
             to be ordered using an Ordinal comparison.
             </para>
             </remarks>
             */

            public int Compare(String a, String b)
            {
                /*
                 <summary>Compare method using Ordinal comparison</summary>
                 <param name="a">The first string in the comparison.</param>
                 <param name="b">The second string in the comparison.</param>
                 <returns>An int containing the result of the comparison.</returns>
                 */

                // Return if we are comparing the same object or one of the 
                // objects is null, since we don't need to go any further.
                if (a == b) return 0;
                if (a == null) return -1;
                if (b == null) return 1;

                // Ensure we have string to compare
                string sa = a as string;
                string sb = b as string;

                // Get the CompareInfo object to use for comparing
                System.Globalization.CompareInfo myComparer = System.Globalization.CompareInfo.GetCompareInfo("en-US");
                if (sa != null && sb != null)
                {
                    // Compare using an Ordinal Comparison.
                    return myComparer.Compare(sa, sb, System.Globalization.CompareOptions.Ordinal);
                }
                throw new ArgumentException("a and b should be strings.");
            }
        }

        //______________________________________________________________________________
        // SHA256 Hash Code

        private string CreateSHA256Signature(bool useRequest)
        {
            // Hex Decode the Secure Secret for use in using the HMACSHA256 hasher
            // hex decoding eliminates this source of error as it is independent of the character encoding
            // hex decoding is precise in converting to a byte array and is the preferred form for representing binary values as hex strings. 
            byte[] convertedHash = new byte[_secureSecret.Length / 2];
            for (int i = 0; i < _secureSecret.Length / 2; i++)
            {
                convertedHash[i] = (byte)Int32.Parse(_secureSecret.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }

            // Build string from collection in preperation to be hashed
            StringBuilder sb = new StringBuilder();
            SortedList<String, String> list = (useRequest ? _requestFields : _responseFields);
            foreach (KeyValuePair<string, string> kvp in list)
            {
                if (kvp.Key.StartsWith("vpc_") || kvp.Key.StartsWith("user_"))
                    sb.Append(kvp.Key + "=" + kvp.Value + "&");
            }
            // remove trailing & from string
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);

            // Create secureHash on string
            string hexHash = "";
            using (HMACSHA256 hasher = new HMACSHA256(convertedHash))
            {
                byte[] hashValue = hasher.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
                foreach (byte b in hashValue)
                {
                    hexHash += b.ToString("X2");
                }
            }
            return hexHash;
        }
    }
}