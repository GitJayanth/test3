//using AJAXControls;
using System;
using System.Xml;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Collections.Specialized;
using System.Web.Services;
using System.Web.Services.Protocols;
using HyperCatalog.Business;

namespace HyperCatalog.UI.Main.AjaxControls
{
	/// <summary>
	/// Description résumée de LookupTerm.
	/// </summary>
	[WebService(Namespace = "http://www.hypercatalog.fr/webservices/",
		 Description = "A WebService retreiving the names of static terms starting with a specific string.")]

	public class LookupStaticTerm : WebService//, ILookUpService
	{

		private const string CACHEENTRYNAME = "AjaxTerms";
		private const int MAXSUGGEST = 15;
  
  
		[WebMethod]
		[Obsolete("use GetPrefixedEntries")]
		public string Lookup_StartWith(string prefix) 
		{
			return (GetPrefixedEntries(prefix).OuterXml);
		} // HC_StartWith


		#region ILookUpService Members

		/// <summary>Return Lookup entries that start with the given prefix.</summary>
		/// <param name="prefix">A prefix string</param>
		/// <returns>Semikolon separated list of entries.</returns>
		[WebMethod(Description="Return Lookup entries that start with the given prefix.")]
		public XmlDocument GetPrefixedEntries(string prefix) 
		{
			DateTime t1 = DateTime.Now;
			CollectionView vTerms = GetTerms();
			vTerms.ApplyFilter("Value", prefix, CollectionView.FilterOperand.StartsWith);
			System.Xml.XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.AppendChild(xmlDoc.CreateElement("entries"));
			if(prefix!=null)
			{
				int nbRows = 0;
				try
				{
					for (int i=0;i< vTerms.Count;i++)
					{
						Term c = (Term)vTerms[i];
						XmlNode entryNode = xmlDoc.CreateElement("entry");
						AjaxUtils.addAttribute(ref entryNode, "key", c.Value);
						AjaxUtils.addAttribute(ref entryNode, "value", string.Empty);
						xmlDoc.DocumentElement.AppendChild(entryNode);
						nbRows ++;
						if (nbRows == MAXSUGGEST)
						{
							break;
						}
					}
				}
				catch
				{
					HttpRuntime.Cache.Remove(CACHEENTRYNAME);
				}
			}
			return xmlDoc;
		} // GetPrefixedEntries

  
		public string GetEntry(string Entry) 
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion


		#region ----- private members -----

		/// <summary>Get the current List of HC_ and load if necessary.</summary>
		private static CollectionView GetTerms() 
		{
			Cache c = HttpRuntime.Cache;
			CollectionView view = (CollectionView)c[CACHEENTRYNAME];
			if (view == null) 
			{
        using (TermList Terms = HyperCatalog.Business.Term.GetAll("TermTypeCode = 'S'"))
        {
          view = new CollectionView(Terms);
          view.ApplySort("Value", System.ComponentModel.ListSortDirection.Ascending);
          c.Insert(CACHEENTRYNAME, view, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(30), CacheItemPriority.High, null);
        }
			} // if
			return view;

		} // Load
  
		#endregion
  

	} // class
}
