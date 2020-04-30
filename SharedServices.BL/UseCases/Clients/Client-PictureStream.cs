using Microsoft.AspNetCore.Http;
using SharedServices.BL.Domain;
using SharedServices.DAL;
using System;
using System.IO;

namespace SharedServices.BL.UseCases.Clients
{
    public partial class Client
    {
        public void ThreatPicture(IFormFile picture, ApplicationUser user)
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
            user.Picture = Mapping.Mapping.Mapper.Map<DAL.Entities.Picture>(pic);
        }

        public string PictureSource(ApplicationUser user)
        {
            var base64 = Convert.ToBase64String(user.Picture.Image);
            var src = string.Format("data:{0}/{1}", user.Picture.ContentType, base64);

            return src;
        }
    }
}