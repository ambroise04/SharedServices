using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using Microsoft.AspNetCore.Http;
using SharedServices.BL.UseCases.Admin;
using SharedServices.DAL;
using SharedServices.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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

        private static string GenerateSource(byte[] image, string mineType)
        {
            var base64 = Convert.ToBase64String(image);
            var src = string.Format("data:{0};base64,{1}", mineType, base64);

            return src;
        }

        public static string ResizePicture(this ApplicationUser user, int width, int height)
        {
            // Read a file and resize it.
            byte[] photoBytes = user.Picture.Image;
            Color color = Color.FromArgb(92, 106, 119);
            Size size = new Size(width, height);


            using MemoryStream inStream = new MemoryStream(photoBytes);
            using MemoryStream outStream = new MemoryStream();
            using (ImageFactory imageFactory = new ImageFactory())
            {
                // Load, resize, set the format and quality and save an image.
                imageFactory.Load(inStream)
                            .Resize(size)
                            .BackgroundColor(color)
                            .Save(outStream);
            }
            return GenerateSource(outStream.ToArray(), user.Picture.ContentType);
        }
    }
}