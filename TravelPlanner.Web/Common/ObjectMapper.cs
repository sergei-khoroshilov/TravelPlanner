using AutoMapper;

namespace TravelPlanner.Web.Common
{
    public class ObjectMapper
    {
        static ObjectMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<DB.User, Models.User>();
                cfg.CreateMap<Models.User, DB.User>();

                cfg.CreateMap<DB.User, Models.UserWithPassword>();
                cfg.CreateMap<Models.UserWithPassword, DB.User>();

                cfg.CreateMap<DB.Trip, Models.Trip>();
                cfg.CreateMap<Models.Trip, DB.Trip>();
            });
        }

        public DestType Map<DestType>(object source)
        {
            return Mapper.Map<DestType>(source);
        }
    }
}
