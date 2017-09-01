# Dual Graph Group Membership Sample

Attempting to retreive all users in a VSTS group is no easy task. VSTS groups can nest users, Azure Active Directory groups, and other VSTS groups. In order to get all users, the new VSTS Graph API public preview and the Microsoft Graph API must be used in tandem to return all results. The below sample demonstrates how to do this.

## Sample Application

This buildable sample will walk you through the steps to create a console application which uses ADAL to authenticate a user and display a full list of all VSTS and AAD usernames within a VSTS group.

To run this sample you will need:
* [Visual Studio IDE](https://www.visualstudio.com/vs/)
* An Azure Active Directory (AAD) tenant. If you do not have one, follow these [steps to set up an AAD](https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-howto-tenant)
* A user account in your AAD tenant
* A VSTS account backed by your AAD tenant where your user account has access. If you have an existing VSTS account not connected to your AAD tenant follow these [steps to connect your AAD tenant to your VSTS account](https://www.visualstudio.com/en-us/docs/setup-admin/team-services/manage-organization-access-for-your-account-vs)

## Step 1: Clone or download vsts-auth-samples repository

From a shell or command line: 
```no-highlight
git clone https://github.com/Microsoft/vsts-auth-samples.git
```

## Step 2: Register the sample application with you Azure Active Directory tenant

1. Sign in to the [Azure Portal](https://portal.azure.com).
2. On the top bar, click on your account and under the Directory list, choose the Active Directory tenant where you wish to register your application.
3. On the left hand navigation menu, select `Azure Active Directory`.
4. Click on `App registrations` and select `New application registration` from the top bar.
5. Enter a `name` for you application, ex. "Adal native app sample", choose `Native` for `application type`, and enter `http://adalsample` for the `Redirect URI`. Finally click `create` at the bottom of the screen.
6. Save the `Application ID` from your new application registration. You will need it later in this sample.
7. Grant permissions for VSTS. Click `Required permissions` -> `add` -> `1 Select an API` -> type in and select `Microsoft Visual Studio Team Services` -> check the box for `Delegated Permissions` -> click `Select` -> click `Done` -> click `Grant Permissions` -> click `Yes`.
8. Grant permissions for Microsoft Graph API. Click `Required permissions` -> `Add` -> `1 Select an API` -> type in and select `Microsoft Graph` -> check the box for `Delegated Permissions` -> click `Select` -> click `Done` -> click `Grant Permissions` -> click `Yes`.

## Step 3: Install and configure ADAL (optional)

Packages: `Microsoft.Identity.Model.Clients.ActiveDirectory` and `Microsoft.VisualStudio.Services.Client` (Public Preview) has already been installed and configured in the sample, but if you are adding to your own project you will need to [install and configure it yourself](https://www.nuget.org/packages/Microsoft.IdentityModel.Clients.ActiveDirectory).

## Step 4: Run the sample

1. Navigate to the sample in cloned repo `vsts-auth-samples/DualGraphGroupMembershipSample/`.
2. Open the solution file `DualGraphGroupMembershipSample.sln` in [Visual Studio 2017](https://www.visualstudio.com/downloads/).
3. Use [Nuget package restore](https://docs.microsoft.com/en-us/nuget/consume-packages/package-restore) to ensure you have all dependencies installed.
4. Open CS file `Program.cs` and there is a section with input values to change at the top of the class:
    * `vstsCollectionUrl` - update this with the url to your VSTS collection, e.g. http://myaccount.visualstudio.com.
    * `clientId` - update this with the `application id` you saved from step 2.6 above.
    * `replyUri` - update this to `http://adalsample`, you can also add other reply urls in [azure portal](https://portal.azure.com)
    * `groupDisplayName` - update this with the display name of the group you would like to get all user names from.
5. Build and run the solution. After running you should see an interactive login prompt. Then after authentication and authorization, a list of all usernames from the selected VSTS group.