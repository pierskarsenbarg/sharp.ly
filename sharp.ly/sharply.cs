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
            xmlDoc = GetBitlyXML(strUrl);
            XmlNode element = xmlDoc.SelectSingleNode("//bitly/results/nodeKeyVal/shortUrl");
            resultUrl = element.InnerText;
            return resultUrl;
        }

        private XmlDocument GetBitlyXML(string URL)
        {
            WebClient client = new WebClient();
            Stream data = client.OpenRead(URL);
            StreamReader reader = new StreamReader(data);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(reader.ReadToEnd());
            return xmlDoc;
        }

    }
}
