using DustInTheWind.ConsoleTools;
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
        public bool loginsession()
        {
            try
            {
                var sessiontoken = "";
                Console.WriteLine("");
                CustomConsole.WriteEmphasized("Enter your Email ID Associated With This Session : ");
                var useremail = Console.ReadLine();
                Console.WriteLine("");

                if (System.IO.File.Exists("." + Path.DirectorySeparatorChar + "tokens" + Path.DirectorySeparatorChar + JWT.Get512Hash(JWT.Get512Hash(useremail))))
                {
                    sessiontoken = System.IO.File.ReadAllText("." + Path.DirectorySeparatorChar + "tokens" + Path.DirectorySeparatorChar + JWT.Get512Hash(JWT.Get512Hash(useremail))).ToString();
                    CustomConsole.WriteEmphasized("Enter the strong password that was used to secure this Session : ");
                    var userpassword = session.ReadPassword();
                    Console.WriteLine("");

                    session.InititalizeClient(clientId, tokenServerURL, sessiontoken);
                    if (session.InitiateHexaEightSession())
                    {
                        var loginresponse = session.VerifySessionCredentials(useremail, userpassword).Result;
                        var sessionresponse = "";

                        if (loginresponse == "Ok")
                        {
                            sessionresponse = session.VerifySession().Result;
                            if (sessionresponse == "Ok")
                            {
                                var loggedinuser = session.FetchLoggedInUser();
                                var appname = session.FetchAppName();
                                var resourcename = session.FetchResourceIdentity();
                                CustomConsole.WriteLine();
                                CustomConsole.WriteLineSuccess("Logged In User : " + loggedinuser);
                                CustomConsole.WriteLine();
                                CustomConsole.WriteLineSuccess("Application Realm : " + appname);
                                CustomConsole.WriteLine();
                                CustomConsole.WriteLineSuccess("Resource : " + resourcename);
                                CustomConsole.WriteLine();
                                return true;
                            }
                            else
                            {
                                CustomConsole.WriteLineError("Error: Login Failed [ " + sessionresponse + " ]");
                                return false;
                            }
                        }

                        if (loginresponse == "Solve Captcha")
                        {
                            var newcaptcha = session.FetchNewCaptcha();
                            if (newcaptcha != "")
                            {
                                Console.WriteLine("Use the below Link to Verify to scan the QR Code using an Email token to reveal the Captcha code");
                                Console.WriteLine("");
                                Console.WriteLine(session.GetCaptchaCodeDesktopURL());
                                Console.WriteLine("");

                                Console.WriteLine("");
                                CustomConsole.WriteEmphasized("Enter the Captcha : ");
                                var usercaptcha = Console.ReadLine();
                                Console.WriteLine("");
                                Thread.Sleep(3000);

                                sessionresponse = session.VerifySession(usercaptcha).Result;
                                if (sessionresponse == "Ok")
                                {
                                    var loggedinuser = session.FetchLoggedInUser();
                                    var appname = session.FetchAppName();
                                    var resourcename = session.FetchResourceIdentity();
                                    CustomConsole.WriteLine();
                                    CustomConsole.WriteLineSuccess("Logged In User : " + loggedinuser);
                                    CustomConsole.WriteLine();
                                    CustomConsole.WriteLineSuccess("Application Realm : " + appname);
                                    CustomConsole.WriteLine();
                                    CustomConsole.WriteLineSuccess("Resource : " + resourcename);
                                    CustomConsole.WriteLine();
                                    return true;
                                }
                                else
                                {
                                    CustomConsole.WriteLineError("Error: Login Failed [ " + sessionresponse + " ]");
                                    return false;
                                }
                            }
                            else
                            {
                                CustomConsole.WriteLineError("Error: Unable To Fetch a Captcha. Try again");
                                return false;
                            }
                        }

                        CustomConsole.WriteLineError("Error: Login Failed. Try again [ " + loginresponse + " ]");
                        return false;

                    }
                    else
                    {
                        CustomConsole.WriteLineError("Error: Unable to Initialize HexaEight Session. Try again");
                        return false;
                    }
                }
                else
                {
                    CustomConsole.WriteLineError("Error: Unable to Find a Session associated with this Email Address");
                    return false;
                }
            }
            catch (Exception ex)
            {
                CustomConsole.WriteLineError("Error: We were unable to complete this process. Try again [" + ex.Message + "]");
                return false;
            }

        }

    }
}
