using Microsoft.AspNetCore.Http;
using SharedServices.BL.UseCases.Admin;
using SharedServices.DAL;
using SharedServices.DAL.Entities;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharedServices.BL.Extensions
{
    public static class ApplicationUserExtensions
    {
        public static void ManageRelationship(this ApplicationUser user, List<Domain.Service> services, Adminitrator adminitrator)
        {
            foreach (var service in services)
            {
                if (user.UserServices is null)
                    user.UserServices = new List<ApplicationUserServices>();
                
                user.UserServices.Add(new ApplicationUserServices
                {
                    User = user,
                    Service = adminitrator.GetServiceByIdWithoutConverting(service.Id)
                });
            }
        }

        public static void ManagePicture(this ApplicationUser user, IFormFile picture)
        {
            if (picture.Length < 0)
                throw new ArgumentNullException($"The submitted picture is null. {nameof(picture)}");

            byte[] bytePicture;

            using var stream = new MemoryStream();
            picture.CopyToAsync(stream);
            bytePicture = stream.ToArray();
            var pic = new Picture
            {
                ContentType = picture.ContentType,
                Image = bytePicture
            };
            user.Picture = Mapping.Mapping.Mapper.Map<Picture>(pic);
        }

        public static string PictureSource(this ApplicationUser user)
        {
            var base64 = Convert.ToBase64String(user.Picture.Image);
            var src = string.Format("data:{0};base64,{1}", user.Picture.ContentType, base64);

            return src;
        }
    }
}