using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;

namespace sharp_ly
{
    public class sharply
    {
        public enum OutputFormatType
        {
            json,
            xml
        }

        /// <summary>
        /// Set your bit.ly username (the one you login with)
        /// </summary>
        private string _Username;
        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }

        /// <summary>
        /// Set your bit.ly API key. It can be found here: http://bit.ly/account/
        /// </summary>
        private string _APIKey;
        public string apiKey
        {
            get { return _APIKey; }
            set { _APIKey = value; }
        }

        /// <summary>
        /// The Url of the API. Usually http://api.bit.ly
        /// </summary>
        private string _baseAPIUrl = "http://api.bit.ly";
        public string baseAPIUrl
        {
            get { return _baseAPIUrl; }
            set { _baseAPIUrl = value; }
        }

        private string _apiVersion = "2.0.1";
        public string apiVersion
        {
            get { return _apiVersion; }
            set { _apiVersion = value; }
        }

        private string GetFormatType(OutputFormatType format)
        {
            return format.ToString().ToLower();
        }

        /// <summary>
        /// Takes in the URL and returns a shortened version of that URL
        /// </summary>
        /// <param name="longURL">URL to shorten</param>
        /// <returns>resultURL</returns>
        public string Shorten(string longURL)
        {
            string resultUrl = "";
            string strUrl = baseAPIUrl + "/shorten?version={0}&longUrl={1}&login={2}&apiKey={3}&format=xml";
            strUrl = string.Format(strUrl, apiVersion, System.Web.HttpUtility.UrlEncode(longURL), Username, apiKey);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(GetBitlyData(strUrl));
            XmlNode element = xmlDoc.SelectSingleNode("//bitly/results/nodeKeyVal/shortUrl");
            resultUrl = element.InnerText;
            return resultUrl;
        }

        /// <summary>
        /// Get bit.ly shortened URL in the XML format
        /// </summary>
        /// <param name="longURL">URL to shorten</param>
        /// <returns>xmlDoc</returns>
        public XmlDocument ShortenAsXML(string longURL)
        {
            string strUrl = baseAPIUrl + "/shorten?version={0}&longUrl={1}&login={2}&apiKey={3}&format={4}";
            strUrl = string.Format(strUrl, apiVersion, System.Web.HttpUtility.UrlEncode(longURL), Username, apiKey,GetFormatType(OutputFormatType.xml));
            string result = GetBitlyData(strUrl);
            if (!string.IsNullOrEmpty(result))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(result);
                return xmlDoc;
            }
            return null;
        }

        /// <summary>
        /// Get bit.ly shortened URL in JSON format
        /// </summary>
        /// <param name="longURL">URL to shorten</param>
        /// <returns>resultJSON</returns>
        public string ShortenAsJSON(string longURL)
        {
            string resultJSON = "";
            string strUrl = baseAPIUrl + "/shorten?version={0}&longUrl={1}&login={2}&apiKey={3}&format={4}";
            strUrl = string.Format(strUrl, apiVersion, System.Web.HttpUtility.UrlEncode(longURL), Username, apiKey,GetFormatType(OutputFormatType.json));
            resultJSON = GetBitlyData(strUrl);
            return resultJSON;
        }


        /// <summary>
        /// Get info about bit.ly link
        /// </summary>
        /// <param name="input">can be either a URL or hash</param>
        /// <returns>Result as type XmlDocument</returns>
        public XmlDocument InfoAsXML(string input)
        {
            string strUrl = baseAPIUrl + "/info?version={0}&login={1}&apiKey={2}&{3}={4}&format={5}";
            if (input.IndexOf("http://") > 0)
            {
                // Is a bit.ly url
                strUrl = string.Format(strUrl, apiVersion, Username, apiKey, "shortUrl", input, GetFormatType(OutputFormatType.xml));
                
            }
            else
            {
                // Is the hash of a url
                strUrl = string.Format(strUrl, apiVersion, Username, apiKey, "hash", input, GetFormatType(OutputFormatType.xml));
            }
            string result = GetBitlyData(strUrl);
            if (!string.IsNullOrEmpty(result))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(result);
                return xmlDoc;
            }
            return null;
        }

        public string InfoAsJSON(string input)
        {
            string strUrl = baseAPIUrl + "/info?version={0}&login={1}&apiKey={2}&{3}={4}&format={5}";
            if (input.IndexOf("http://") > 0)
            {
                // Is a bit.ly url
                strUrl = string.Format(strUrl, apiVersion, Username, apiKey, "shortUrl", input, GetFormatType(OutputFormatType.json));

            }
            else
            {
                // Is the hash of a url
                strUrl = string.Format(strUrl, apiVersion, Username, apiKey, "hash", input, GetFormatType(OutputFormatType.json));
            }
            return GetBitlyData(strUrl);
        }

        public XmlDocument StatsAsXML(string input)
        {
            string strUrl = baseAPIUrl + "/stats?version={0}&login={1}&apiKey={2}&{3}={4}&format={5}";
            if (input.IndexOf("http://") > 0)
            {
                // Is a bit.ly url
                strUrl = string.Format(strUrl, apiVersion, Username, apiKey, "shortUrl", input, GetFormatType(OutputFormatType.xml));

            }
            else
            {
                // Is the hash of a url
                strUrl = string.Format(strUrl, apiVersion, Username, apiKey, "hash", input, GetFormatType(OutputFormatType.xml));
            }
            string result = GetBitlyData(strUrl);
            if (!string.IsNullOrEmpty(result))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(result);
                return xmlDoc;
            }
            return null;
        }

        public string StatsAsJSON(string input)
        {
            string strUrl = baseAPIUrl + "/stats?version={0}&login={1}&apiKey={2}&{3}={4}&format={5}";
            if (input.IndexOf("http://") > 0)
            {
                // Is a bit.ly url
                strUrl = string.Format(strUrl, apiVersion, Username, apiKey, "shortUrl", input, GetFormatType(OutputFormatType.json));

            }
            else
            {
                // Is the hash of a url
                strUrl = string.Format(strUrl, apiVersion, Username, apiKey, "hash", input, GetFormatType(OutputFormatType.json));
            }
            return GetBitlyData(strUrl);
        }

        /// <summary>
        /// Returns xml document from url
        /// </summary>
        /// <param name="URL">URL that will return XML</param>
        /// <returns>xmlDoc</returns>
        private string GetBitlyData(string URL)
        {
            WebClient client = new WebClient();
            Stream data = client.OpenRead(URL);
            StreamReader reader = new StreamReader(data);
            return reader.ReadToEnd();
        }

    }
}
