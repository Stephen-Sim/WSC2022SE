using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class CreateUserAction
    {
        UnitTestDBEntities ent;

        public CreateUserAction()
        {
            ent = new UnitTestDBEntities();
        }

        public void InsertUser(User user)
        {
            try
            {
                ent.Users.Add(user);
                ent.SaveChanges();
            }
            catch (Exception)
            {

            }
        }

        public bool IsUserInserted(long id)
        {
            return ent.Users.Any(x => x.Id == id);
        }

        public void RemoveUser(string username)
        {
            try
            {
                ent.Users.Remove(ent.Users.FirstOrDefault(x => x.Username == username));
                ent.SaveChanges();
            }
            catch (Exception)
            {

            }
        }
    }
}
