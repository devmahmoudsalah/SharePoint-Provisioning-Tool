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
        private const string NAME_SharePoint2013OnPrem = "SharePoint2013OnPrem";
        private const string NAME_SharePoint2016OnPrem = "SharePoint2016OnPrem";
        private const string NAME_SharePoint2016Online = "SharePoint2016Online";

        public static AppDomain appDomain2013OP = null;
        public static AppDomain appDomain2016OP = null;
        public static AppDomain appDomain2016OL = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SharePointProvisioningTool());

            if (appDomain2013OP != null)
            {
                AppDomain.Unload(appDomain2013OP);
                appDomain2013OP = null;

            }

            if (appDomain2016OP != null)
            {
                AppDomain.Unload(appDomain2016OP);
                appDomain2016OP = null;

            }

            if (appDomain2016OL != null)
            {
                AppDomain.Unload(appDomain2016OL);
                appDomain2016OL = null;

            }

        }

        public static SPLoader LoadSPLoader(SharePointVersion version)
        {
            SPLoader loader = null;

            switch (version)
            {
                case SharePointVersion.SharePoint_2013_On_Premises:

                    AppDomainSetup setup2013OP = new AppDomainSetup()
                    {
                        ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                        ApplicationName = NAME_SharePoint2013OnPrem,
                        PrivateBinPath = $"{NAME_SharePoint2013OnPrem};",
                        DisallowApplicationBaseProbing = false,
                        DisallowBindingRedirects = false

                    };

                    appDomain2013OP = AppDomain.CreateDomain(Constants.SharePoint_2013_On_Premises, null, setup2013OP);

                    appDomain2013OP.Load(typeof(SPLoader).Assembly.FullName);

                    loader = (SPLoader)Activator.CreateInstance(appDomain2013OP,
                                                                typeof(SPLoader).Assembly.FullName,
                                                                typeof(SPLoader).FullName,
                                                                false,
                                                                BindingFlags.Public | BindingFlags.Instance,
                                                                null, null, null, null).Unwrap();


                    loader.LoadProvisioningAssembly(setup2013OP.ApplicationName);

                    break;

                case SharePointVersion.SharePoint_2016_On_Premises:

                    AppDomainSetup setup2016OP = new AppDomainSetup()
                    {
                        ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                        ApplicationName = NAME_SharePoint2016OnPrem,
                        PrivateBinPath = $"{NAME_SharePoint2016OnPrem};",
                        DisallowApplicationBaseProbing = false,
                        DisallowBindingRedirects = false

                    };

                    appDomain2016OP = AppDomain.CreateDomain(Constants.SharePoint_2016_On_Premises, null, setup2016OP);

                    appDomain2016OP.Load(typeof(SPLoader).Assembly.FullName);

                    loader = (SPLoader)Activator.CreateInstance(appDomain2016OP,
                                                                typeof(SPLoader).Assembly.FullName,
                                                                typeof(SPLoader).FullName,
                                                                false,
                                                                BindingFlags.Public | BindingFlags.Instance,
                                                                null, null, null, null).Unwrap();


                    loader.LoadProvisioningAssembly(setup2016OP.ApplicationName);

                    break;

                case SharePointVersion.SharePoint_2016_OnLine:

                    AppDomainSetup setup2016OL = new AppDomainSetup()
                    {
                        ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                        ApplicationName = NAME_SharePoint2016Online,
                        PrivateBinPath = $"{NAME_SharePoint2016Online};",
                        DisallowApplicationBaseProbing = false,
                        DisallowBindingRedirects = false

                    };

                    appDomain2016OL = AppDomain.CreateDomain(Constants.SharePoint_2016_Online, null, setup2016OL);

                    appDomain2016OL.Load(typeof(SPLoader).Assembly.FullName);

                    loader = (SPLoader)Activator.CreateInstance(appDomain2016OL,
                                                                typeof(SPLoader).Assembly.FullName,
                                                                typeof(SPLoader).FullName,
                                                                false,
                                                                BindingFlags.Public | BindingFlags.Instance,
                                                                null, null, null, null).Unwrap();


                    loader.LoadProvisioningAssembly(setup2016OL.ApplicationName);

                    break;

                default:

                    break;

            } //switch

            return loader;

        } //LoadSPLoader

    } //Program

}
