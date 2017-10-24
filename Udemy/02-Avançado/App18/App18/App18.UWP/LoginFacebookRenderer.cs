﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

using Newtonsoft.Json;
using Xamarin.Auth;

using App18.UWP;
using App18.Views;

[assembly: ExportRenderer(typeof(LoginFacebook), typeof(LoginFacebookRenderer))]
namespace App18.UWP
{
    public class LoginFacebookRenderer : PageRenderer
    {
        public LoginFacebookRenderer()
        {
            //Usando o OAuth(Xamarin.Auth)
            var oauth = new OAuth2Authenticator(
                clientId: "1278399355605621",
                scope: "email",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html")
            );

            oauth.Completed += async (sender, args) => {
                
                if (args.IsAuthenticated)
                {
                    //Acesso aos dados - Token de Acesso
                    var token = args.Account.Properties["access_token"].ToString();

                    var requisicao = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me?fields=name,email"), null, args.Account);
                    var resposta = await requisicao.GetResponseAsync();

                    var obj = Newtonsoft.Json.Linq.JObject.Parse(resposta.GetResponseText());

                    var Nome = obj["name"].ToString().Replace("\"", "");

                    var Email = obj["email"].ToString().Replace("\"", "");

                    App18.App.NavegarParaInicial(Nome, Email);

                }
                else
                {
                    App18.App.NavegarParaInicial("Login rejeitado");
                }
            };


            Windows.UI.Xaml.Controls.Frame rootFrame = Windows.UI.Xaml.Window.Current.Content as Windows.UI.Xaml.Controls.Frame;
            rootFrame.Navigate(oauth.GetUI(), oauth);
        }
    }
}
