using Xunit;
using System.Threading.Tasks;
using CleemyTest.Controllers;
using CleemyTest.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CleemyTest.Methodes;
using System.Data.Entity;
using System;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using System.Linq;
using CleemyTest.Models;

namespace CleemyTest.UnitTests
{
    public class UtilisateursControllerTests
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<DataContext> _contextOptions;

        #region ConstructorAndDispose
        public UtilisateursControllerTests()
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
            context.Utilisateurs.RemoveRange(context.Utilisateurs.Where(u => u.Nom == "Doe" && u.Prenom == "John"));
            context.SaveChanges();
        }

        DataContext CreateContext() => new DataContext(_contextOptions);

        public void Dispose() => _connection.Dispose();
        #endregion

        // Test GET: api/params/Utilisateurs
        [Fact]
        public async void GetUtilisateurs_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new UtilisateursController(context);

            // Act
            var okResult = await controller.GetUtilisateurs();
            // Assert
            Assert.IsType<ActionResult<IEnumerable<Utilisateurs>>>(okResult as ActionResult<IEnumerable<Utilisateurs>>);
        }

        // Test GET: api/params/Utilisateurs/sort
        [Fact]
        public async void GetUtilisateurs_WithSort_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new UtilisateursController(context);

            // Act
            var okResult = await controller.GetUtilisateurs(UtilisateursController.UtilisateursOrder.NomAsc);
            // Assert
            Assert.IsType<ActionResult<IEnumerable<Utilisateurs>>>(okResult as ActionResult<IEnumerable<Utilisateurs>>);
        }

        // GET: api/params/Utilisateurs/Id/5
        [Fact]
        public async void GetUtilisateurs_WithId_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new UtilisateursController(context);

            // Act
            var okResult = await controller.GetUtilisateur(1);
            // Assert
            Assert.IsType<ActionResult<Utilisateurs>>(okResult as ActionResult<Utilisateurs>);
        }

        // PUT: api/params/Utilisateurs/Id/5
        [Fact]
        public async void PutUtilisateurs_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new UtilisateursController(context);

            // Act
            var Utilisateurs = new UtilisateursModel(1, "Stark", "Anthony", 2);
            var okResult = await controller.PutUtilisateurs(1, Utilisateurs);

            // Assert
            Assert.IsType<NoContentResult>(okResult as NoContentResult);
        }

        // POST: api/params/Utilisateurs
        [Fact]
        public async void PostUtilisateurs_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new UtilisateursController(context);

            // Act
            var Utilisateurs = new UtilisateursModel(-1, "Doe", "John", 1);
            var okResult = await controller.PostUtilisateurs(Utilisateurs);
            // Assert
            Assert.IsType<ActionResult<Utilisateurs>>(okResult as ActionResult<Utilisateurs>);
        }
    }
}
