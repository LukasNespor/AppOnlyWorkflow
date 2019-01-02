# How to reproduce the issue
1) Register application at https://tenant.sharepoint.com/_layouts/15/appregnew.aspx
2) Request permissions at https://tenant-admin.sharepoint.com/_layouts/15/appinv.aspx with **App's Permission Request XML** bellow. Don't forget to note the client id and client secret, it will be needed in the app.config.
3) Create custom list
4) Create Workflow 2010 in newly created list and add some action (eg. 'Log to History').
5) Save workflow.
6) Create item in the list.
7) Fill app.config and run this app

### App's Permission Request XML
```
<AppPermissionRequests AllowAppOnlyPolicy="true">
    <AppPermissionRequest Scope="http://sharepoint/content/tenant" Right="FullControl" />
</AppPermissionRequests>
```