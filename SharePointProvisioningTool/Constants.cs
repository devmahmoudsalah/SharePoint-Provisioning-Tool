using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karabina.SharePoint.Provisioning
{
    public enum SharePointVersion
    {
        SharePoint_Invalid = 0,
        SharePoint_2013_On_Premise = 1,
        SharePoint_2016_On_Premise = 2,
        SharePoint_2016_OnLine = 3
    }

    public static class Constants
    {
        public static readonly string SharePoint_2013_On_Premise = "SharePoint 2013 On Premise";
        public static readonly string SharePoint_2016_On_Premise = "SharePoint 2016 On Premise";
        public static readonly string SharePoint_2016_Online = "SharePoint 2016 Online";
        public static readonly string FileDialogFilter = "SharePoint Provisioning Template Files (*.pnp)|*.pnp|All Files (*.*)|*.*";
    }
}
