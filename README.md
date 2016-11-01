
![](SharePointProvisioningTool/Resources/Karabina122x122.png) 
# SharePoint Provisioning Tool #

SharePoint 2013 On Premises, SharePoint 2016 On Premises and SharePoint 2016 Online Provisioning Tool front -end for the [Pnp-Sites-Core](https://github.com/OfficeDev/PnP-Sites-Core) Provisioning Framework.

## Introduction ##

The aim of this project is to make it easy for SharePoint admins to use the power of the provisioning framework in a user-friendly interface.

The tool allows you to perform 3 functions:
- Create templates from a SharePoint site
- Edit the saved templates
- Apply templates to a SharePoint site

## Operations ##
When executing the tool it opens up as a MDI application.

![](images/MainScreen.png)

The three buttons are for "Create Template", "Edit Template" and "Apply Template" in that order. On clicking any of them 
will open the Select SharePoint Version screen.

![](images/VersionSelect.png)

If you select the wrong version you will get an error during the chosen operation.

-------------

#### Create Template

To create a provisioning template from a SharePoint site, click the "Create Template" button. After selecting the SharePoint version the following screen will appear.

![](images/CreateTemplate.png)

Complete the fields as shown on the screen. The "Authentication not required" checkbox is for the On Premises versions and if the tool is run from the server and you are logged in with the farm account.

The "Options" button enables you to select what will be saved in the template. This includes the options to select what content will be saved with the template.

![](images/CreateOptions.png)

When the "Exculde base template" option is checked and the SharePoint site was created from one of the standard templates, then the saved template will not have any of the files, pages, content types or site fields matching in the base template.


After clicking the "Create" button a progress screen will appear showing the progress of the save operation.

![](images/CreateProgress.png)

----------
### Edit Template

To edit a saved template, click the "Edit Template" button. After selecting the SharePoint version the following screen will appear.

![](images/EditStart.png)

Click the "Browse..." button to select the provisioning template to edit.

![](images/OpenTemplate.png)

Select the template that you want to edit from the list or download more templates from the [SharePoint Templates Gallery](https://templates-gallery.sharepointpnp.com).

![](images/EditTemplateList.png)

To be continued...

-----------
```
Disclaimer
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS 
OR IMPLIED, INCLUDING ANY IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR 
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

