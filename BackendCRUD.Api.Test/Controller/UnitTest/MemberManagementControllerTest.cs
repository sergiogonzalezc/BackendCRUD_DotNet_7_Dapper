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
using MediatR;
using AutoMapper;

namespace BackendCRUD.Api.UnitTest.Controller.UnitTest
{
    public class MemberManagementControllerTest
    {
        private readonly IMemberApplication _membersApplication;        
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public MemberManagementControllerTest()
        {
            _membersApplication = A.Fake<IMemberApplication>();            
            _mediator = A.Fake<IMediator>();
            _mapper = A.Fake<Mapper>();
        }

        /// <summary>
        /// Unit testing
        /// </summary>
        /// <returns></returns>
        [Fact]
        private async Task GetMembers_ReturnOK()
        {
            /// Arrange
            var controller = new MemberManagementController(_membersApplication, _mediator, _mapper);

            /// Act
            var result = await controller.GetMembers();

            /// Assert
            result.Should().NotBeNull();
        }

        /// <summary>
        /// Unit testing
        /// </summary>
        /// <returns></returns>
        [Fact]
        private async Task GetMembers_ReturnOKV2()
        {
            var mockReview = new List<MemberModel>
            {
                new MemberModel
                {
                    Code = "1",
                    Data = null,
                    Message = "Ok",
                    DataList = new List<MemberDTO>
                    {
                       new MemberDTO
                       {
                           Id=1,
                           name="Test",
                           salary_per_year=1000,
                           type="E",
                           role=2,
                           country="Chile",
                           currencie_name="Chilean peso",
                       },
                       new MemberDTO
                       {
                           Id=2,
                           name="Test2",
                           salary_per_year=2000,
                           type="C",
                       }
                    }
                }
            };

            //var mockMembersApplication = new Mock<IMembersApplication>();
            //mockMembersApplication.Setup(x => x.GetMembers()).Returns(mockReview);

            /// Arrange
            var controller = new MemberManagementController(_membersApplication, _mediator, _mapper);

            /// Act
            var result = await controller.GetMembers();

            /// Assert
            result.Should().NotBeNull();
        }


        /// <summary>
        /// Integration testing
        /// </summary>
        /// <returns></returns>
        [Fact]
        private async Task GetMembersFromDataBase_ReturnOK()
        {
            // DbContextOptionsBuilder<AppDbContext> optionsBuilder = new();

            /// Arrange
            var controller = new MemberManagementController(_membersApplication, _mediator, _mapper);

            /// Act
            var result = await controller.GetMembers();

            /// Assert
            result.Should().NotBeNull();
        }



        [Fact]
        private async Task GetMembers_RequestMember_ReturnOK()
        {
            /// Arrange                        
            var controller = new MemberManagementController(_membersApplication, _mediator, _mapper);

            InputCreateMember input = new InputCreateMember
            {
                name = "german",
                salary_per_year = 5000,
                type = "C"
            };

            /// Act
            var result = await controller.InsertMember(input);

            /// Assert
            result.Should().NotBeNull();
        }


        [Fact]
        private async Task GetMembers_ModifyMember()
        {
            /// Arrange
            var MembersApplication = A.Fake<ICollection<MemberModel>>();
            var MembersList = A.Fake<List<MemberModel>>();

            var logger = A.Fake<ILogger<TagManagementController>>();

            var controller = new MemberManagementController(_membersApplication, _mediator, _mapper);

            InputUpdateMember input = new InputUpdateMember
            {
                Id = 1,
                name = "german",
                salary_per_year = 5000,
                type = "C"
            };

            /// Act
            //var result = await controller.ModifyMember(input);

            /// Assert
            //result.Should().NotBeNull();

        }
    }
}


