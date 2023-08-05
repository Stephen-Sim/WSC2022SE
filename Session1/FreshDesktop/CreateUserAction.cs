using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshDesktop
{
    public class CreateUserAction
    {
        WSC2022SE_Session1Entities ent;

        public CreateUserAction()
        {
            ent = new WSC2022SE_Session1Entities();
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
            return ent.Users.Any(x => x.ID == id);
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
