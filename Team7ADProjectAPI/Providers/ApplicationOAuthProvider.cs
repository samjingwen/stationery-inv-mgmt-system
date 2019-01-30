using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Team7ADProjectApi.Entities;
using Team7ADProjectApi.Models;

namespace Team7ADProjectApi.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            //Check doa for list of valid acting department heads
            LogicDB dbContext = new LogicDB();
            var todayDate = DateTime.Now.Date;
            ApplicationDbContext appDb = new ApplicationDbContext();
            var doaList = dbContext.DelegationOfAuthority.Where(x => x.StartDate <= todayDate && x.EndDate >= todayDate).ToList();
            foreach (var doa in doaList)
            {
                if (userManager.IsInRole(doa.DelegatedTo, "Acting Department Head"))
                {
                    continue;
                }
                else
                {
                    using (var dbContextTransaction = appDb.Database.BeginTransaction())
                    {
                        try
                        {
                            if (userManager.IsInRole(doa.DelegatedTo, "Employee"))
                            {
                                userManager.AddToRole(doa.DelegatedTo, "Acting Department Head");
                                userManager.RemoveFromRole(doa.DelegatedTo, "Employee");
                            }
                            dbContextTransaction.Commit();
                        }
                        catch (Exception)
                        {
                            dbContextTransaction.Rollback();

                        }
                    }
                }
            }
            //check doa for list of invalid acting department heads
            var doaExpList = dbContext.DelegationOfAuthority.Where(x => x.StartDate > todayDate || x.EndDate < todayDate).ToList();
            foreach (var doaExp in doaExpList)
            {
                if (userManager.IsInRole(doaExp.DelegatedTo, "Acting Department Head"))
                {
                    using (var dbContextTransaction = appDb.Database.BeginTransaction())
                    {
                        try
                        {
                            userManager.AddToRole(doaExp.DelegatedTo, "Employee");
                            userManager.RemoveFromRole(doaExp.DelegatedTo, "Acting Department Head");
                            dbContextTransaction.Commit();
                        }
                        catch (Exception)
                        {
                            dbContextTransaction.Rollback();
                        }
                    }
                }
                else
                {
                    continue;
                }
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
               OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                CookieAuthenticationDefaults.AuthenticationType);

            
            System.Collections.Generic.IList<string> s= userManager.GetRoles(user.Id);
            AuthenticationProperties properties = CreateProperties(user.Id,s);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName, IList<string> s)
        {

            IDictionary<string, string> data = new Dictionary<string, string>();
            data.Add("userName", userName);
            int count = 0;
            foreach(var i in s)
            {
                string keyName = "roleName" + count;
                data.Add(keyName, i);
                count++;
            }
            return new AuthenticationProperties(data);
        }
    }
}