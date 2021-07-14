# Social Logins With ASP.NET Core

This sample shows how to integrate with a third-party OpenID connect provider. In this example, we'll use GitHub as our auth provider, using the [ASP.NET Contrib libraries.](https://www.nuget.org/profiles/aspnet-contrib). There are other providers such as LinkedIn, OpenId (vanilla), Apple, Salesforce, and so many more.

As a **_bonus_** we'll use OctoKit to call the GitHub APIs on behalf of the user. This is likely an operation for apps that use an exclusive identity provider.

## Getting Started

Before running the application, you'll need to add secrets to your .NET User Secret Store or by adding the secrets directly to any of the `appSettings.json` files in the project folder. 

**I recommend using the secret store as there is no chance of leaking your credentials by accident.**

```console
‣ dotnet user-secrets set github:clientId "<from github>" --project SocialLogins
‣ dotnet user-secrets set github:clientSecret "<from github>" --project SocialLogins
‣ dotnet user-secrets list
   github:clientSecret = <secret>
   github:clientId = <secret>            
```

To get these values, you can create [a new OAuth App under your GitHub account](https://github.com/settings/developers).

You'll also need .NET 5 SDK installed (this sample would likely work down to SDK 3.1).

```
‣ dotnet restore
‣ dotnet run --project SocialLogins
```

## GitHub Scopes

When working with OpenID connect providers, the client must request what scopes its asking the authentication server. In the case of GitHub, we could have the ability to read a user's information, update their bio, change repos, and much more. To limit what our app can do, we need to specify the necessary scopes.

See the [GitHub scopes documentation](https://docs.github.com/en/developers/apps/building-oauth-apps/scopes-for-oauth-apps) for more information.

## Notable Code Blocks

Here are some of the more notable code blocks you may want to investigate when looking through this sample.

- [Startup](./SocialLogins/Startup.cs#L36-L69): Registering OAuth handlers 
- [Access Token](./SocialLogins/Startup.cs#L60-L68): Save Access Token to Claims
- [Signin](./SocialLogins/Pages/Signin.cshtml): Page with login buttons
- [Startup - Sign Out](./SocialLogins/Startup.cs#L103-L111): Handle Sign out process
- [Account Page](./SocialLogins/Pages/Account.cshtml): Account information from Claims and GitHub API call.

## License

The MIT License (MIT)
Copyright © 2021 Khalid Abuhakmeh

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE