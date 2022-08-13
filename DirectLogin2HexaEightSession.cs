using DustInTheWind.ConsoleTools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace HexaEight_Sessions_Demo
{
    partial class HexaEightUser
    {
        public bool directloginseession()
        {
            try
            {
                Console.WriteLine("");
                CustomConsole.WriteEmphasized("Enter your Email ID  ");
                var useremail = Console.ReadLine();
                Console.WriteLine("");

                session.InititalizeClient(clientId, tokenServerURL, "");
                if (session.InitiateHexaEightSession_Direct(useremail).Result)
                {
                    HexaEightJose.JWT.DisplayQRCodeImage("Scan the Below QRCode using HexaEight Authenticator Mobile App to Authorize This Request", session.GetRawQRCode());
                    Console.WriteLine("");
                    Console.WriteLine("If you are having Trouble scanning the the above QR Code, alternatively you can paste the below URL in any browser and scan the QR Code using any HexaEight Resource Identity Token associated with your Email Vault To Complete This Authentication process");
                    Console.WriteLine("");
                    Console.WriteLine(session.GetRawQRCode_DesktopURL());
                    Console.WriteLine("");
                    Console.WriteLine("Press Any Key Once This QR Code Has Been Authorized Using HexaEight Resource Identity Token.");
                    Console.ReadKey();
                    Thread.Sleep(8000);

                    var directsessionresp = session.CompleteLoginProcess_Direct().Result;
                    if (directsessionresp == "Ok")
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

                        if (directsessionresp == "Solve Captcha")
                        {
                            var newcaptcha = session.GetCaptchaCode();
                            Console.WriteLine("Use the below Link to Verify to scan the QR Code using an Email token to reveal the Captcha code");
                            Console.WriteLine("");
                            Console.WriteLine(session.GetCaptchaCodeDesktopURL());
                            Console.WriteLine("");

                            if (newcaptcha !=  "")
                            {
                                var sessionresponse = "";
                                var logincnt = 0;
                                while (sessionresponse != "Ok" || logincnt < 3)
                                {

                                    Console.WriteLine("");
                                    CustomConsole.WriteEmphasized("Enter the Captcha : ");
                                    var usercaptcha = Console.ReadLine();
                                    Console.WriteLine("");
                                    Thread.Sleep(3000);

                                    sessionresponse = session.CompleteLoginProcess_Direct(usercaptcha).Result;
                                    if (sessionresponse == "Ok")
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        CustomConsole.WriteLineError("Error: Invalid Captcha. [ " + sessionresponse + " ] Try again!!");
                                    }
                                    logincnt++;
                                }
                                if (session.FetchLoggedInUser() == "")
                                {
                                    CustomConsole.WriteLineError("Error: Login Failed [ " + sessionresponse + " ]");
                                    return false;
                                }
                                else
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

                            }
                            else
                            {
                                CustomConsole.WriteLineError("Error: Unable To Fetch a Captcha. Try again");
                                return false;
                            }
                        }
                        else
                        {
                            CustomConsole.WriteLineError("Error: Login Failed [ " + directsessionresp + " ]");
                            return false;
                        }
                    }

                }
                else
                {
                    CustomConsole.WriteLineError("Error: Initialization Failed!! Try again");
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
