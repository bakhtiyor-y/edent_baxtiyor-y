using AutoMapper;
using Edent.Api.Helpers;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class ToothService : EntityService<Tooth>, IToothService
    {
        public ToothService(IRepository<Tooth> repository, IMapper mapper)
            : base(repository, mapper)
        {

        }

        public string ChangeImage(IFormFile _file, int toothId)
        {
            //var pathToSave = MediaUrls.GetSharedImageDirectory();
            //if (!Directory.Exists(pathToSave))
            //{
            //    Directory.CreateDirectory(pathToSave);
            //}

            //if (_file.Length > 0)
            //{
            //    var fileName = ContentDispositionHeaderValue.Parse(_file.ContentDisposition).FileName.Trim('"');
            //    var fileGeneratedName = Guid.NewGuid().ToString("D") + Path.GetExtension(fileName);
            //    var fullPath = MediaUrls.GetSharedImageUrl(fileGeneratedName);
            //    using (var stream = new FileStream(fullPath, FileMode.Create))
            //    {
            //        _file.CopyTo(stream);
            //        var tooth = Query().FirstOrDefault(f => f.Id == toothId);
            //        if (tooth != null)
            //        {
            //            var oldImage = Path.Combine(pathToSave, tooth.Image ?? string.Empty);
            //            if (oldImage != "tooth.png" && File.Exists(oldImage))
            //                File.Delete(oldImage);
            //            tooth.Image = fileGeneratedName;
            //            Repository.Edit(tooth);
            //            if (Repository.SaveChanges())
            //            {
            //                return fileGeneratedName;
            //            }
            //            else
            //            {
            //                return tooth.Image;
            //            }
            //        }
            //    }
            //}
            return null;
        }

    }
}
