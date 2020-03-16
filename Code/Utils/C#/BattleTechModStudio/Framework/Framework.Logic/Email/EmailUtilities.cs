﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.Email;
using AutoMapper;
using Framework.Domain.Email.Models;
using Framework.Utils.Extensions.MailMessage;

namespace Framework.Logic.Email
{
    using AsposeAttachment = Attachment;
    using AsposeAttachmentCollection = AttachmentCollection;
    using AsposeMailAddress = MailAddress;
    using AsposeMailAddressCollection = MailAddressCollection;
    using AsposeMailMessage = MailMessage;

    public static class EmailUtilities
    {
        static EmailUtilities()
        {
            Mapper.CreateMap<string, System.Net.Mail.MailAddress>()
                .ConvertUsing(emailAddress =>
                    null == emailAddress ? null : new System.Net.Mail.MailAddress(emailAddress));

            Mapper.CreateMap<List<string>, System.Net.Mail.MailAddressCollection>().ConvertUsing(
                emailAddressList =>
                {
                    var mailAddressCollection = new System.Net.Mail.MailAddressCollection();
                    if (null != emailAddressList &&
                        emailAddressList.Any())
                    {
                        mailAddressCollection.Add(string.Join(",", emailAddressList));
                    }

                    return mailAddressCollection;
                });

            Mapper.CreateMap<Domain.Email.Models.Email, System.Net.Mail.MailMessage>()
                .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body))
                .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.FromAddress))
                .ForMember(dest => dest.IsBodyHtml, opt => opt.MapFrom(src => src.IsContentHtml))
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
                .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.ToAddresses))
                .ForMember(dest => dest.Bcc, opt => opt.MapFrom(src => src.BCCAddresses))
                .ForMember(dest => dest.CC, opt => opt.MapFrom(src => src.CCAddresses));

            // Aspose...
            Mapper.CreateMap<AsposeMailAddress, string>()
                .ConvertUsing(emailAddress => null == emailAddress ? string.Empty : emailAddress.Address);

            Mapper.CreateMap<AsposeMailAddressCollection, List<string>>().ConvertUsing(
                mailAddressCollection =>
                {
                    var emailAddressList = new List<string>();
                    if (null != mailAddressCollection &&
                        mailAddressCollection.Any())
                    {
                        emailAddressList.AddRange(mailAddressCollection.Select(Mapper.Map<string>));
                    }

                    return emailAddressList;
                });

            Mapper.CreateMap<AsposeMailMessage, Domain.Email.Models.Email>()
                .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body))
                .ForMember(dest => dest.FromAddress, opt => opt.MapFrom(src => src.From))
                .ForMember(dest => dest.IsContentHtml, opt => opt.MapFrom(src => src.IsBodyHtml))
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
                .ForMember(dest => dest.ToAddresses, opt => opt.MapFrom(src => src.To))
                .ForMember(dest => dest.BCCAddresses, opt => opt.MapFrom(src => src.Bcc))
                .ForMember(dest => dest.CCAddresses, opt => opt.MapFrom(src => src.CC))
                .ForMember(dest => dest.DateReceived, opt => opt.MapFrom(src => src.Date));
        }

        public static byte[] GetEmailBytes(Domain.Email.Models.Email email)
        {
            var mailMessage = Mapper.Map<System.Net.Mail.MailMessage>(email);
            foreach (var emailFileAttachment in email.FileAttachments)
            {
                mailMessage.Attachments.Add(
                    new System.Net.Mail.Attachment(
                        new MemoryStream(emailFileAttachment.Content),
                        emailFileAttachment.Name));
            }

            return mailMessage.GetEmailContentBytes();
        }

        public static Domain.Email.Models.Email LoadFromDisk(string filePath)
        {
            //// var mailMessage = AsposeMailMessage.Load(filePath, MailMessageLoadOptions.DefaultEml);
            var mailMessage = AsposeMailMessage.Load(filePath, new EmlLoadOptions());
            var email = Mapper.Map<Domain.Email.Models.Email>(mailMessage);
            foreach (var att in mailMessage.Attachments)
            {
                byte[] bytes = null;
                using (var ms = new MemoryStream())
                {
                    att.ContentStream.CopyTo(ms);
                    bytes = ms.ToArray();
                }

                email.FileAttachments = email.FileAttachments ?? new List<EmailFileAttachment>();
                email.FileAttachments.Add(new EmailFileAttachment
                    {Email = email, Name = att.Name, Length = (int) att.ContentStream.Length, Content = bytes});
            }

            return email;
        }
    }
}