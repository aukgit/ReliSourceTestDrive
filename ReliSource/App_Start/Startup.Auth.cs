﻿using System;
using System.Security.Claims;
using ReliSource.Models.Context;
using ReliSource.Models.POCO.Identity;
using ReliSource.Modules;
using ReliSource.Modules.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Owin;

//using ReliSource.Modules.Garbage;

namespace ReliSource {
    public partial class Startup {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app) {
            TestThingsStartup.BeforeLoadingMain();
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            #region Developers Organism Additional Settings in our Component

            AppConfig.RefreshSetting();

            #endregion

            #region Cookie Authentication

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/SignIn"),
                Provider = new CookieAuthenticationProvider {
                    OnValidateIdentity =
                        SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser, long>
                            (TimeSpan.FromMinutes(30), (manager, user) => user.GenerateUserIdentityAsync(manager),
                                id => (Int32.Parse(id.GetUserId()))
                            )
                }
            });

            #endregion

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            #region Facebook External Authentication

            if (AppConfig.Setting.IsFacebookAuthentication) {
                var facebookAuthenticationOptions = new FacebookAuthenticationOptions {
                    AppId = AppConfig.Setting.FacebookClientID.ToString(),
                    AppSecret = AppConfig.Setting.FacebookSecret,
                    AuthenticationType = "FB",
                    SignInAsAuthenticationType = "ExternalCookie",
                    Provider = new FacebookAuthenticationProvider {
                        OnAuthenticated = async ctx => {
                            ctx.Identity.AddClaim(new Claim(ProprietaryClaims.Birthday, ctx.User["birthday"].ToString()));
                            ctx.Identity.AddClaim(new Claim(ProprietaryClaims.Country, ctx.User["location"].ToString()));
                            ctx.Identity.AddClaim(new Claim(ProprietaryClaims.Gender, ctx.User["gender"].ToString()));
                            ctx.Identity.AddClaim(new Claim(ProprietaryClaims.StateOrProvince,
                                ctx.User["location"].ToString()));
                            ctx.Identity.AddClaim(new Claim(ProprietaryClaims.Email, ctx.User["email"].ToString()));
                            ctx.Identity.AddClaim(new Claim(ProprietaryClaims.FirstName,
                                ctx.User["first_name"].ToString()));
                            ctx.Identity.AddClaim(new Claim(ProprietaryClaims.LastName,
                                ctx.User["last_name"].ToString()));
                            ctx.Identity.AddClaim(new Claim(ProprietaryClaims.DisplayName, ctx.User["name"].ToString()));
                            // ctx.Identity.AddClaim(new Claim(ProprietaryClaims.RELATIONSHIP_STATUS, ctx.User["relationship_status"].ToString()));
                            // ctx.Identity.AddClaim(new Claim(ProprietaryClaims.INSPIREATIONAL_PEOPLE, ctx.User["inspirational_people"].ToString()));
                            ctx.Identity.AddClaim(new Claim(ProprietaryClaims.TimeZone, ctx.User["timezone"].ToString()));
                            ctx.Identity.AddClaim(new Claim(ProprietaryClaims.Website, ctx.User["website"].ToString()));
                            //  ctx.Identity.AddClaim(new Claim(ProprietaryClaims.PHONE, ctx.User["inspirational_people"].ToString()));
                            //  ctx.Identity.AddClaim(new Claim(ProprietaryClaims.ABOUT, ctx.User["inspirational_people"].ToString()));
                            ctx.Identity.AddClaim(new Claim(ProprietaryClaims.UserString, ctx.User.ToString()));
                        }
                    }
                };

                facebookAuthenticationOptions.Scope.Add("user_birthday");
                facebookAuthenticationOptions.Scope.Add("public_profile");
                facebookAuthenticationOptions.Scope.Add("user_friends");
                //facebookAuthenticationOptions.Scope.Add("publish_stream");
                //facebookAuthenticationOptions.Scope.Add("friends_likes");
                //facebookAuthenticationOptions.Scope.Add("user_actions.news");
                //facebookAuthenticationOptions.Scope.Add("user_actions.video");
                //facebookAuthenticationOptions.Scope.Add("user_education_history");
                //facebookAuthenticationOptions.Scope.Add("manage_pages");
                facebookAuthenticationOptions.Scope.Add("user_about_me");
                //facebookAuthenticationOptions.Scope.Add("user_likes");
                //facebookAuthenticationOptions.Scope.Add("user_friends");
                //facebookAuthenticationOptions.Scope.Add("user_interests");
                facebookAuthenticationOptions.Scope.Add("user_location");
                //facebookAuthenticationOptions.Scope.Add("user_photos");
                //facebookAuthenticationOptions.Scope.Add("user_relationships");
                //facebookAuthenticationOptions.Scope.Add("user_relationship_details");
                //facebookAuthenticationOptions.Scope.Add("user_status");
                //facebookAuthenticationOptions.Scope.Add("user_tagged_places");
                //facebookAuthenticationOptions.Scope.Add("user_videos");
                facebookAuthenticationOptions.Scope.Add("user_website");
                //facebookAuthenticationOptions.Scope.Add("read_friendlists");
                //facebookAuthenticationOptions.Scope.Add("user_mobile_phone");
                //facebookAuthenticationOptions.Scope.Add("read_stream");
                facebookAuthenticationOptions.Scope.Add("email");
                app.UseFacebookAuthentication(facebookAuthenticationOptions);
            }

            #endregion

            #region Commentout logins: Google, Twiter, Microsoft

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //}); 

            #endregion

            //ParsingNewProblems.SolveCountriesWhichDoesntHaveTimeZone();
            TestThingsStartup.AfterLoadingMain();
        }
    }
}