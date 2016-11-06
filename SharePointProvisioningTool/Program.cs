using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace Karabina.SharePoint.Provisioning
{    
    static class Program
    {

        
        public static SPReflection sharePoint2013OnPremises = null;

        public static SPReflection sharePoint2016OnPremises = null;

        public static SPReflection sharePoint2016Online = null;
        

        public static SharePointVersion currentVerion = SharePointVersion.SharePoint_Invalid;


        /*
        private static Assembly ResolveAssemblyHandler(object source, ResolveEventArgs e)
        {
            Assembly result = null;
            switch (currentVerion)
            {
                case SharePointVersion.SharePoint_2013_On_Premises:
                    result = Assembly.LoadFrom(appDomain2013OP.BaseDirectory);

                    break;

                case SharePointVersion.SharePoint_2016_On_Premises:
                    result = Assembly.LoadFrom(appDomain2016OP.BaseDirectory);

                    break;

                case SharePointVersion.SharePoint_2016_OnLine:
                    result = Assembly.LoadFrom(appDomain2016OL.BaseDirectory);

                    break;

                default:
                    result = Assembly.GetExecutingAssembly();

                    break;

            }

            return result;

        } //ResolveAssemblyHandler
        */

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            //appDomain2013OP
            AppDomainSetup setup2013OP = new AppDomainSetup()
            {
                ApplicationBase = @"C:\Development\KarabinaSharePointTools\SharePoint2013OnPrem\bin\Debug\",
                ApplicationName = "SharePoint2013OnPrem",
                PrivateBinPath = @"C:\Development\KarabinaSharePointTools\SharePoint2013OnPrem\bin\Debug\",
                //ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                DisallowApplicationBaseProbing = false,
                DisallowBindingRedirects = false

            };

            AppDomain appDomain2013OP = AppDomain.CreateDomain("SharePoint 2013 On Premises", null, setup2013OP);

            object sp2013OP = appDomain2013OP.CreateInstanceFrom(setup2013OP.ApplicationBase + setup2013OP.ApplicationName + ".dll", Constants.SharePoint2013OnPrem_Type_FullName).Unwrap();

            sharePoint2013OnPremises = new SPReflection(appDomain2013OP, sp2013OP);


            //appDomain2016OP
            AppDomainSetup setup2016OP = new AppDomainSetup()
            {
                ApplicationBase = @"C:\Development\KarabinaSharePointTools\SharePoint2016OnPrem\bin\Debug\",
                ApplicationName = "SharePoint2016OnPrem",
                PrivateBinPath = @"C:\Development\KarabinaSharePointTools\SharePoint2016OnPrem\bin\Debug\",
                ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                DisallowApplicationBaseProbing = false,
                DisallowBindingRedirects = false

            };

            AppDomain appDomain2016OP = AppDomain.CreateDomain("SharePoint 2016 On Premises", null, setup2016OP);

            object sp2016OP = appDomain2016OP.CreateInstanceFrom(setup2016OP.ApplicationBase + setup2016OP.ApplicationName + ".dll", Constants.SharePoint2016OnPrem_Type_FullName).Unwrap();

            sharePoint2016OnPremises = new SPReflection(appDomain2016OP, sp2016OP);


            //appDomain2016OL
            AppDomainSetup setup2016OL = new AppDomainSetup()
            {
                ApplicationBase = @"C:\Development\KarabinaSharePointTools\SharePoint2016Online\bin\Debug\",
                ApplicationName = "SharePoint2016Online",
                PrivateBinPath = @"C:\Development\KarabinaSharePointTools\SharePoint2016Online\bin\Debug\",
                ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                DisallowApplicationBaseProbing = false,
                DisallowBindingRedirects = false

            };

            AppDomain appDomain2016OL = AppDomain.CreateDomain("SharePoint 2016 Online", null, setup2016OL);

            object sp2016OL = appDomain2016OL.CreateInstanceFrom(setup2016OL.ApplicationBase + setup2016OL.ApplicationName + ".dll", Constants.SharePoint2016Online_Type_FullName).Unwrap();

            sharePoint2016Online = new SPReflection(appDomain2016OL, sp2016OL);


            currentVerion = SharePointVersion.SharePoint_Invalid;

            AppDomain current = AppDomain.CurrentDomain;

            AppDomainSetup currentSetup = new AppDomainSetup()
            {
                ApplicationBase = current.SetupInformation.ApplicationBase,
                ConfigurationFile = current.SetupInformation.ConfigurationFile,
                DisallowApplicationBaseProbing = false,
                DisallowBindingRedirects = false

            };

            AppDomain appDomainCurrent = AppDomain.CreateDomain(AppDomain.CurrentDomain.FriendlyName, null, currentSetup);

            Type sppToolType  = typeof(SPPTool);

            SPPTool sppTool = (SPPTool)appDomainCurrent.CreateInstanceAndUnwrap(sppToolType.Assembly.FullName, sppToolType.FullName);

            sppTool.Main(args);

            AppDomain.Unload(appDomainCurrent);

            AppDomain.Unload(appDomain2016OL);

            AppDomain.Unload(appDomain2016OP);

            AppDomain.Unload(appDomain2013OP);

        }

    }

    public class SPPTool : MarshalByRefObject
    {
        public void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SharePointProvisioningTool());

        }

    }

}
