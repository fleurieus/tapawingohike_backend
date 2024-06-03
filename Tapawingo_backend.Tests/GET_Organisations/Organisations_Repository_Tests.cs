﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Repository;

namespace Tapawingo_backend.Tests.OrganisationsTests
{
    public class Organisations_Repository_Tests : TestBase, IDisposable
    {
        private readonly OrganisationsRepository _organisationsRepository;
        private readonly DataContext _context;

        public Organisations_Repository_Tests()
        {
            _context = CreateDbContext();
            _organisationsRepository = new OrganisationsRepository(_context);
        }

        //Good Weather
        [Fact]
        public void Get_Existing_Organisation_By_Id()
        {
            var organisation = _organisationsRepository.GetOrganisationById(1);

            Assert.NotNull(organisation);
            Assert.Equal(1, organisation.Id);
            Assert.Equal("TestOrganisation1", organisation.Name);
        }
        //


        //Bad Weather
        [Fact]
        public void Get_Non_Existing_Organisation_By_Id()
        {
            var organisation = _organisationsRepository.GetOrganisationById(999);

            Assert.Null(organisation);
        }
        //

        public new void Dispose()
        {
            _context.Dispose();
            base.Dispose();
        }
    }
}
