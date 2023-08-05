using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Tests
{
    [TestClass()]
    public class CreateUserActionTests
    {
        public CreateUserAction createUserAction { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            createUserAction = new CreateUserAction();
        }

        [TestMethod]
        public void InsertUserTest()
        {
            var user = new User()
            {
                Username = "test123"
            };

            CreateUserAction createUserAction = new CreateUserAction();

            createUserAction.InsertUser(user);

            var isInserted = createUserAction.IsUserInserted(user.Id);

            Assert.IsTrue(isInserted, "User is inserted");
        }

        [TestCleanup]
        public void Cleanup()
        {
            createUserAction.RemoveUser("test123");
        }
    }
}