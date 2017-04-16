using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SM.Utilities
{
    public class XmlUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object DeserializeFromXmlFile(string filePath, Type type)
        {
            var reader = new StreamReader(filePath);
            var xmlSerial = new XmlSerializer(type);
            object obj = xmlSerial.Deserialize(reader);
            reader.Close();
            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlContent"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object DeserializeFromXmlString(string xmlContent, Type type)
        {
            var reader = new StringReader(xmlContent);
            var xmlSerial = new XmlSerializer(type);
            object obj = xmlSerial.Deserialize(reader);
            reader.Close();
            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="obj"></param>
        public static void SerializeToXmlFile(string filePath, object obj)
        {
            var writer = new StreamWriter(filePath);
            var xmlSerial = new XmlSerializer(obj.GetType());
            xmlSerial.Serialize(writer, obj);
            writer.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToXmlString(object obj)
        {
            var writer = new StringWriter(new StringBuilder());
            new XmlSerializer(obj.GetType()).Serialize(writer, obj);
            string str = writer.ToString().Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
            str = str.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
            str = str.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "").Trim();
            return str;
        }

        public static string GetNodeValue(XmlNodeList nodeList, string childNodeName)
        {
            for (int i = 0; i < nodeList.Count; i++)
            {
                var xmlNode = nodeList.Item(i);
                if (xmlNode != null)
                    switch (xmlNode.NodeType)
                    {
                        case XmlNodeType.Element:
                            XmlElement element = (XmlElement) nodeList.Item(i);
                        
                            if (element != null && element.Name.Equals(childNodeName,StringComparison.CurrentCultureIgnoreCase))
                            {
                                return element.InnerText;
                            }
                            break;
                    }
            }
            return "";
        }

        public static string GetNodeValue(XmlDocument xmlDocument, string childNodeName)
        {
            try
            {
                return xmlDocument.GetElementsByTagName(childNodeName)[0].InnerText;
            }
            catch 
            {
                return "";               
            }
        }
    }
}
