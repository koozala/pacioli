using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacioli.WindowsApp.NET8.Config
{
    public class UserPreferences
    {
        public string UserName { get; set; }
        public string DefaultFolder { get; set; }
        public string AttachmentOutputFolder { get; set; } 

        public UserPreferences()
        {
            UserName = Environment.UserName;
            DefaultFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            AttachmentOutputFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }
    }
}
