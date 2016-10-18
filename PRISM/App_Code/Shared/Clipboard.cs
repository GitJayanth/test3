#region using
using System;
using System.Collections;
using System.Collections.Specialized;
using HyperCatalog.Business;
#endregion

namespace HyperCatalog.Shared.Defs
{
  public enum ClipboardAction
  {
    Copy, Cut
  }

  #region ClipboardItem
  [Serializable]
  public class ClipboardItem
  {
    private System.Int64 _ItemId;
    private int _ContainerId;
    private string _ContainerName;
    private string _CultureCode;

    public System.Int64 ItemId
    {
      get { return _ItemId;}
      set { _ItemId = value; }
    }
    public int ContainerId
    {
      get { return _ContainerId;}
      set { _ContainerId = value; }
    }
    public string CultureCode
    {
      get { return _CultureCode;}
      set { _CultureCode = value; }
    }
    public string ContainerName
    {
      get { return _ContainerName;}
      set { _ContainerName = value; }
    }
    public ClipboardItem(System.Int64 itemId, int containerId, string containerName, string cultureCode)
    {
      _ItemId = itemId;
      _ContainerId = containerId;
      _ContainerName = containerName;
      _CultureCode = cultureCode;
    }
  }
  #endregion

  #region "Object list"
  [Serializable()]
  public class ClipboardItemList:SortableCollectionBase, IEnumerable
  {
    // Method implementation from the CollectionBase class
    public void Add(ClipboardItem aClipboardItem)
    {     
      List.Add(aClipboardItem);      
    }       

    // Method implementation from the CollectionBase class
    public void Remove(int index)
    {
      // Check to see if there is a ClipboardItem at the supplied index.
      if (index > Count - 1 || index < 0) 
      {
        // Handle the error that occurs if the valid page index is       
        // not supplied.    
        // This exception will be written to the calling function             
        throw new Exception("Index out of bounds");            
      }        
      List.RemoveAt(index);       
    } 
    // Method implementation from the CollectionBase class
    public virtual ClipboardItem Item(int Index) 
    {     
      // The appropriate item is retrieved from the List object and     
      // explicitly cast to the ClipboardItem type, then returned to the      
      // caller.     
      return (ClipboardItem) List[Index]; 
    }

    // Implement the method GetEnumerator from the longerface IEnumerable       
    public new ClipboardItemEnumerator GetEnumerator() 
    {           
      return new ClipboardItemEnumerator(this);        
    }        
    // Implement the GetEnumerator() method:        
    IEnumerator IEnumerable.GetEnumerator() 
    {            
      return GetEnumerator();        
    }

  }

  /*Define the Enumeration class to return */       
  public class ClipboardItemEnumerator: IEnumerator 
  { 
    int nIndex;            
    ClipboardItemList collection;            
    public ClipboardItemEnumerator(ClipboardItemList coll) 
    {                
      collection = coll;                
      nIndex = -1;            
    }
    // Sets the enumerator to its initial position, which is
    // before the first element in the collection.
    public void Reset() 
    { 
      nIndex = -1;
    }
    // Advances the enumerator to the next element of the 
    // collection.            
    public bool MoveNext() 
    {
      nIndex++;                
      return(nIndex < collection.Count);            
    }
    // Gets the current element in the collection.
    public ClipboardItem Current 
    {
      get 
      {                    
        return((ClipboardItem)collection.Item(nIndex));                
      }            
    }            
    // The current property on the IEnumerator longerface: 
    object IEnumerator.Current 
    {                
      get 
      {                    
        return(Current);                
      }            
    }     
  } // End of class ClipboardItemEnumeration
  #endregion

  #region Clipboard class
  /// <summary>
	/// Description résumée de CopyCutItems.
	/// </summary>
  [Serializable]
  public class Clipboard
  {
    private ClipboardAction _Action = ClipboardAction.Copy;
    private ClipboardItemList _Items = new ClipboardItemList();
    private int _InputFormId = -1;
    private System.Int64 _ItemId = -1;
    private string _CultureCode = HyperCatalog.Shared.SessionState.MasterCulture.Code;

    public ClipboardAction Action
    {
      get { return _Action; }
      set { _Action = value; }
    }

    public System.Int64 ItemId
    {
      get { return _ItemId; }
      set { _ItemId = value; }
    }
    public int InputFormId
    {
      get { return _InputFormId; }
      set { _InputFormId = value; }
    }

    public string CultureCode
    {
      get { return _CultureCode; }
      set { _CultureCode = value; }
    }

    public ClipboardItemList Items
    {
      get { return _Items; }
    }

    #region Constructors
    public Clipboard()
    {
    }

    public Clipboard(System.Int64 itemId)
    {
      _ItemId = itemId;
    }

    public Clipboard(string cultureCode)
    {
      _CultureCode = cultureCode;
    }
    public Clipboard(System.Int64 itemId, string cultureCode)
    {
      _ItemId = itemId;
      _CultureCode = cultureCode;
    }

    public Clipboard(System.Int64 itemId, int inputFormId)
    {
      _ItemId = itemId;
      _InputFormId = inputFormId;
    }

    public Clipboard(int inputFormId)
    {
      _InputFormId = inputFormId;
    }

    public Clipboard(int inputFormId, string cultureCode)
    {
      _InputFormId = inputFormId;
      _CultureCode = cultureCode;
    }
    public Clipboard(System.Int64 itemId, int inputFormId, string cultureCode)
    {
      _ItemId = itemId;
      _InputFormId = inputFormId;
      _CultureCode = cultureCode;
    }
    
    
    public Clipboard(int inputFormId, ClipboardAction action)
    {
      _Action = action;
    }

    public Clipboard(ClipboardAction action)
    {
      _Action = action;
		}
    #endregion
	}
  #endregion
}
