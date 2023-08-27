using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Session4UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        public WSC2022SE_Session4Entities ent { get; set; }
        [TestInitialize]
        public void Init() 
        {
            ent = new WSC2022SE_Session4Entities();
        }

        [TestMethod]
        public void CheckDates()
        {
            var itemPrices = ent.ItemPrices.ToList().Where(x => x.Date.Date < new DateTime(2017, 2, 17)).ToList();

            Assert.IsNotNull(itemPrices, "There are property or listing have availabilites before 19/02/2017");
        }

        [TestMethod]
        public void CheckScore()
        {
            var itemScores = ent.ItemScores.Where(x => x.Value < 0 || x.Value > 5).ToList();

            Assert.IsNotNull(itemScores, "There are property or listing have score higher than 5 or lower than 0");
        }

        [TestMethod]
        public void CheckDuplicateUser()
        {
            var isTrue = ent.Users.ToList().GroupBy(x => new
            {
                x.Username,
            }).Any(x => x.Count() >= 2);

            Assert.IsFalse(isTrue, "There is duplicated users.");
        }
    }
}
