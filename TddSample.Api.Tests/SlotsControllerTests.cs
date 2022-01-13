using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TddSample.Api.Application.Query;
using TddSample.Api.Controllers;
using TddSample.Domain;
using Xunit;

namespace TddSample.Api.Tests
{
    public class SlotsControllerTests
    {
        public IServiceCollection _services;
        public IMediator? Mediator { get; private set; }

        public SlotsControllerTests()
        {
            _services = new ServiceCollection();
            _services.AddMediatR(typeof(GetSlotsQuery));
            Mediator = _services.BuildServiceProvider().GetService<IMediator>();
        }

        [Trait("Type","Slots Controller Tests")]
        [Fact(DisplayName = "Should return slots")]
        public async Task ShouldReturnSlotsAsync()
        {
            // Arrange
            var controller = new SlotsController(Mediator);
            // Act
            var slots = await controller.Get();
            // Assert
            slots.Should().NotBeNull();
        }

        [Trait("Type", "Slots Controller Tests")]
        [Fact(DisplayName = "Should return valid slots")]
        public async Task ShouldReturnValidSlot()
        {
            // Arrange
            var controller = new SlotsController(Mediator);
            // Act
            var result = await controller.Get();
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var slots = Assert.IsAssignableFrom<IEnumerable<Slot>>(objectResult.Value);
            // Assert
            slots.First().From.Hours.Should().BeGreaterThan(0);
        }

        [Trait("Type", "Slots Controller Tests")]
        [Fact(DisplayName = "Should return slots after current time")]
        public async Task ShouldReturnSlotsAfterCurrentTime()
        {
            // Arrange
            var controller = new SlotsController(Mediator);
            // Act
            var result = await controller.Get();
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var slots = Assert.IsAssignableFrom<IEnumerable<Slot>>(objectResult.Value);
            // Assert
            slots.First().From.Hours.Should().BeGreaterThan(DateTime.Now.Hour);
        }
    }
}