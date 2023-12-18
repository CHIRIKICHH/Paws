using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Paws
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjkwNjQ5OEAzMjMzMmUzMDJlMzBWb2Z3NEl0SkZ3V0RlVGZHTU95dHNzTkNwcEw4TytrM0dVRkNjMmxuY2Q4PQ==");
        }
    }
}
