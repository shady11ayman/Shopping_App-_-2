using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_App___2.Data;

namespace Shopping_App___2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController (AppDbContext dbContext) : ControllerBase
    {
        
        [HttpGet]
        [Route("GetUser")]
        public ActionResult<User> getuserByID(int id)
        {
            var user = dbContext.Set<User>().Find(id);
            return user == null ? NotFound() : user;
        }


        [HttpPost]
        [Route("AddUser")]
        public ActionResult<User> AddUser(User user)
        {

            dbContext.Set<User>().Add(user);
            dbContext.SaveChanges();
            return Ok(user);

        }

        [HttpPut]
        [Route("EditUser/{id}")]
        public ActionResult<User> EditUser(int id , [FromBody] User updatedUser)
        {

            var targetUser = dbContext.Set<User>().Find(id);

            targetUser.Name = updatedUser.Name;


            dbContext.Set<User>().Update(targetUser);
            dbContext.SaveChanges();
            return Ok(targetUser);

        }

        [HttpDelete]
        [Route("DeleteUser/{id}")]
        public ActionResult DeleteUser(int id)
        {
            var wantToDelete = dbContext.Set<User>().Find(id);
            dbContext.Set<User>().Remove(wantToDelete);
            dbContext.SaveChanges();
            return Ok(wantToDelete);
        }


    }
}
