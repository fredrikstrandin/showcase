using LoanManager.Validation;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionValidationExtention
    {
        //Lägg till dom valideringarna som skall göras på lånen.
        public static IServiceCollection AddValidations(this IServiceCollection services)
        {
            services.AddSingleton<ILoanApplicationValidation, MonthlyIncomeValidation>();
            services.AddSingleton<ILoanApplicationValidation, MaxTenYearsValidation>();
            services.AddSingleton<ILoanApplicationValidation, AtLessOneMounthValidation>();

            return services;
        }
    }
}
