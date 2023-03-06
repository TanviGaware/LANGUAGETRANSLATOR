using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using Automation.common;
using System.Configuration;
using System.Linq;
using System.Threading;
using UBSAPConnectivity;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;


namespace DocTranslate
{
    public partial class Form1 : Form
    {
        public static string strProcessStartTime = null;
        public static BackgroundWorker backgroundWorker1 = new BackgroundWorker();
        int DocCount = 0;
        string fromLang = String.Empty;
        string toLang = string.Empty;

        public Form1()
        {
            InitializeComponent();

        }

        IWebDriver driver;



        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                string path = ConfigurationManager.AppSettings["InputFilePath"].ToString();//Getting input folder path from app config

                string FinalPath = null;
                DirectoryInfo d = new DirectoryInfo(path);
                FileInfo[] Files = d.GetFiles("*.docx"); //Getting docx files
                string str = "";
                foreach (FileInfo file in Files)
                {


                    DocCount++;

                    str = file.Name;


                    this.Hide();

                    frmWait f1 = new frmWait("Please Wait  " + file + " is being processed!!!");
                    f1.Show();



                    float size = file.Length;
                    float FileSizeInMB = (size / 1024f) / 1024f;

                    if (FileSizeInMB > 7)
                    {

                        MessageBox.Show("File cannot be uploaded because size is greater than 7 MB");
                        f1.Close();

                        f1.Dispose();
                        continue;


                    }

                    else
                    {
                        FinalPath = string.Concat(path, str);//concat path with file name

                        driver = new ChromeDriver();

                    gotoRedirectURL:
                        try
                        {
                            driver.Url = "https://www.onlinedoctranslator.com/translationform";
                            Thread.Sleep(10000);
                          //  driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                            driver.Manage().Window.Maximize();

                            Thread t1 = new Thread(Test);
                            t1.Start();

                            func_WriteLogFileProcessLog("website started");
                        }
                        catch (Exception)
                        {

                            System.Threading.Thread.Sleep(1000);
                            goto gotoRedirectURL;
                        }


                        string inp = string.Empty;

                        //*[@id='upload-document-form']/div/button

                        IWebElement btnUploadFile = driver.FindElement(By.XPath("//*[@id='upload-document-form']/div/button"));
                        btnUploadFile.Click();
                        Thread.Sleep(1000);
                        func_WriteLogFileProcessLog("upload button clicked");

                        System.Threading.Thread.Sleep(200);

                        sendWinKey(FinalPath);

                        while (windowChkByCaption("Open") != 0)
                        {
                            sendWinKey("~");

                        }

                        //*[@id="from"]

                        IWebElement detectLang = driver.FindElement(By.XPath("//*[@id='from']"));

                        while (detectLang.Text != "")
                        {
                            detectLang = driver.FindElement(By.XPath("//*[@id='from']"));
                            if (detectLang.Text == "Detect language")
                            {
                                continue;
                            }
                            else if (detectLang.Text.StartsWith("Afrikaans\r\n"))
                            {
                                break;
                            }


                        }

                        IWebElement detectTo = driver.FindElement(By.XPath("//*[@id='to']"));

                        System.Threading.Thread.Sleep(1000);
                        func_WriteLogFileProcessLog("file uploaded successfully");


                        IWebElement btnTranslate = driver.FindElement(By.XPath("//*[@id='translation-button']"));
                        btnTranslate.Click();
                        Thread.Sleep(2000);

                        IWebElement frmWait = driver.FindElement(By.XPath(" //*[@id='progress-percentage']"));
                        Thread.Sleep(1000);

                        while (frmWait.Text != "All done!")
                        {

                            if (frmWait.Text != "All done!")
                            {
                                continue;
                            }
                            else
                            {
                                driver.Url = "https://www.onlinedoctranslator.com/translationprocess";

                                Thread.Sleep(5000);
                                // driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5000);

                                frmWait = driver.FindElement(By.XPath(" //*[@id='progress-percentage']"));
                                if (frmWait.Text == "All done!")
                                {
                                    break;
                                }
                            }
                           
                           
                        }

                        System.Threading.Thread.Sleep(1000);
                        //To save translated file

                        IWebElement frmdownload = driver.FindElement(By.XPath("//*[@id='start-download']/p[1]/a"));
                        frmdownload.Click();
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception" + ex);
            }

            // obj_IE.Close();
            // f1.Close();
        //    MessageBox.Show("Translation Completed Successfully");
            this.Close();
            System.Environment.Exit(0);


        }

        public void Test()
        {
            //if (obj_IE != null)
            //{
            //    while (true)
            //    {
            //        obj_IE.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Hide);
            //    }
            //}
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            strProcessStartTime = DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss");
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= 100; i++)
            {
                // Wait 100 milliseconds.
                Thread.Sleep(100);
                // Report progress.
                backgroundWorker1.ReportProgress(i);
            }
        }

        //private void backgroundWorker1_ProgressChanged(object sender,ProgressChangedEventArgs e)
        //{
        //    // Change the value of the ProgressBar to the BackgroundWorker progress.
        //    progressBar1.Value = e.ProgressPercentage;
        //    // Set the text.
        //    this.Text = e.ProgressPercentage.ToString();
        //}

        internal int windowChkByClass(string Class)
        {
            try
            {
                WindowHandler windowHandler = null;
                windowHandler = WindowHandler.getInstance();
                int sapWindowHandle = windowHandler.FindWindowHandle("MozillaDialogClass", null);

                if (sapWindowHandle == 0)
                {
                    System.Threading.Thread.Sleep(1000);
                    //   goto label_Check;
                }
                else
                {
                    windowHandler.SetWindowInForeground(sapWindowHandle);
                    //windowHandler.Maximize(sapWindowHandle);
                }
                return sapWindowHandle;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        internal int windowChkByCaption(string Caption)
        {
            try
            {
                WindowHandler windowHandler = null;
                windowHandler = WindowHandler.getInstance();
                int sapWindowHandle = windowHandler.FindWindowHandle(null, Caption);

                if (sapWindowHandle == 0)
                {
                    System.Threading.Thread.Sleep(1000);
                    //   goto label_Check;
                }
                else
                {
                    windowHandler.SetWindowInForeground(sapWindowHandle);
                    //windowHandler.Maximize(sapWindowHandle);

                }
                return sapWindowHandle;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return 0;
            }
        }


        internal int windowCloseByCaption(string Caption)
        {
            try
            {
                WindowHandler windowHandler = null;
                windowHandler = WindowHandler.getInstance();
                int MozillaWindowHandle = windowHandler.FindWindowHandle(null, Caption);
                if (MozillaWindowHandle == 0)
                {
                    System.Threading.Thread.Sleep(1000);
                    //   goto label_Check;
                }
                else
                {
                    windowHandler.SetWindowInForeground(MozillaWindowHandle);
                    //windowHandler.Maximize(sapWindowHandle);
                    windowHandler.Close(MozillaWindowHandle);
                }
                return MozillaWindowHandle;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        internal int windowChkByClassMozilla(string Class)
        {
            try
            {
                WindowHandler windowHandler = null;
                windowHandler = WindowHandler.getInstance();
                int MozillaWindowHandle = windowHandler.FindWindowHandle("MozillaWindowClass", null);
                if (MozillaWindowHandle == 0)
                {
                    System.Threading.Thread.Sleep(1000);
                    //   goto label_Check;
                }
                else
                {
                    windowHandler.SetWindowInForeground(MozillaWindowHandle);
                    //windowHandler.Maximize(sapWindowHandle);
                }
                return MozillaWindowHandle;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }



        public static void func_WriteLogFileProcessLog(String strData)
        {
            try
            {

                //                frmWait

                string strDirectory = AppDomain.CurrentDomain.BaseDirectory + "ProcessLog";
                //string strDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");

                if (System.IO.Directory.Exists(strDirectory) == false)
                {
                    Directory.CreateDirectory(strDirectory);
                }

                String strLogFile = strDirectory + "\\Log of " + strProcessStartTime + ".txt";
                if (File.Exists(strLogFile) == false)
                {

                    File.WriteAllText(strLogFile, "");
                }

                if (strData == Environment.NewLine)
                {
                    File.AppendAllText(strLogFile, Environment.NewLine);
                }
                else
                {
                    File.AppendAllText(strLogFile, DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt") + " >>> " + strData + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while writing Log File.\n" + ex.Message.ToString());
                return;
            }
        }

        public void sendWinKey(string keys)
        {
            SendKeys.Flush();
            SendKeys.Send(keys);
            SendKeys.Flush();
            System.Threading.Thread.Sleep(500);
        }
    }
}