using AppOnlyWorkflow.Models;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WorkflowServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace AppOnlyWorkflow
{
    class Program
    {
        static void Main(string[] args)
        {
            StartWFAppOnly();
            StartWFUser();
        }

        /// <summary>
        /// Running this method results in exception written to the console:
        /// Access denied. You do not have permission to perform this action or access this resource.
        /// </summary>
        static void StartWFAppOnly()
        {
            using (ClientContext cc = GetClientContextForApp(Config.SiteUrl))
            {
                ListItem item = GetItem(cc.Web, Config.WebRelativeListUrl, Config.ListItemId);
                StartWorkflowOnItem(cc, item, Config.WorkflowName);
            }
        }

        static void StartWFUser()
        {
            using (ClientContext cc = new ClientContext(Config.SiteUrl))
            using (SecureString pwd = new SecureString())
            {
                Config.Password.ToList().ForEach(c => pwd.AppendChar(c));
                cc.Credentials = new SharePointOnlineCredentials(Config.UserName, pwd);

                ListItem item = GetItem(cc.Web, Config.WebRelativeListUrl, Config.ListItemId);
                StartWorkflowOnItem(cc, item, Config.WorkflowName);
            }
        }

        static ClientContext GetClientContextForApp(string siteUrl)
        {
            Uri url = new Uri(siteUrl);
            string realm = TokenHelper.GetRealmFromTargetUrl(url);
            string accessToken = TokenHelper.GetAppOnlyAccessToken(TokenHelper.SharePointPrincipal, url.Authority, realm).AccessToken;

            return TokenHelper.GetClientContextWithAccessToken(url.ToString(), accessToken);
        }

        static ListItem GetItem(Web web, string webRelativeListUrl, int itemId)
        {
            ClientContext cc = web.Context as ClientContext;
            cc.Load(web, x => x.ServerRelativeUrl);
            cc.ExecuteQuery();

            List list = web.GetList($"{web.ServerRelativeUrl}/{webRelativeListUrl}");
            ListItem item = list.GetItemById(itemId);
            cc.Load(item, x => x.Id, x => x["GUID"], x => x.ParentList.Id);
            cc.ExecuteQuery();

            return item;
        }

        static void StartWorkflowOnItem(ClientContext cc, ListItem item, string workflowName)
        {
            try
            {
                WorkflowServicesManager manager = new WorkflowServicesManager(cc, cc.Web);
                InteropService service = manager.GetWorkflowInteropService();
                cc.Load(service);
                cc.ExecuteQuery();

                var initiationData = new Dictionary<string, object>();
                Guid itemGuid = new Guid(item["GUID"].ToString());
                service.StartWorkflow(workflowName, new Guid(), item.ParentList.Id, itemGuid, initiationData);
                cc.ExecuteQuery();
            }
            catch (ServerException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }
    }
}
