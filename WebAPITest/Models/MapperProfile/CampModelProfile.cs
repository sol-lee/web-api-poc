using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPITest.Data.Entities;

namespace WebAPITest.Models.MapperProfile
{
    public class CampModelProfile : Profile
    {
        public CampModelProfile()
        {
            CreateMap<Camp, CampModel>()
                .ForMember(dest => dest.StartDate, option => option.MapFrom(src => src.EventDate))
                .ForMember(dest => dest.EndDate,
                    option => option.ResolveUsing(src => src.EventDate.AddDays(src.Length > 0 ? src.Length - 1 : 0)))
                //.ForMember(dest => dest.Url, option => option.ResolveUsing(
                //    (src, dest, unused, rCtx) =>
                //    {
                //        IUrlHelper url = (IUrlHelper)rCtx.Items["UrlHelper"];
                //        return url.Link("MyIndexGet", new { index = src.Id});
                //    }));
                .ForMember(dest => dest.Url, option => option.ResolveUsing<UrlValueResolver>())

                .ReverseMap()
                .ForMember(dest => dest.Length,
                    option => option.ResolveUsing(src => (src.EndDate - src.StartDate).Days + 1))
                .ForMember(dst => dst.Location, option => option.ResolveUsing(src =>
                {
                    Location l = new Location();
                    l.Address1 = src.LocationAddress1;
                    l.Country = src.LocationCountry;
                    return l;
                }));

        }
    }
}
