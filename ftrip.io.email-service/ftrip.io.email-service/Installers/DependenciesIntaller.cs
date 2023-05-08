﻿using ftrip.io.email_service.ContactsList;
using ftrip.io.framework.Installers;
using Microsoft.Extensions.DependencyInjection;

namespace ftrip.io.email_service.Installers
{
    public class DependenciesIntaller : IInstaller
    {
        private readonly IServiceCollection _services;

        public DependenciesIntaller(IServiceCollection services)
        {
            _services = services;
        }

        public void Install()
        {
            _services.AddScoped<IContactRepository, ContactRepository>();
        }
    }
}