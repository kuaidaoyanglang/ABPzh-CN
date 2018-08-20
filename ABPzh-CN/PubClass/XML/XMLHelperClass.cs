
using System;
using System.Collections.Generic;
using System.Xml;

namespace ABPzh_CN.PubClass.XML
{
  public class XMLHelperClass
  {
    public List<TextClass> Texts = new List<TextClass>();
    public List<TextClass> TextsOK = new List<TextClass>();
    public List<TextClass> TextsNo = new List<TextClass>();

    public void writeXml(string xmlPath)
    {
      XmlDocument xmlDocument = new XmlDocument();
      XmlNode element1 = (XmlNode) xmlDocument.CreateElement("texts");
      foreach (TextClass textClass in this.TextsOK)
      {
        XmlElement element2 = xmlDocument.CreateElement("text");
        element2.SetAttribute("name", textClass.Name);
        if (textClass.IsValue)
          element2.SetAttribute("value", textClass.Value);
        else
          element2.InnerText = textClass.Value;
        element1.AppendChild((XmlNode) element2);
      }
      XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", (string) null);
      xmlDocument.AppendChild((XmlNode) xmlDeclaration);
      XmlElement element3 = xmlDocument.CreateElement("localizationDictionary");
      element3.SetAttribute("culture", "zh-CN");
      element3.AppendChild(element1);
      xmlDocument.AppendChild((XmlNode) element3);
      xmlDocument.Save(xmlPath);
    }

    public void loadXml(string xmlPath)
    {
      XmlDocument xmlDocument = new XmlDocument();
      XmlNodeList xmlNodeList;
      try
      {
        xmlDocument.Load(xmlPath);
        xmlNodeList = xmlDocument.SelectNodes("//text");
        if (xmlNodeList.Count == 0)
          return;
      }
      catch (Exception ex)
      {
        return;
      }
      this.Texts.Clear();
      int num = 0;
      foreach (XmlNode xmlNode in xmlNodeList)
      {
        XmlAttributeCollection attributes = xmlNode.Attributes;
        if (attributes.GetNamedItem("value") == null)
        {
          this.Texts.Add(new TextClass()
          {
            Name = attributes.GetNamedItem("name").Value,
            Value = xmlNode.InnerText,
            IsValue = false,
            Index = num
          });
          ++num;
        }
        else
        {
          this.Texts.Add(new TextClass()
          {
            Name = attributes.GetNamedItem("name").Value,
            Value = attributes.GetNamedItem("value").Value,
            IsValue = true,
            Index = num
          });
          ++num;
        }
      }
    }
  }
}
