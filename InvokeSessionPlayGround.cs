using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace HexaEight_Sessions_Demo
{
    partial class HexaEightUser
    {
        public void invoke_session_playground()
        {
            string lastcommand = "";
            string user = session.FetchLoggedInUser().ToString().Split('@')[0];

            while (lastcommand.ToUpper() != "LOGOUT")
            {
                CustomConsole.WriteSuccess("HESession " + user.Trim() + ">");
                lastcommand = Console.ReadLine();
                CustomConsole.WriteLine();

                try
                {
                    if (lastcommand.ToUpper() == "CLS")
                    {
                        Console.Clear();
                    }

                    if (lastcommand.ToUpper() == "WHOAMI")
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
                    }

                    if (lastcommand.ToUpper() == "PROTECT")
                    {

                        CustomConsole.WriteEmphasized("Enter the Plain Text Message To Be Protected : ");
                        var message = Console.ReadLine();
                        Console.WriteLine("");
                        try
                        {
                            var protectedmsg = session.ProtectMessage(message);

                            if (protectedmsg != "")
                            {
                                CustomConsole.WriteLineSuccess("The below message can be stored anywhere and can only be decrypted only using your logged in session");
                                Console.WriteLine("");
                                HorizontalLine.QuickDisplay();
                                Console.WriteLine(protectedmsg);
                                HorizontalLine.QuickDisplay();
                            }
                            else
                            {
                                CustomConsole.WriteLineError("Error : Encryption Failed.");
                            }
                        }
                        catch
                        {
                            CustomConsole.WriteLineError("Error : Encryption Failed.");
                        }
                    }

                    if (lastcommand.ToUpper() == "DECHIPER")
                    {

                        CustomConsole.WriteEmphasized("Paste the Encrypted Message : ");
                        var protectedmessage = Console.ReadLine();
                        Console.WriteLine("");
                        try
                        {

                            var originalmsg = session.DechipherMessage(protectedmessage);

                            if (originalmsg != "")
                            {
                                CustomConsole.WriteLineSuccess("Original Message is Below in the Body Section:");
                                Console.WriteLine("");
                                HorizontalLine.QuickDisplay();
                                Console.WriteLine(originalmsg);
                                HorizontalLine.QuickDisplay();
                            }
                            else
                            {
                                CustomConsole.WriteLineError("Error : Unable To Dechipher Encrypted Message");
                            }
                        }
                        catch
                        {
                            CustomConsole.WriteLineError("Error : Unable To Dechipher Encrypted Message");
                        }


                    }

                    if (lastcommand.ToUpper() == "ENCRYPT")
                    {

                        CustomConsole.WriteEmphasized("Enter the destination Email address Or Resource Name : ");
                        var destination = Console.ReadLine();
                        Console.WriteLine("");

                        CustomConsole.WriteEmphasized("Enter the Plain Text Message To Be Encrypted : ");
                        var message = Console.ReadLine();
                        Console.WriteLine("");
                        try
                        {
                            var encryptedmsg = session.EncryptTextMessageToDestination(message, destination);

                            if (encryptedmsg != "" && !encryptedmsg.Contains("Error"))
                            {
                                CustomConsole.WriteLineSuccess("The below message can now be shared with " + destination + " and can only be decrypted by " + destination);
                                Console.WriteLine("");
                                HorizontalLine.QuickDisplay();
                                Console.WriteLine(encryptedmsg);
                                HorizontalLine.QuickDisplay();
                            }
                            else
                            {
                                CustomConsole.WriteLineError("Error : Unable To Encrypt Message To Destination. Try Again [" + encryptedmsg + "]");
                            }
                        }
                        catch
                        {
                            CustomConsole.WriteLineError("Error : Unable To Encrypt Message To Destination. Try Again");
                        }
                    }

                    if (lastcommand.ToUpper() == "DECRYPT")
                    {

                        CustomConsole.WriteEmphasized("Paste the Encrypted Message : ");
                        var encmessage = Console.ReadLine();
                        Console.WriteLine("");
                        try
                        {

                            if (encmessage != "")
                            {
                                var decryptedmsg = session.DecryptTextMessage(encmessage);

                                CustomConsole.WriteLineSuccess("The Decrypted Message from Sender is Below :");
                                Console.WriteLine("");
                                HorizontalLine.QuickDisplay();
                                Console.WriteLine(decryptedmsg);
                                HorizontalLine.QuickDisplay();
                            }
                            else
                            {
                                CustomConsole.WriteLineError("Error : Unable To Decrypt Message. Try Again ");
                            }
                        }
                        catch
                        {
                            CustomConsole.WriteLineError("Error : Unable To Decrypt Message. Try again");
                        }
                    }

                    if (lastcommand.ToUpper() == "CHECKMSG")
                    {

                        CustomConsole.WriteEmphasized("Paste the Encrypted Message : ");
                        var encmessage = Console.ReadLine();
                        Console.WriteLine("");
                        try
                        {

                            if (encmessage != "")
                            {
                                var decryptedmsg = session.DecryptTextMessage(encmessage);
                                var userinfo = session.Retrieveuserinfo(encmessage);
                                var activesince = "";
                                var codeverifier = "";
                                dynamic messageinfo;
                                string sender = "";
                                string receiver = "";
                                string sentat = "";
                                string codechallenge = "";
                                string messagebody = "";
                                if (userinfo.Contains("USERINFO|"))
                                {
                                    activesince = userinfo.Split("|")[1];
                                    try { codechallenge = userinfo.Split("|")[2]; } catch { }
                                    messageinfo = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(decryptedmsg);
                                    sender = messageinfo["SENDER"];
                                    receiver = messageinfo["RECEIVER"];
                                    sentat = messageinfo["STIME"];
                                    messagebody = messageinfo["BODY"];
                                    try { codeverifier = messagebody.Split(":")[0].Replace("[", "").Replace("]", "").Trim(); } catch { }
                                    Console.WriteLine("");
                                    HorizontalLine.QuickDisplay();
                                    Console.WriteLine("Sender :" + sender);
                                    Console.WriteLine("Receiver :" + receiver);
                                    Console.WriteLine("Sent At :" + sentat);
                                    if (messagebody.Contains(":"))
                                    {
                                        Console.WriteLine("Message:" + messagebody.Split(":")[1]);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Message:" + messagebody.Trim());
                                    }
                                    if (String.IsNullOrEmpty(codechallenge) || String.IsNullOrEmpty(codeverifier))
                                    {
                                        Console.WriteLine("PKCE Verified ? : No (PKCE Server Side Verification Disabled)");
                                    }
                                    else
                                    {
                                        Console.WriteLine("PKCE Verified ? : " + HexaEightJose.JWT.ValidateMessage(codeverifier, codechallenge));
                                    }
                                    HorizontalLine.QuickDisplay();
                                    Console.WriteLine("");
                                }
                                else
                                {
                                    CustomConsole.WriteLineSuccess("Unable To Validate Message due to missing user information. Try again");
                                    Console.WriteLine("");
                                }

                            }
                            else
                            {
                                CustomConsole.WriteLineError("Error : Message Validation Failed!! Try again");
                            }
                        }
                        catch
                        {
                            CustomConsole.WriteLineError("Error : Message Validation Failed. Try again");
                        }
                    }

                    if (lastcommand.ToUpper() == "HELP")
                    {
                        CustomConsole.WriteLineEmphasized("Allowed Commands Are");
                        CustomConsole.WriteLineEmphasized("Encrypt - Allows you to Encrypt Plain Text Message To another User or Resource");
                        CustomConsole.WriteLineEmphasized("Decrypt - Allows you to Decrypt Encrypted Message From another User or Resource");
                        CustomConsole.WriteLineEmphasized("Protect - Self Protect a Plain Text Message (Very useful when you want to store this Data in Local Storage)");
                        CustomConsole.WriteLineEmphasized("Dechiper - Allows you to Decrypt Self Protected Message ");
                        CustomConsole.WriteLineEmphasized("CheckMsg - Validates Received Message");
                        CustomConsole.WriteLineEmphasized("Whoami - Shows Details of Logged In User");
                        CustomConsole.WriteLineEmphasized("Cls - Clear the Screen");
                        CustomConsole.WriteLineEmphasized("Logout - Logout from the Session ");
                        CustomConsole.WriteLineEmphasized("");
                    }

                    if (lastcommand.ToUpper() == "LOGOUT")
                    {
                        session.LogoutSession();
                        CustomConsole.WriteEmphasized("Logging Off Session.");
                        Console.WriteLine("");
                        session = new HexaEightJose.JWT("");
                    }
                }
                catch
                {

                }
            }

        }
    }
}
