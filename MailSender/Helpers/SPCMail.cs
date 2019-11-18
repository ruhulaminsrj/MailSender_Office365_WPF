﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MailSender.Helpers
{
    class SPCMail
    {
        static SPCMail instance;
        public static SPCMail Instance
        {
            get
            {
                if (instance == null)
                    instance = new SPCMail();
                return instance;
            }
            set { instance = value; }
        }
        public async Task<int> PrepareSPCMail(string fromEmail, string toAddress, string subject, string body, string mailsCc, string[] filePathAttachments = null)
        {

            string[] mailAddresses = toAddress.Replace(',',';').Split(';');
            List<string> _mailsCc = new List<string>();
            if (!string.IsNullOrEmpty(mailsCc))
            {
                _mailsCc.AddRange(mailsCc.Replace(',',';').Split(';'));
            }

            int result = 0;
            await Task.Run(() =>
            {
                try
                {
                    using (System.Net.Mail.MailMessage myMail = new System.Net.Mail.MailMessage())
                    {
                        myMail.From = new MailAddress(fromEmail);
                       
                        foreach (var address in mailAddresses)
                        {                            
                            myMail.To.Add(address.Trim());
                        }

                        //prepare file attachment
                        if (filePathAttachments != null)
                        {
                            foreach (var file in filePathAttachments)
                            {
                                Attachment at = new Attachment(file);
                                myMail.Attachments.Add(at);
                            }

                        }

                        //add mails cc
                        foreach (var mCc in _mailsCc)
                        {
                            myMail.CC.Add(new MailAddress(mCc.Trim()));
                        }


                        myMail.Subject = subject;
                        myMail.IsBodyHtml = true;
                        myMail.Body = body;
                        myMail.IsBodyHtml = true;
                        using (System.Net.Mail.SmtpClient s = new System.Net.Mail.SmtpClient("mailnew.spclt.com.vn"))
                        {
                            s.DeliveryMethod = SmtpDeliveryMethod.Network;
                            s.UseDefaultCredentials = false;
                            s.Credentials = new System.Net.NetworkCredential(myMail.From.ToString(), Properties.Settings.Default._hostPassword);
                            s.EnableSsl = true;
                            //2019-11-18 add: old pass c.H "UCpYm46k"

                            s.Send(myMail);
                        }
                    }
                    result = 1;
                }
                catch (Exception ex)
                {
                    result = -1;
                }


            });
            return result;

            ////Send email

            //MailMessage mail = new MailMessage();
            //// set the addresses
            //mail.From = new MailAddress("PC_SPCF4@spclt.com.vn", "HuaVanBe");
            //// mail.[to].add("be.hua@spclt.com.vn")

            ////Get email address 
            //string EmailAdress = "";
            //EmailAdress = ms.getScalar(String.Format("Select link from {0} where sapno ='{1}'", database, CameraCode));

            //if (EmailAdress != "")
            //{
            //    mail.To.Add(EmailAdress);
            //    mail.cc.add("be.hua@spclt.com.vn,hanthanh");


            //    // set the content

            //    mail.Subject = "[AutoEmail] "
            //                + DateTime.Now.ToString("[yyyyMMddHHmmss]") + "_Chương trình Camera bị tắt khi đang khóa: " + CameraCode;
            //    string lockedOrder = "";
            //    try
            //    { //lockedOrder = dgv1.Rows[0].Cells[0].Value.ToString(); 
            //    }
            //    catch { }
            //    mail.Body = "OrderNo bị khóa: " + lockedOrder
            //                + "<br>EmpID: " + EmpID.Text
            //                + "<br>EmpName: " + EmpName.Text
            //                + "<br>User Name: " + Environment.UserName
            //                + "<br>Computer Name: " + Environment.MachineName
            //                ;
            //    mail.IsBodyHtml = true;
            //    SmtpClient smtp = new SmtpClient("mailnew.spclt.com.vn");
            //    // send the message
            //    try
            //    {
            //        smtp.Send();
            //        // MsgBox("Your Email has been sent sucessfully - Thank You")
            //    }
            //    catch (Exception exc)
            //    {
            //        // MsgBox("Send failure: " & exc.ToString())
            //        //Connection.SaveError(10, "DeletePO", "DeletePO", "~~~");
            //    }
            //}
        }
    }
}
