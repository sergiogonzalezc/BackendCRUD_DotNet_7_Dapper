using BackendCRUD.Api.Controllers;
using BackendCRUD.Api.UnitTest.Data;
using BackendCRUD.Application.Interface;
using BackendCRUD.Application.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using System.Runtime.CompilerServices;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using BackendCRUD.Application.Model;
using Microsoft.EntityFrameworkCore;
using BackendCRUD.Api.Model;
using Moq;
using BackendCRUD.Infraestructure;
using BackendCRUD.Infraestructure.Repository;
using AutoMapper;
using MediatR;

namespace BackendCRUD.Api.UnitTest.Controller.IntegracionTest
{
    public class MemberManagementControllerTest
    {        
        private readonly IMemberApplication _membersApplication;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public MemberManagementControllerTest()
        {
            _membersApplication = A.Fake<IMemberApplication>();
         
        }

        private async Task<DBContextBackendCRUD> GetDataBaseContext()
        {
            var newOptions = new DbContextOptionsBuilder<DBContextBackendCRUD>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dataBaseContext = new DBContextBackendCRUD(newOptions);
            dataBaseContext.Database.EnsureCreated();
            // populate de database in memory
            if (await dataBaseContext.Member.CountAsync() <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    dataBaseContext.MemberType.Add(new Infraestructure.Repository.MemberTypesEF()
                    {
                        Id = "E",
                        Description = "Employee"
                    });

                    dataBaseContext.MemberType.Add(new Infraestructure.Repository.MemberTypesEF()
                    {
                        Id = "C",
                        Description = "Contract"
                    });

                    dataBaseContext.Member.Add(new Infraestructure.Repository.MemberEF()
                    {
                        Id = 1,
                        name = "peter",
                        salary_per_year = 20000,
                        type = "C",                        
                    });


                    dataBaseContext.Member.Add(new Infraestructure.Repository.MemberEF()
                    {
                        Id = 2,
                        name = "john",
                        salary_per_year = 10000,
                        type = "C",
                    });

                    await dataBaseContext.SaveChangesAsync();
                }
            }

            return dataBaseContext;

        }

        /// <summary>
        /// Integration testing
        /// </summary>
        /// <returns></returns>
        [Fact]
        private async Task GetMembers_ReturnFronDataBaseOK()
        {
            /// Arrange
            var contextDb = GetDataBaseContext();
            var name = "peter";                      

            /// Assert
            
        }
    }
}


