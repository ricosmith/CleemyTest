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

namespace CleemyTest.UnitTests
{
    public class DepensesControllerTests : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<DataContext> _contextOptions;

        #region ConstructorAndDispose
        public DepensesControllerTests()
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
            context.Depenses.RemoveRange(context.Depenses);
            context.SaveChanges();

            var dollar = context.Devises.Find(2);
            var rouble = context.Devises.Find(3);
            var restaurant = context.Natures.Find(1);
            var hotel = context.Natures.Find(2);
            var stark = context.Utilisateurs.Find(1);
            var romanova = context.Utilisateurs.Find(2);

            context.Depenses.Add(new Depenses
            {
                Date = DateTime.Today.AddDays(-1),
                Montant = 15,
                Commentaire = "Test Commentaire",
                DeviseFK = dollar.Id,
                NatureFK = restaurant.Id,
                UtilisateurFK = stark.Id
            });

            context.Depenses.Add(new Depenses
            {
                Date = DateTime.Today.AddDays(-1),
                Montant = 15,
                Commentaire = "Test Commentaire",
                DeviseFK = rouble.Id,
                NatureFK = hotel.Id,
                UtilisateurFK = romanova.Id
            });

            context.SaveChanges();
        }

        DataContext CreateContext() => new DataContext(_contextOptions);

        public void Dispose() => _connection.Dispose();
        #endregion

        // Test GET: api/Depenses/Id/{DepensesId}
        [Fact]
        public async void GetDepenses_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new DepensesController(context);

            // Act
            var okResult = await controller.GetDepense(1);
            // Assert
            Assert.IsType<ActionResult<Depenses>>(okResult as ActionResult<Depenses>);
        }

        // Test GET: api/Depenses/Utilisateur/{UtilisateursId}
        [Fact]
        public async void GetDepensesUtilisateur_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new DepensesController(context);

            // Act
            var okResult = await controller.GetDepensesUtilisateur(1);
            // Assert
            Assert.IsType<ActionResult<IEnumerable<Depenses>>>(okResult as ActionResult<IEnumerable<Depenses>>);
        }

        // Test GET: api/Depenses/Utilisateur/{UtilisateursId}/{sort}
        [Fact]
        public async void GetDepensesUtilisateur_WithSort_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new DepensesController(context);

            // Act
            var okResult = await controller.GetDepensesUtilisateur(1, DepensesController.DepensesOrder.montantAsc);
            // Assert
            Assert.IsType<ActionResult<IEnumerable<Depenses>>>(okResult as ActionResult<IEnumerable<Depenses>>);
        }

        // GET: api/Depenses/AffichDesc/{Id}
        [Fact]
        public async void GetAffichDesc_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new DepensesController(context);

            // Act
            var okResult = await controller.GetAffichDesc(1);
            // Assert
            Assert.IsType<ActionResult<string>>(okResult as ActionResult<string>);
        }

        // PUT: api/Depenses/5
        [Fact]
        public async void PutDepenses_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new DepensesController(context);

            // Act
            var depenses = new DepensesModel(1, 2, System.DateTime.Today, 2, 3, 20, "Modif Commentaire");
            var okResult = await controller.PutDepenses(1, depenses);

            // Assert
            Assert.IsType<NoContentResult> (okResult as NoContentResult);
        }

        // POST: api/Depenses
        [Fact]
        public async void PostDepenses_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new DepensesController(context);

            // Act
            var depenses = new DepensesModel(0, 2, System.DateTime.Today.AddDays(-5), 2, 3, 15, "Modif Commentaire");
            var okResult = await controller.PostDepenses(depenses);
            // Assert
            Assert.IsType<ActionResult<Depenses>>(okResult as ActionResult<Depenses>);
        }

        // DELETE: api/Depenses/5
        //[Fact]
        //public async void DeleteDepenses_WhenCalled_ReturnsOkResult()
        //{
        //    // Act
        //    var depense = await _context.Depenses.Include(util => util.Utilisateurs).FirstOrDefaultAsync(i => i.Utilisateurs.Nom == "Stark" && i.Date == DateTime.Today.AddDays(-1) && i.Montant == 15);
        //    var okResult = await controller.DeleteDepenses(depense.Id);
        //    // Assert
        //    Assert.Equal(null, okResult);
        //}

        // Verifs Contraintes pas d'utilisateur
        [Fact]
        public async void PostDepenses_WhenCalled_ReturnsPasUtil()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new DepensesController(context);

            // Act
            var depenses = new DepensesModel(2, 999, System.DateTime.Today, 2, 3, 15, "Commentaire");
            var okResult = await controller.PostDepenses(depenses);

            // Assert
            Assert.IsType<ObjectResult>(okResult.Result as ObjectResult);
        }

        // Verifs Contraintes pas de devise
        [Fact]
        public async void PostDepenses_WhenCalled_ReturnsPasDevise()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new DepensesController(context);

            // Act
            var depenses = new DepensesModel(2, 2, System.DateTime.Today, 2, 999, 15, "Commentaire");
            var okResult = await controller.PostDepenses(depenses);

            // Assert
            Assert.IsType<ObjectResult>(okResult.Result as ObjectResult);
        }

        // Verifs Contraintes pas de nature
        [Fact]
        public async void PostDepenses_WhenCalled_ReturnsPasNature()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new DepensesController(context);

            // Act
            var depenses = new DepensesModel(2, 2, System.DateTime.Today, 999, 3, 15, "Commentaire");
            var okResult = await controller.PostDepenses(depenses);

            // Assert
            Assert.IsType<ObjectResult>(okResult.Result as ObjectResult);
        }

        // Verifs Contraintes date future
        [Fact]
        public async void PostDepenses_WhenCalled_ReturnsDateFuture()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new DepensesController(context);

            // Act
            var depenses = new DepensesModel(2, 2, System.DateTime.Today.AddDays(25), 2, 3, 15, "Commentaire");
            var okResult = await controller.PostDepenses(depenses);

            // Assert
            Assert.IsType<ObjectResult>(okResult.Result as ObjectResult);
        }

        // Verifs Contraintes date - de 3 mois
        [Fact]
        public async void PostDepenses_WhenCalled_ReturnsDate3Mois()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new DepensesController(context);

            // Act
            var depenses = new DepensesModel(2, 2, System.DateTime.Today.AddMonths(-4), 2, 3, 15, "Commentaire");
            var okResult = await controller.PostDepenses(depenses);

            // Assert
            Assert.IsType<ObjectResult>(okResult.Result as ObjectResult);
        }

        // Verifs Contraintes commentaire obligatoire
        [Fact]
        public async void PostDepenses_WhenCalled_ReturnsPasCommentaire()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new DepensesController(context);

            // Act
            var depenses = new DepensesModel(2, 2, System.DateTime.Today, 2, 3, 15, "");
            var okResult = await controller.PostDepenses(depenses);

            // Assert
            Assert.IsType<ObjectResult>(okResult.Result as ObjectResult);
        }

        // Verifs Contraintes devise check
        [Fact]
        public async void PostDepenses_WhenCalled_ReturnsDevisePasOK()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new DepensesController(context);

            // Act
            var depenses = new DepensesModel(2, 2, System.DateTime.Today, 2, 1, 15, "Commentaire");
            var okResult = await controller.PostDepenses(depenses);

            // Assert
            Assert.IsType<ObjectResult>(okResult.Result as ObjectResult);
        }

        // Verifs Contraintes doublon
        [Fact]
        public async void PostDepenses_WhenCalled_ReturnsDoublon()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new DepensesController(context);

            // Act
            var depenses = new DepensesModel(1, 1, System.DateTime.Today.AddDays(-1), 1, 2, 15, "Test Commentaire");
            var okResult = await controller.PostDepenses(depenses);

            // Assert
            Assert.IsType<ObjectResult>(okResult.Result as ObjectResult);
        }
    }
}