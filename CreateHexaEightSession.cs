using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.Controls;
using HexaEightJose;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace HexaEight_Sessions_Demo
{
    partial class HexaEightUser
    {
        public void  Createsession()
        {
            try
            {
                Console.Clear();
                CustomConsole.WriteLine();
                CustomConsole.WriteLine(HorizontalAlignment.Center, "Create New HexaEight Session");
                CustomConsole.WriteLine();
                HorizontalLine.QuickDisplay();

                CustomConsole.WriteEmphasized("Enter your Email ID To Get Started : ");
                var useremail = Console.ReadLine();
                Console.WriteLine("");

                if (useremail.Trim() != "")
                {
                    CustomConsole.WriteEmphasized("Enter a strong remember-able password to secure this Session :");
                    var clientpassword = session.ReadPassword();
                    Console.WriteLine("");

                    CustomConsole.WriteEmphasized("Re-Enter your password For Verification : ");
                    var rclientpassword = session.ReadPassword();
                    Console.WriteLine("");

                    if (clientpassword.Trim() == rclientpassword.Trim())
                    {
                        if (clientpassword.Length >= 15)
                        {
                            //Initialize the Client Program by passing the Client ID and URL of the Authentication Server
                            session.InititalizeClient(clientId, tokenServerURL, "");

                            //Inititalize a new HexaEight Session
                            if (session.InitiateHexaEightSession())
                            {
                                //Use the useremail and password to fetch a QR Code that can be used to authenticate the user
                                var qrcoderesp = session.FetchLoginQRCode(useremail, clientpassword).Result;

                                //Display the QR Code to the user - This uses the console to display a QR Code
                                session.DisplayLoginQRCode(qrcoderesp);
                                Thread.Sleep(8000);

                                //Once the user authenticates using HexaEight Authenticator, fetch the user JWT token 
                                var jwtsession = session.FetchJWTToken(qrcoderesp, useremail, clientpassword).Result;

                                //Verify and Save the jwttoken so that it can be used by the user to login to Client Program anytime using the emailid and password
                                session.InititalizeClient(clientId, tokenServerURL, jwtsession);
                                var resourceidentity = session.FetchResourceIdentity();

                                if (resourceidentity != "")
                                {
                                    try
                                    {
                                        System.IO.Directory.CreateDirectory("." + Path.DirectorySeparatorChar + "tokens");
                                    }
                                    finally
                                    {
                                        CustomConsole.WriteLine();
                                        Console.WriteLine("Saving Session Token... ");
                                        CustomConsole.WriteLine();
                                    }


                                    if (System.IO.File.Exists("." + Path.DirectorySeparatorChar + "tokens" + Path.DirectorySeparatorChar + JWT.Get512Hash(JWT.Get512Hash(useremail))))
                                    {
                                        System.IO.File.Delete("." + Path.DirectorySeparatorChar + "tokens" + Path.DirectorySeparatorChar + JWT.Get512Hash(JWT.Get512Hash(useremail)));
                                    }
                                    var file = System.IO.File.CreateText("." + Path.DirectorySeparatorChar + "tokens" + Path.DirectorySeparatorChar + JWT.Get512Hash(JWT.Get512Hash(useremail)));
                                    file.WriteLine(jwtsession);
                                    file.Flush();
                                    file.Close();

                                    CustomConsole.WriteLineSuccess("Success : Session Created Successfully. The JWT session token is displayed below.");
                                    CustomConsole.WriteLine();
                                    CustomConsole.WriteLineSuccess("Paste the below token in a browser to view its contents...");
                                    CustomConsole.WriteLine();
                                    HorizontalLine.QuickDisplay();
                                    CustomConsole.WriteLineSuccess("https://jwt.io/#debugger-io?token=" + jwtsession);
                                    HorizontalLine.QuickDisplay();
                                }
                                else
                                {
                                    CustomConsole.WriteLineError("Error : Unable To Create a New Session. Try again");
                                    CustomConsole.WriteLine();
                                }
                            }
                            else
                            {
                                CustomConsole.WriteLineError("Error : Session Initialization Error. Try again");
                                CustomConsole.WriteLine();
                            }
                        }
                        else
                        {
                            CustomConsole.WriteLineError("Error : Password Length Insufficient. Minimum 15 Characters");
                            CustomConsole.WriteLine();

                        }
                    }

                    else
                    {
                        CustomConsole.WriteLineError("Error : Passwords dont match!! Try again.");
                        CustomConsole.WriteLine();
                    }
                }
                else
                {
                    CustomConsole.WriteLineError("Error : Email Addresses dont match!! Try again.");
                    CustomConsole.WriteLine();
                }
            }
            catch (Exception ex)
            {
                CustomConsole.WriteLineError("Error : Unable to create Session. Try again [" + ex.Message + "]");
                CustomConsole.WriteLine();
            }


        }
    }
}
