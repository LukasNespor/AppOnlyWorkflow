using System.Configuration;

namespace AppOnlyWorkflow.Models
{
    internal static class Config
    {
        public static string SiteUrl { get { return ConfigurationManager.AppSettings.Get(nameof(SiteUrl)); } }
        public static string WebRelativeListUrl { get { return ConfigurationManager.AppSettings.Get(nameof(WebRelativeListUrl)); } }
        public static string WorkflowName { get { return ConfigurationManager.AppSettings.Get(nameof(WorkflowName)); } }
        public static int ListItemId { get { return int.Parse(ConfigurationManager.AppSettings.Get(nameof(ListItemId))); } }
        public static string UserName { get { return ConfigurationManager.AppSettings.Get(nameof(UserName)); } }
        public static string Password { get { return ConfigurationManager.AppSettings.Get(nameof(Password)); } }
    }
}
