namespace Web;

public static class MapperDi
{
    public static void AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Business.mapper.AccountMapperProfile).Assembly);
        services.AddAutoMapper(typeof(Business.mapper.ProductMapperProfile).Assembly);
        services.AddAutoMapper(typeof(Business.mapper.CategoryMapperProfile).Assembly);
        services.AddAutoMapper(typeof(Business.mapper.AutoMapperProfile).Assembly);

    }
}
