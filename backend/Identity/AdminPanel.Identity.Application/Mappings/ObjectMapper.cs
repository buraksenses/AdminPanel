using AutoMapper;

namespace AdminPanel.Identity.Application.Mappings;

public class ObjectMapper
{
    private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfiles>();
        });
        return config.CreateMapper();
    });

    public static IMapper Mapper => lazy.Value;
}