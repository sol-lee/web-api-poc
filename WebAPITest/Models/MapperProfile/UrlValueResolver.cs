using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using WebAPITest.Controllers;
using WebAPITest.Data.Entities;

namespace WebAPITest.Models.MapperProfile
{
    public class UrlValueResolver : IValueResolver<Camp, CampModel, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UrlValueResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// To return Url string.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="destMember"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string Resolve(Camp source, CampModel destination, string destMember, ResolutionContext context)
        {
            UrlHelper url = (UrlHelper)_httpContextAccessor.HttpContext.Items[BaseController.URLHELPER];
            return url.Link("MyIndexGet", new {index = source.Id});
        }
    }
}
