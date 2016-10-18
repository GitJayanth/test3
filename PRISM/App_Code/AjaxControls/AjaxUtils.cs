using System;
using System.Xml;

  /// <summary>
  /// Description résumée de AjaxUtils.
  /// </summary>
  public class AjaxUtils
  {
    public AjaxUtils()
    {
      //
      // TODO : ajoutez ici la logique du constructeur
      //
    }
    public static  XmlNode addAttribute(ref XmlNode node, string attributeName, string nodeValue)
    {
      XmlNode newAttribute, cAttribute;
      cAttribute = node.Attributes.GetNamedItem(attributeName);
      if (cAttribute==null) 
      {
        newAttribute = node.OwnerDocument.CreateAttribute(attributeName);
        newAttribute.InnerText = nodeValue;
        node.Attributes.SetNamedItem(newAttribute);
        return  newAttribute;
      }
      else
      {
        cAttribute.Value = nodeValue;
        return cAttribute;
      }       
    }

  }
