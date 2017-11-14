using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Http;
using System.Web.Http.Results;
using DataExchange;
using DataExchange.EntityModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSD.Web.Api.Controllers;
using SSD.Web.Api.Models;

namespace WebApiTests
{
    /// <summary>
    ///     Summary description for MenuControllerUnitTests 
    /// </summary>
    [TestClass]
    public class MenuControllerUnitTests
    {
        /// <summary>
        ///     Cleanups this instance. 
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _db.Dispose();
            _scope.Dispose();
        }

        [TestMethod]
        public void GetAllMenuItems()
        {
            // Arrange
            var model = _db.Menus.ListAll();
            var controller = new MenuController(_db);

            // Act
            IHttpActionResult actionResult = controller.Get();
            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<MenuListBindingModel>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(model.Count, contentResult.Content.Count());
        }

        [TestMethod]
        public void GetRootMenuItemsForCustomers()
        {
            // Arrange
            string role = "Customer";
            IEnumerable<Menu> model = _db.Menus.GetMenuByRole(role);
            var controller = new MenuController(_db);

            // Act
            IHttpActionResult actionResult = controller.GetRootMenus(role);
            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<MenuListBindingModel>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(model.Count(), contentResult.Content.Count());
        }

        [TestMethod]
        public void GetRootMenuItemsForGuests()
        {
            // Arrange
            string role = "Guest";
            IEnumerable<Menu> model = _db.Menus.GetMenuByRole(role);
            var controller = new MenuController(_db);

            // Act
            IHttpActionResult actionResult = controller.GetRootMenus(role);
            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<MenuListBindingModel>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(model.Count(), contentResult.Content.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetRootMenuItemsForNullRole()
        {
            // Arrange
            string role = "Bob";
            IEnumerable<Menu> model = _db.Menus.GetMenuByRole(role);
            var controller = new MenuController(_db);

            // Act
            IHttpActionResult actionResult = controller.GetRootMenus(role);
            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<MenuListBindingModel>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(model.Count(), contentResult.Content.Count());
        }

        [TestMethod]
        public void GetSubMenuItemsForCustomers()
        {
            // Arrange
            string role = "Customer";
            IEnumerable<Menu> model = _db.Menus.GetSubMenusByRole(role);
            var controller = new MenuController(_db);

            // Act
            IHttpActionResult actionResult = controller.GetSubMenus(role);
            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<MenuListBindingModel>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(model.Count(), contentResult.Content.Count());
        }

        [TestMethod]
        public void GetSubMenuItemsForGuests()
        {
            // Arrange
            string role = "Guest";
            IEnumerable<Menu> model = _db.Menus.GetSubMenusByRole(role);
            var controller = new MenuController(_db);

            // Act
            IHttpActionResult actionResult = controller.GetSubMenus(role);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetSubMenuItemsForNullRole()
        {
            // Arrange
            string role = "Guest";
            IEnumerable<Menu> model = _db.Menus.GetSubMenusByRole(role);
            var controller = new MenuController(_db);

            // Act
            IHttpActionResult actionResult = controller.GetSubMenus(role);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        /// <summary>
        ///     Initializes this instance. 
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _scope = new TransactionScope();
            _db = new DataManager(new AppDbContext());
        }

        private DataManager _db;
        private TransactionScope _scope;
    }
}