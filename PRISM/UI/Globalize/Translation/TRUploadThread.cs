#region uses
using System;
using System.Threading;
using HyperCatalog.Business;
using HyperCatalog.Shared;
using HyperCatalog.WebServices.TranslationWS;
#endregion

namespace hc_termbase.UI.Globalize.Translation
{
  /// <summary>
  /// Description résumée de TRUploadThread.
  /// </summary>
  public class TRUploadThread
  {
    public TRUploadThread()
    {
      //
      // TODO : ajoutez ici la logique du constructeur
      //
    }
    // property to indicate if task has run at least once.
    private bool _firstRunComplete = false;
    public bool firstRunComplete

    {

      get { return _firstRunComplete; }

    }

 

    // property to indicate iftask is running.

    private bool _running = false;
    public bool Running

    {

      get { return _running; }

    }

 

    //property to indicate whether the last task succeeded.

 

    public bool _lastTaskSuccess = true;
    public bool LastTaskSuccess

    {

      get

      {

        if (_lastFinishTime == DateTime.MinValue)

          throw new InvalidOperationException("The task has never completed.");

        return _lastTaskSuccess;

      }

    }

 

    //store any exception generated during the task.

    private Exception _exceptionOccured = null;
    public Exception ExceptionOccured

    {

      get { return _exceptionOccured; }

    }

 
    private DateTime _lastStartTime = DateTime.MinValue;
    public DateTime LastStartTime

    {

      get

      {

        if (_lastStartTime == DateTime.MinValue)

          throw new InvalidOperationException("The task has never started.");

        return _lastStartTime;

      }

    }

 
    private DateTime _lastFinishTime = DateTime.MinValue;
    public DateTime LastFinishTime

    {

      get

      {

        if (_lastFinishTime == DateTime.MinValue)

          throw new InvalidOperationException("The task has never completed.");

        return _lastFinishTime;

      }

    }


    // Start the task

    public void RunTask()

    {

      // Only one thread is allowed to enter here.

      lock (this)

      {

        if (!_running)

        {

          _running = true;

          _lastStartTime = DateTime.Now;

          Thread t = new Thread(new ThreadStart(DoWork));

          t.Start();

        }

        else

        {

          throw new InvalidOperationException("The task is already running!");

        }

      }

    }
 
    public void DoWork()
    {
      try
      {
        // Next line is a placeholder for your "job" - a DTS package or other long-running task
        // Replace the following line with your code.
        WSTranslation ws = HyperCatalog.WebServices.WSInterface.Translation;
        System.Net.CookieContainer cookies = new System.Net.CookieContainer();
        ws.CookieContainer = cookies;
        string credential = ws.SignOn(SessionState.User.Pseudo, SessionState.User.ClearPassword);
        System.Guid logId = new Guid();
        string zipFileName;
        ws.ProcessZipRequest(credential,logId, zipFileName);
        ws.SignOff(credential);
        // Set Success property.
        _lastTaskSuccess = true;

      }

      catch (Exception e)

      {
        // Task Failed.
        _lastTaskSuccess = false;
        _exceptionOccured = e;
      }

      finally
      {
        _running = false;
        _lastFinishTime = DateTime.Now;
        if (!_firstRunComplete) _firstRunComplete = true;
      }
    }
  }
}
