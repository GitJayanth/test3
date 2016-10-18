using System;
using System.Web.UI;
using System.Resources;
using System.Reflection;
using System.Runtime.CompilerServices;

#region Standard Attributes
[assembly: AssemblyCompany("HyperObjects")]
[assembly: AssemblyCopyright("Copyright(c) 1996-2005 HyperObjects")]
[assembly: AssemblyTrademark("HyperCatalog")]
[assembly: NeutralResourcesLanguageAttribute("en-US")]	
#endregion

//////////////////////////////////////////////////////////////////////////////////////////
/// History
/// 1.0.0.1 - 25/04/04: [Enhancement] Updated Clipboard Definition (Philippe)
/// 1.0.0.2 - 14/10/05: [Enhancement] Web Service definitions (Philippe)
/// 1.0.0.3 - 14/01/06: [Bug] Web Service popup menu correction (Philippe)
/// 1.0.0.4 - 30/01/06: [Enhancement] Forced zip in HCPage
/// 1.0.0.5 - 15/03/06: [Enhancement] Manage roll (create or delete) in menu of the tree (Mickael)
/// 1.0.0.6 - 10/04/06: [Bug] Remove style for grid with sort columns (Mickael)
/// 1.0.0.7 - 12/04/06: [Enhancement] Add web service to return static term list (Mickael)
/// 1.0.0.8 - 14/04/06: [Enhancement] Add new HiglightGridRowFilter methods with list of column index (Mickael)
/// 1.0.0.9 - 20/04/06: [Enhancement] (CRYS-175) Remove of popup 'Menu' Add Child, Delete, Create Roll, Delete Roll, InputForm, Template if item is not in scope (Mickael)
/// 1.0.1.0 - 25/04/06: [Enhancement] Add sub menu Clone item (Mickael)
/// 1.0.1.1 - 25/04/06: [Enhancement] Add sub menu Spell check and Move status to (Mickael)
/// 1.0.1.2 - 25/04/06: [Enhancement] Add parameter HasRoll for treeview to lookup children (Mickael)
/// 1.0.1.3 - 20/06/06: [Enhancement] Add sub menu Move item (Pervenche)
/// 1.0.1.4 - 30/06/06: [Enhancement] Spell checker only in master language and Move status only master and regional language (Mickael)
/// 1.0.1.5 - 30/06/06: [Enhancement] Update max level (in HyperSettings database) for soft roll, preview, compare and compare with (Mickael)
/// 1.0.1.6 - 04/07/06: [Enhancement] Update menu in treeview for 'Create Roll' (Mickael)
//////////////////////////////////////////////////////////////////////////////////////////
#region Assembly related attributes
[assembly: AssemblyDescription("HyperCatalog Core Shared Information")]
[assembly: AssemblyVersion("1.0.1.6")]
#endregion

#region  Maintainance related attributes
[assembly: CLSCompliant(false)]
[assembly: AssemblyDelaySign(false)]
//Cannot 
#endregion
