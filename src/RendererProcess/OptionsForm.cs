﻿using Bridge;
using Retyped;
using Widgetoko.Twitter;

namespace Widgetoko.RendererProcess
{
    public class OptionsForm
    {
        [Init(InitPosition.Top)]
        public static void InitGlobals()
        {
            // Init global variables (modules):
            var Electron = (electron.Electron.AllElectron)node.require.Self("electron");
            var jQuery = node.require.Self("jquery");
        }

        [Template("Electron")]
        public static electron.Electron.AllElectron Electron;

        public static void Main()
        {
            ConfigureEventHandlers();

            // Get credentials from the main process:
            var credentials = (TwitterCredentials)Electron.ipcRenderer.sendSync(Constants.IPC.GetCredentialsSync);

            // Display values on the form:
            jquery.jQuery.select("#apiKeyInput").val(credentials?.ApiKey);
            jquery.jQuery.select("#apiSecretInput").val(credentials?.ApiSecret);
            jquery.jQuery.select("#accessTokenInput").val(credentials?.AccessToken);
            jquery.jQuery.select("#accessTokenSecretInput").val(credentials?.AccessTokenSecret);
        }

        private static void ConfigureEventHandlers()
        {
            jquery.jQuery.select("#okButton").on("click", (e, args) =>
            {
                var cred = new TwitterCredentials
                {
                    ApiKey = (string) jquery.jQuery.select("#apiKeyInput").val(),
                    ApiSecret = (string) jquery.jQuery.select("#apiSecretInput").val(),
                    AccessToken = (string) jquery.jQuery.select("#accessTokenInput").val(),
                    AccessTokenSecret = (string) jquery.jQuery.select("#accessTokenSecretInput").val(),
                };

                Electron.ipcRenderer.send(Constants.IPC.SetCredentials, cred);
                Electron.remote.getCurrentWindow().close();
            });

            jquery.jQuery.select("#cancelButton").on("click", (e, args) =>
            {
                Electron.remote.getCurrentWindow().close();
            });
        }
    }
}