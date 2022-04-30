using AutoMapper;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class AutoMapperHelper
    {
        public static IMapper Mapper { get; set; }

        static AutoMapperHelper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

            Mapper = configuration.CreateMapper();
        }

        public static MapTo Map<MapTo>(this object source)
        {
            return Mapper.Map<MapTo>(source);
        }
    }
}
