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
    public class DevisesControllerTests
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<DataContext> _contextOptions;

        #region ConstructorAndDispose
        public DevisesControllerTests()
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
            context.Devises.RemoveRange(context.Devises.Where(d => d.Desc == "Dollar Canadien"));
            context.SaveChanges();
        }

        DataContext CreateContext() => new DataContext(_contextOptions);

        public void Dispose() => _connection.Dispose();
        #endregion

        // Test GET: api/params/Devises
        [Fact]
        public async void GetDevises_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new DevisesController(context);

            // Act
            var okResult = await controller.GetDevises();
            // Assert
            Assert.IsType<ActionResult<IEnumerable<Devises>>>(okResult as ActionResult<IEnumerable<Devises>>);
        }

        // Test GET: api/params/Devises/sort
        [Fact]
        public async void GetDevises_WithSort_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new DevisesController(context);

            // Act
            var okResult = await controller.GetDevises(DevisesController.DevisesOrder.descriptionAsc);
            // Assert
            Assert.IsType<ActionResult<IEnumerable<Devises>>>(okResult as ActionResult<IEnumerable<Devises>>);
        }

        // GET: api/params/Devises/Id/5
        [Fact]
        public async void GetDevises_WithId_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new DevisesController(context);

            // Act
            var okResult = await controller.GetDevise(1);
            // Assert
            Assert.IsType<ActionResult<Devises>>(okResult as ActionResult<Devises>);
        }

        // PUT: api/params/Devises/Id/5
        [Fact]
        public async void PutDevises_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new DevisesController(context);

            // Act
            var devises = context.Devises.FirstOrDefault(dev => dev.Id == 1);
            var okResult = await controller.PutDevises(1, devises);

            // Assert
            Assert.IsType<NoContentResult>(okResult as NoContentResult);
        }

        // POST: api/params/Devises
        [Fact]
        public async void PostDevises_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new DevisesController(context);

            // Act
            var devises = new Devises() { Desc = "Dollar Canadien" };
            var okResult = await controller.PostDevises(devises);
            // Assert
            Assert.IsType<ActionResult<Devises>>(okResult as ActionResult<Devises>);
        }
    }
}
