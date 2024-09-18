using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;
using System.Linq;

namespace Edent.Api.Infrastructure.Filters
{
    public class FileValidationFilter : ActionFilterAttribute
    {
        private float _maxSize;
        private string _stringExtentions;
        private string[] _extenstions;

        /// <summary>
        /// File Validation Fitler
        /// </summary>
        /// <param name="maxSize"></param>
        /// <param name="extentions"></param>
        public FileValidationFilter(int maxSize, string extentions)
        {
            _maxSize = maxSize;
            _stringExtentions = extentions;
            _extenstions = !string.IsNullOrEmpty(extentions) ? extentions.Split(",") : new string[] { };
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;

            if (request.HasFormContentType)
            {
                var file = request.Form.Files[0];
                if (file != null)
                {
                    if (_extenstions.Any() && !_extenstions.Contains(Path.GetExtension(file.FileName.ToLower())))
                        context.ModelState.AddModelError("FileExtention", string.Format("Поддерживаемые форматы {0}", _stringExtentions));
                    if (file.Length > _maxSize)
                        context.ModelState.AddModelError("FileSize", string.Format("Размер файла не должен превышать {0} мб", _maxSize / (1024 * 1024)));
                }
            }
        }
    }
}
