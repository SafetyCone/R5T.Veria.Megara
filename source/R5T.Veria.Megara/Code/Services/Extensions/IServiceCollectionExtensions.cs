using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.Bath;
using R5T.Bedford;
using R5T.Dacia;
using R5T.Megara;
using R5T.Vandalia;


namespace R5T.Veria.Megara
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="RoundTripFileSerializationVerifier{T}"/> implementation of the <see cref="IRoundTripFileSerializationVerifier{T}"/> service.
        /// </summary>
        public static IServiceCollection AddRoundTripFileSerializationVerifier<TValue>(this IServiceCollection services,
            ServiceAction<IHumanOutput> addHumanOutput,
            ServiceAction<IFileEqualityComparer> addFileEqualityComparer,
            ServiceAction<IFileSerializer<TValue>> addFileSerializer,
            ServiceAction<IValueEqualityComparer<TValue>> addValueEqualityComparer)
        {
            services
                .AddSingleton<IRoundTripFileSerializationVerifier<TValue>, RoundTripFileSerializationVerifier<TValue>>()
                .RunServiceAction(addHumanOutput)
                .RunServiceAction(addFileEqualityComparer)
                .RunServiceAction(addFileSerializer)
                .RunServiceAction(addValueEqualityComparer)
                ;

            return services;
        }

        /// <summary>
        /// Adds the <see cref="RoundTripFileSerializationVerifier{T}"/> implementation of the <see cref="IRoundTripFileSerializationVerifier{T}"/> service.
        /// </summary>
        public static ServiceAction<IRoundTripFileSerializationVerifier<TValue>> AddRoundTripFileSerializationVerifierAction<TValue>(this IServiceCollection services,
            ServiceAction<IHumanOutput> addHumanOutput,
            ServiceAction<IFileEqualityComparer> addFileEqualityComparer,
            ServiceAction<IFileSerializer<TValue>> addFileSerializer,
            ServiceAction<IValueEqualityComparer<TValue>> addValueEqualityComparer)
        {
            var serviceAction = new ServiceAction<IRoundTripFileSerializationVerifier<TValue>>(() => services.AddRoundTripFileSerializationVerifier(
                addHumanOutput,
                addFileEqualityComparer,
                addFileSerializer,
                addValueEqualityComparer));
            return serviceAction;
        }
    }
}
