using CleemyTest.Controllers;
using CleemyTest.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CleemyTest.UnitTests
{
    public class NaturesControllerTests
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<DataContext> _contextOptions;

        #region ConstructorAndDispose
        public NaturesControllerTests()
        {
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite(_connection)
                .Options;

            // Create the schema and seed some data
            using var context = new DataContext(_contextOptions);

            // Add entities for DbContext instance
            context.Natures.RemoveRange(context.Natures.Where(n => n.Desc == "Train"));
            context.SaveChanges();
        }

        DataContext CreateContext() => new DataContext(_contextOptions);

        public void Dispose() => _connection.Dispose();
        #endregion

        // Test GET: api/params/Natures
        [Fact]
        public async void GetNatures_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new NaturesController(context);

            // Act
            var okResult = await controller.GetNatures();
            // Assert
            Assert.IsType<ActionResult<IEnumerable<Natures>>>(okResult as ActionResult<IEnumerable<Natures>>);
        }

        // Test GET: api/params/Natures/sort
        [Fact]
        public async void GetNatures_WithSort_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new NaturesController(context);

            // Act
            var okResult = await controller.GetNatures(NaturesController.NaturesOrder.descriptionAsc);
            // Assert
            Assert.IsType<ActionResult<IEnumerable<Natures>>>(okResult as ActionResult<IEnumerable<Natures>>);
        }

        // GET: api/params/Natures/Id/5
        [Fact]
        public async void GetNatures_WithId_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new NaturesController(context);

            // Act
            var okResult = await controller.GetNature(1);
            // Assert
            Assert.IsType<ActionResult<Natures>>(okResult as ActionResult<Natures>);
        }

        // PUT: api/params/Natures/Id/5
        [Fact]
        public async void PutNatures_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new NaturesController(context);

            // Act
            var Natures = context.Natures.FirstOrDefault(nat => nat.Id == 1);
            var okResult = await controller.PutNatures(1, Natures);

            // Assert
            Assert.IsType<NoContentResult>(okResult as NoContentResult);
        }

        // POST: api/params/Natures
        [Fact]
        public async void PostNatures_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new NaturesController(context);

            // Act
            var Natures = new Natures() { Desc = "Train" };
            var okResult = await controller.PostNatures(Natures);
            // Assert
            Assert.IsType<ActionResult<Natures>>(okResult as ActionResult<Natures>);
        }
    }
}
