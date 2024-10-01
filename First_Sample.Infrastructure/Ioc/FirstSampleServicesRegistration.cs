using First_Sample.Application.InterFaces;
using First_Sample.Application.Logs;
using First_Sample.Application.Mapping_Profile;
using First_Sample.Application.Security.DynamicRole;
using First_Sample.Application.Services;
using First_Sample.Domain.InterFaces;
using First_Sample.Persistence.Context;
using First_Sample.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class FirstSampleServicesRegistration
{
    public static void ConfigureFirstSampleServices(this IServiceCollection services,
         IConfiguration configuration)
    {
        services.AddDbContext<First_Sample_Context>(options =>
        {
            options.UseSqlServer(configuration
                .GetConnectionString("FirstSampleConnectionString"));
        });

        services.AddAutoMapper(typeof(MappingProfile));

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IProvinceRepository, ProvinceRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IAnswerRepository, AnswerRepository>();
        services.AddScoped<ISiteSettingRepository, SiteSettingRepository>();
        services.AddTransient<IUtilities,Utilities>();
        services.AddScoped<IMessageSender,MessageSender>();

        //DynamicPermission IoC
        services.AddScoped<IAuthorizationHandler, DynamicRoleHandler>();
        services.AddScoped<DynamicRoleAuthorizeFilter>();

        //Logs
        services.AddScoped<LogActionFilter>();

        // Services
        services.AddScoped<ISiteSettingService,SiteSettingService>();
        services.AddScoped<IMessageSenderService, MessageSenderService>();
        services.AddScoped<UserService>();
        services.AddScoped<ProvinceService>();
        services.AddScoped<CityService>();
        services.AddScoped<QuestionService>();
        services.AddScoped<AnswerService>();


    }
}
