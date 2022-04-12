using LoanManager.Validation;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionValidationExtention
    {
        //Lägg till dom valideringarna som skall göras på lånen.
        public static IServiceCollection AddValidations(this IServiceCollection services)
        {
            services.AddScoped<ILoanApplicationValidation, MonthlyIncomeValidation>();
            services.AddScoped<ILoanApplicationValidation, MaxTenYearsValidation>();
            services.AddScoped<ILoanApplicationValidation, AtLessOneMounthValidation>();

            return services;
        }
    }
}
