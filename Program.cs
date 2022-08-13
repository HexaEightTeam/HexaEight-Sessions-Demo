using System;
using CommandLine;
using DustInTheWind.ConsoleTools;
using HexaEightJose;


namespace HexaEight_Sessions_Demo
{
    partial class HexaEightUser
    {
        private JWT session;
        
        public string clientId  { get; set; }
        public string tokenServerURL { get; set; }

        public HexaEightUser()
        {
            // Create an Instance of HexaEightJose JWT and Initialize it using a Empty String.
            session = new JWT("");
            clientId = "";
            tokenServerURL = "";
        }

    }

    class Program
    {
        public class Options
        {
            [Option('c', "createsession", Required = false, HelpText = "Create A New HexaEight Session and Save Session Token")]
            public bool createsession { get; set; }

            [Option('l', "login", Required = false, HelpText = "Login To HexaEight Session using Saved Session Token")]
            public bool sessionlogin { get; set; }

            [Option('d', "directlogin", Required = false, HelpText = "Direct Login Without Saving Session Token")]
            public bool directlogin { get; set; }

        }


        static void Main(string[] args)
        {
            var hexaeightuser = new HexaEightUser();
            hexaeightuser.clientId = "87ADEEA59F56FE1427ABC682763DF8474BADFDBC";
            hexaeightuser.tokenServerURL = "https://anyemail.chat:8443";

            try
            {
                if (args.Length > 0)
                {
                    Parser.Default.ParseArguments<Options>(args)
                       .WithParsed<Options>(o =>
                       {
                           if (o.createsession)
                           {
                               hexaeightuser.Createsession();
                           }

                           if (o.sessionlogin)
                           {
                               if (hexaeightuser.loginsession())
                               {
                                   hexaeightuser.invoke_session_playground();
                               }
                           }

                           if (o.directlogin)
                           {
                               if (hexaeightuser.directloginseession())
                               {
                                   hexaeightuser.invoke_session_playground();
                               }

                           }


                       });
                }
                else
                {
                    CustomConsole.WriteLineWarning("Missing Arguments. Use -h to view list of Options");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                CustomConsole.WriteLineWarning("Session Error!!. Use -h for Help.");
            }


        }
    }
}
