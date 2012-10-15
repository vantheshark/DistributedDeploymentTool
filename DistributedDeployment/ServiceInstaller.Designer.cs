using System;
using System.Text;

namespace DistributedDeployment
{
    partial class ServiceInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Try to parse the arg from InstallUtils arguments and apply them to the service assemblypath
        /// 
        /// To Install:
        /// C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe /listen=5555 /token=YourS3cur!tyTok3n /i DD.exe
        /// 
        /// To Uninstall:
        /// C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe /listen=5555 /token=YourS3cur!tyTok3n /u DD.exe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeAssemblyPath(object sender, System.Configuration.Install.InstallEventArgs e)
        {
            var port = this.Context.Parameters["listen"];
            var token = this.Context.Parameters["token"];
            if (!String.IsNullOrEmpty(port) && !String.IsNullOrEmpty(token))
            {
                var path = "\"" + Context.Parameters["assemblypath"] + "\" --listen " + port + " --token " + token;
                Console.WriteLine("New Path: {0}", path);
                Context.Parameters["assemblypath"] = path;
            }
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.serviceProcessInstaller1.Password = null;
            this.serviceProcessInstaller1.Username = null;
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.Description = "This service listens on a specific port for the remote commands from client";
            this.serviceInstaller1.DisplayName = "Distributed Deployment Service";
            this.serviceInstaller1.ServiceName = "DistributedDeployment";
            this.serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // Installer
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller1,
            this.serviceInstaller1});

            this.BeforeInstall += new System.Configuration.Install.InstallEventHandler(ChangeAssemblyPath);
            this.BeforeUninstall += new System.Configuration.Install.InstallEventHandler(ChangeAssemblyPath);
        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller serviceInstaller1;
    }
}