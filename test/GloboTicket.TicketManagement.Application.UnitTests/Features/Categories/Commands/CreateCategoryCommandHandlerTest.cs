using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Features.Categories.Commands.CreateCategory;
using GloboTicket.TicketManagement.Application.Profiles;
using GloboTicket.TicketManagement.Application.UnitTests.Mocks;
using Moq;
using Shouldly;
using Xunit;

namespace GloboTicket.TicketManagement.Application.UnitTests.Features.Categories.Commands
{
    public class CreateCategoryCommandHandlerTest
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandlerTest()
        {
            _mockCategoryRepository = RepositoryMocks.GetCategoryRepository();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task CreateCategoryAddsValidCategory()
        {
            _mockCategoryRepository.Setup(repo => repo.IsCategoryNameUnique(It.IsAny<string>())).ReturnsAsync(true);

            var handler = new CreateCategoryCommandHandler(_mapper, _mockCategoryRepository.Object);

            var result = await handler.Handle(new CreateCategoryCommand() {Name = "test"}, CancellationToken.None);

            result.Success.ShouldBe(true);
            result.Category.Name.ShouldBe("test");
            result.ValidationErrors.ShouldBe(null);
        }

        [Fact]
        public async Task CreateCategoryFailsToAddExistingCategory()
        {
            _mockCategoryRepository.Setup(repo => repo.IsCategoryNameUnique(It.IsAny<string>())).ReturnsAsync(false);

            var handler = new CreateCategoryCommandHandler(_mapper, _mockCategoryRepository.Object);

            var result = await handler.Handle(new CreateCategoryCommand() { Name = "test" }, CancellationToken.None);

            result.Success.ShouldBe(false);
            result.Category.ShouldBe(null);
            result.ValidationErrors.Count.ShouldBe(1);
        }

        [Fact]
        public async Task CreateCategoryFailsToAddCategoryWithTooLongName()
        {
            _mockCategoryRepository.Setup(repo => repo.IsCategoryNameUnique(It.IsAny<string>())).ReturnsAsync(true);

            var handler = new CreateCategoryCommandHandler(_mapper, _mockCategoryRepository.Object);

            var tooLongName = "name".PadRight(51, 't');

            var result = await handler.Handle(new CreateCategoryCommand() { Name = tooLongName }, CancellationToken.None);

            result.Success.ShouldBe(false);
            result.Category.ShouldBe(null);
            result.ValidationErrors.Count.ShouldBe(1);
        }

        [Fact]
        public async Task CreateCategoryFailsToAddCategoryWithEmptyName()
        {
            _mockCategoryRepository.Setup(repo => repo.IsCategoryNameUnique(It.IsAny<string>())).ReturnsAsync(true);

            var handler = new CreateCategoryCommandHandler(_mapper, _mockCategoryRepository.Object);

            var emptyName = string.Empty;

            var result = await handler.Handle(new CreateCategoryCommand() { Name = emptyName }, CancellationToken.None);

            result.Success.ShouldBe(false);
            result.Category.ShouldBe(null);
            result.ValidationErrors.Count.ShouldBe(1);
        }
    }
}
