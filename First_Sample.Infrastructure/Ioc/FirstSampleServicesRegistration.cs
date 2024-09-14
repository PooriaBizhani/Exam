using First_Sample.Application.Mapping_Profile;
using First_Sample.Application.Services;
using First_Sample.Domain.InterFaces;
using First_Sample.Persistence.Context;
using First_Sample.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace First_Sample.Infrastructure.Ioc
{
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
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IProvinceRepository, ProvinceRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IAnswerRepository, AnswerRepository>();

            services.AddScoped<UserService>();
            services.AddScoped<ProvinceService>(); 
            services.AddScoped<CityService>();

            services.AddScoped<QuestionService>();
            services.AddScoped<AnswerService>();
        }
    }
}
