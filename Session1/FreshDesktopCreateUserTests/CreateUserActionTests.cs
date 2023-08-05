using Microsoft.VisualStudio.TestTools.UnitTesting;
using FreshDesktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshDesktop.Tests
{
    /**
     * 
     * have to add the entity model and delete it,
     * for adding nuget purposes
     */
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
                GUID = Guid.NewGuid(),
                UserTypeID = 2,
                Username = "test123",
                FullName = "test test",
                Password = "abc123",
                Gender = true,
                BirthDate = new DateTime(2000, 1, 1),
                FamilyCount = 5
            };

            CreateUserAction createUserAction = new CreateUserAction();

            createUserAction.InsertUser(user);

            var isInserted = createUserAction.IsUserInserted(user.ID);

            Assert.IsTrue(isInserted, "User is not inserted");
        }

        [TestCleanup]
        public void Cleanup()
        {
            createUserAction.RemoveUser("test123");
        }
    }
}