using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration.Ini;

namespace WebChangeAlarm.Configurators
{
    class ConfigurationFileManager
    {
        #region PROPERTIES

        public string WebsiteHomeUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public string WebsiteNodeTitle { get; set; }
        public string WebsiteNodePublishedAt { get; set; }
        public string WebsiteNodeMessageBody { get; set; }
        public string EmailAdminEmail { get; set; }
        public string EmailRecipentsSourceType { get; set; }
        public string EmailRecipentsSource { get; set; }
        public string EmailFromName { get; set; }
        public string EmailFromEmail { get; set; }
        public string SmtpClientHost { get; set; }
        public int SmtpClientPort { get; set; }
        public bool SmtpClientUseSsl { get; set; }
        public string SmtpClientUsername { get; set; }
        public string SmtpClientPassword { get; set; }

        #endregion

        #region CONSTRUCTOR

        public ConfigurationFileManager(string configFilename)
        {
            LoadConfigFile(configFilename);
        }

        #endregion

        #region METHODS


        private void LoadConfigFile(string filename)
        {
            StreamReader sr = new StreamReader(filename);

            IniConfigurationSource iniSrouce = new IniConfigurationSource();
            iniSrouce.Path = filename;

            IniConfigurationProvider iniFile = new IniConfigurationProvider(iniSrouce);
            iniFile.Load(sr.BaseStream);

            string value;
            iniFile.TryGet("Website:homeurl", out value);
            WebsiteHomeUrl = value;

            iniFile.TryGet("Website:url", out value);
            WebsiteUrl = value;

            iniFile.TryGet("Website:nodeTitle", out value);
            WebsiteNodeTitle = value;

            iniFile.TryGet("Website:nodePublishedAt", out value);
            WebsiteNodePublishedAt = value;

            iniFile.TryGet("Website:nodeMessageBody", out value);
            WebsiteNodeMessageBody = value;

            iniFile.TryGet("Email:adminEmail", out value);
            EmailAdminEmail = value;

            iniFile.TryGet("Email:recipentsSourceType", out value);
            EmailRecipentsSourceType = value;

            iniFile.TryGet("Email:recipentsSource", out value);
            EmailRecipentsSource = value;

            iniFile.TryGet("Email:fromName", out value);
            EmailFromName = value;

            iniFile.TryGet("Email:fromEmail", out value);
            EmailFromEmail = value;

            iniFile.TryGet("SmtpClient:host", out value);
            SmtpClientHost = value;

            iniFile.TryGet("SmtpClient:port", out value);
            SmtpClientPort = int.Parse(value);

            iniFile.TryGet("SmtpClient:usessl", out value);
            SmtpClientUseSsl = bool.Parse(value);

            iniFile.TryGet("SmtpClient:username", out value);
            SmtpClientUsername = value;

            iniFile.TryGet("SmtpClient:password", out value);
            SmtpClientPassword = value;


        }

        #endregion
    }
}
