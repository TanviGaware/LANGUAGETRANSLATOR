using System;
using System.Collections.Generic;
using System.Text;
using Automation.common;
using System.Diagnostics;


namespace Automation.Common
{
    /// <summary>
    /// This is base class for all automation scripts (Individual tasks).
    /// </summary>
    public abstract class AbstractScript
    {
        #region Constants
        /// <summary>
        /// path to store common eventcorder recording files relative tSo application startup path. 
        /// </summary>
        public const string pathOfCopyTakes = @"common/";
        #endregion

        #region Static (Class) Variables
        /// <summary>
        /// id of last script created. 
        /// </summary>
        //private static int lastId = 0;
        /// <summary>
        /// true if some eventcorder recording is currently running.
        /// This is used to stop simultaneous execution of eventcorder recordings.
        /// </summary>
        private static bool someScriptIsRunning = false;
        /// <summary>
        /// true if user has requested suspension of script execution.
        /// This is used to stop further subtasks once user ask for suspension especially in case of wrong execution. 
        /// </summary>
        public static bool userInterruption = false;
        /// <summary>
        /// To Store configuration related to a script. Configurations can be windows titles, user name or any other parameter.
        /// </summary>
        public static Dictionary<string, string> configurationProperties;
        #endregion

        #region Instance Variables
        /// <summary>
        /// string indicating type (name) of script (task). 
        /// </summary>
        protected string type;
        /// <summary>
        /// id of this script instance. 
        /// </summary>
        protected int id;
        /// <summary>
        /// Window Handler instance used for performing various window related functions like bringing window in foreground, getting window handle etc.
        /// </summary>
        protected WindowHandler windowHandler = null;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// This will initialize instance variables and load configuration if not loaded.
        /// </summary>
        /// <param name="a_type">This is a string indicating class name for script.</param>
        //public AbstractScript(string a_type)
        //{
        //    type = a_type;
        //    id = ++lastId;
        //    windowHandler = WindowHandler.getInstance();

        //    if (configurationProperties == null)
        //    {
        //        configurationProperties = PropertiesFileHandler.readPropertyFile(
        //            System.Net.Mime.MediaTypeNames.Application.StartupPath + "//configuration.properties", true);
        //    }
        //}
        #endregion

        #region Static Methods
        public static bool isSomeScriptRunning()
        {
            return someScriptIsRunning;
        }

        public static void throwExceptionIfUserInterruption()
        {
            if (userInterruption)
            {
                throw new Exception("User Interruption.");
            }
        }

        /// <summary>
        /// This method will get the monitor resolution. 
        /// </summary>
        /// <returns>monitor resolution</returns>
        /// 
        //public static Size getMonitorInfo()
        //{
        //    return SystemInformation.PrimaryMonitorSize;
        //}
        #endregion

        #region Instance Methods
        /// <summary>
        /// This is the method which will be called internally on script execution.
        /// Every Subclass script must implement this method and define the steps which should be executed.
        /// </summary>
        public abstract void process();

        /// <summary>
        /// This is the method which is called directly on script execution.
        /// </summary>
        public void execute()
        {
            preProcess();
            process();
            postProcess();
        }

        /// <summary>
        /// This is the method called before process method. 
        /// This method is used for performing commont tasks before process method execute like setting  boolean flags to indicate that script is currently being executed.
        /// The subclass may override the default implementation of this method but this method MUST be called from new implementation.
        /// </summary>
        public void preProcess()
        {
            userInterruption = false;
            someScriptIsRunning = true;
        }

        /// <summary>
        /// This is the method called after process method. 
        /// This method is used for performing commont tasks after process method execute like setting  boolean flags to indicate that script is execution is about to finish.
        /// The subclass may override the default implementation of this method but this method MUST be called from new implementation.
        /// </summary>
        public void postProcess()
        {
            someScriptIsRunning = false;
        }

        /// <summary>
        /// This method is used to show user message
        /// </summary>
        /// <param name="message">Message Text</param>
        /// <param name="caption">Title of Message Box Window</param>
        //public void showMessage(string message, string caption)
        //{
        //    windowHandler.SetWindowInForeground(Process.GetCurrentProcess().MainWindowHandle.ToInt32());
        //    MessageBox.Show(message, caption, MessageBoxButtons.OK,
        //        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
        //        MessageBoxOptions.DefaultDesktopOnly);
            

        //}

        /// <summary>
        ///  This method is used for confirmation from user.
        /// </summary>
        ///  <param name="message">Message Text</param>
        /// <param name="caption">Title of Message Box Window</param>
        /// <returns></returns>
        //public bool askForYesOrNo(string message, string caption)
        //{
        //    DialogResult result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo,
        //     MessageBoxIcon.Information, MessageBoxDefaultButton.Button2,
        //     MessageBoxOptions.DefaultDesktopOnly);

        //    if (result.Equals(DialogResult.Yes))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        /// <summary>
        /// This method is used to remove new line characters from a string. 
        /// This is helpful when applying regular exepression on  a string as including new line characters in regular expression can be tricky . 
        /// </summary>
        /// <param name="line">text contaiining new line characters</param>
        /// <returns></returns>
        public string removeNewLine(string line)
        {
            //LogHandler.getLogFile().WriteText("starting removeNewLine " + line);

            if (line == null || line.Trim().Length == 0)
            {
                return "";
            }

            string result = line.Replace(System.Environment.NewLine, " ");
            //LogHandler.getLogFile().WriteText("ending removeNewLine " + result);
            return result;
        }

        /// <summary>
        ///  This is the helper method for to change the value of combobox using eventcorder.
        ///  This method will use keyborad events generated by eventcorder.
        /// </summary>
        /// <param name="newValue">Target Value of Combobox</param>
        /// <param name="currentValue">Current value of Combobox</param>
        /// <param name="pathOfTakeForSelectDropDown">Path of eventcorder recording to select the combobox whose value needs to be changed.</param>
        /// <param name="dropDownValues">List of drop down value in Correct ORDER.</param>
        //public void changeDropDownValue(string newValue, string currentValue,
        //   string pathOfTakeForSelectDropDown, List<string> dropDownValues)
        //{
        //    if (!currentValue.Equals(newValue))
        //    {
        //        EventcorderWrapper.getInstance().playEventsAndThrowExceptionOnInterrupt(pathOfTakeForSelectDropDown);
        //        changeDropDownValue(newValue, currentValue, dropDownValues);
        //    }
        //}

        //public static void changeDropDownValue(string newValue, string currentValue, List<string> dropDownValues)
        //{
        //    bool useDownKey = false;
        //    int difference = 0;

         

        //    if (dropDownValues.IndexOf(newValue) >= 0)
        //    {
        //        if (dropDownValues.IndexOf(newValue) > dropDownValues.IndexOf(currentValue))
        //        {
        //            useDownKey = true;
        //            difference = dropDownValues.IndexOf(newValue) - dropDownValues.IndexOf(currentValue);
        //        }
        //        else
        //        {
        //            difference = dropDownValues.IndexOf(currentValue) - dropDownValues.IndexOf(newValue);
        //        }

        //        if (useDownKey)
        //        {
        //            for (int i = 0; i < difference; i++)
        //            {
        //                EventcorderWrapper.getInstance().playEventsAndThrowExceptionOnInterrupt(Application.StartupPath + @"/" + AbstractScript.pathOfCopyTakes + "downArrowKey.xml");
        //                System.Threading.Thread.Sleep(10);
        //            }
        //        }
        //        else
        //        {
        //            for (int i = 0; i < difference; i++)
        //            {
        //                EventcorderWrapper.getInstance().playEventsAndThrowExceptionOnInterrupt(Application.StartupPath + @"/" + AbstractScript.pathOfCopyTakes + "upArrowKey.xml");
        //                System.Threading.Thread.Sleep(10);
        //            }
        //        }
        //        EventcorderWrapper.getInstance().playEventsAndThrowExceptionOnInterrupt(Application.StartupPath + @"/" + AbstractScript.pathOfCopyTakes + "enterKey.xml");
        //        EventcorderWrapper.getInstance().playEventsAndThrowExceptionOnInterrupt(Application.StartupPath + @"/" + AbstractScript.pathOfCopyTakes + "enterKey.xml");
        //    }

        //}

        /// <summary>
        /// This is also helper method for to change the value of combobox using eventcorder.
        ///  This method should be used when the combobox contains the values which can not fit into its size and so only part of value is visible.
        /// </summary>
        /// <param name="newValue">Target Value of Combobox</param>
        /// <param name="partOfCurrentValue">Unique substring of visible part of Current value of Combobox</param>
        /// <param name="pathOfTakeForSelectDropDown">Path of eventcorder recording to select the combobox whose value needs to be changed.</param>
        /// <param name="dropDownValues">List of drop down value in Correct ORDER.</param>
        //public void changeDropDownValueWithPartOfCurrentValue(string newValue, string partOfCurrentValue,
        //   string pathOfTakeForSelectDropDown, List<string> dropDownValues)
        //{
        //    //LogHandler.getLogFile().WriteText("staring changeDropDownValueWithPartOfCurrentValue" + newValue + partOfCurrentValue + dropDownValues);
        //    if (!newValue.StartsWith(partOfCurrentValue))
        //    {
        //        EventcorderWrapper.getInstance().playEventsAndThrowExceptionOnInterrupt(pathOfTakeForSelectDropDown);
        //        bool useDownKey = false;
        //        int difference = 0;

        //        int indexOfPartOfCurrentValue = -1;
        //        for (int i = 0; i < dropDownValues.Count; i++)
        //        {
        //            if (dropDownValues[i].StartsWith(partOfCurrentValue))
        //            {
        //                indexOfPartOfCurrentValue = i;
        //                break;
        //            }
        //        }

        //        if (dropDownValues.IndexOf(newValue) > indexOfPartOfCurrentValue)
        //        {
        //            useDownKey = true;
        //            difference = dropDownValues.IndexOf(newValue) - indexOfPartOfCurrentValue;
        //        }
        //        else
        //        {
        //            difference = indexOfPartOfCurrentValue - dropDownValues.IndexOf(newValue);
        //        }

        //        if (useDownKey)
        //        {
        //            for (int i = 0; i < difference; i++)
        //            {
        //                EventcorderWrapper.getInstance().playEventsAndThrowExceptionOnInterrupt(Application.StartupPath + @"/" + AbstractScript.pathOfCopyTakes + "downArrowKey.xml");
        //            }
        //        }
        //        else
        //        {
        //            for (int i = 0; i < difference; i++)
        //            {
        //                EventcorderWrapper.getInstance().playEventsAndThrowExceptionOnInterrupt(Application.StartupPath + @"/" + AbstractScript.pathOfCopyTakes + "upArrowKey.xml");
        //            }
        //        }
        //        EventcorderWrapper.getInstance().playEventsAndThrowExceptionOnInterrupt(Application.StartupPath + @"/" + AbstractScript.pathOfCopyTakes + "enterKey.xml");
        //        //LogHandler.getLogFile().WriteText("ending changeDropDownValueWithPartOfCurrentValue");
        //    }
        //}

        /// <summary>
        /// Ths method sets the specified value to system clipboard.
        /// </summary>
        /// <param name="text">value to set to clipboard.</param>
        //public void setTextToClipboard(string text)
        //{
        //    try
        //    {
        //        Clipboard.SetText(text);
        //    }
        //    catch (Exception e)
        //    {
        //        //LogHandler.getLogFile().WriteError(e);
        //    }
        //}

        /// <summary>
        /// This value gets current value from system  clipboard.
        /// </summary>
        /// <returns></returns>
        //public string getTextFromClipboard()
        //{
        //    return Clipboard.GetText();
        //}

        /// <summary>
        /// This method rethrows eventcorder exception in case of user interruption.
        /// </summary>
        /// <param name="e">eventcorder exception</param>
        /// 
        //public void throwAgainForUserInterruption(EventcorderException e)
        //{
        //    if (e.errorCode == EventcorderException.USER_INTERRUPTION)
        //    {
        //        throw e;
        //    }
        //}

        /// <summary>
        /// This method is used to wait for the specified clickview to appear on the screen.
        /// This will be used in case of screen refresh.
        /// </summary>
        /// <param name="takeWithClickView">path of clickview</param>
        /// 

        //public void waitForClickview(string takeWithClickView)
        //{
        //    bool clickViewAppears = false;
        //    while (!clickViewAppears)
        //    {
        //        throwExceptionIfUserInterruption();
        //        try
        //        {
        //            EventcorderWrapper.getInstance().playEventsAndThrowExceptionOnInterrupt(takeWithClickView);
        //            clickViewAppears = true;
        //        }
        //        catch (EventcorderException ex)
        //        {
        //            //LogHandler.getLogFile().WriteError(ex);
        //            throwAgainForUserInterruption(ex);
        //            System.Threading.Thread.Sleep(200);
        //        }
        //    }
        //}


        /// <summary>
        /// This method is used to wait for the specified clickview to appear on the screen and press enter if clickview is visible.
        /// This will be used in case of screen refresh.
        /// </summary>
        /// <param name="takeWithClickView">path of clickview take</param>
        /// <param name="takeWithEnter">path of enter take</param>
        //public void waitForClickviewAndEnter(string takeWithClickView, string takeWithEnter)
        //{
        //    bool clickViewAppears = false;
        //    while (!clickViewAppears)
        //    {
        //        throwExceptionIfUserInterruption();
        //        try
        //        {
        //            EventcorderWrapper.getInstance().playEventsAndThrowExceptionOnInterrupt(takeWithClickView);
        //            clickViewAppears = true;
        //        }
        //        catch (EventcorderException ex)
        //        {
        //            //LogHandler.getLogFile().WriteError(ex);
        //            throwAgainForUserInterruption(ex);
        //            System.Threading.Thread.Sleep(20);

        //            EventcorderWrapper.getInstance().playEventsAndThrowExceptionOnInterrupt(takeWithEnter);
        //        }
        //    }
        //}
        #endregion
    }
}


