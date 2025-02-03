using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shopping_App___2.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shopping_App___2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    
    public class ProductController(AppDbContext dbContext , JwtOptions jwtOptions) : ControllerBase
    {

        //done
        [HttpPost]
        [Route("AddProduct")]
        public ActionResult<Product> addProduct(Product product)
        {
            dbContext.Set<Product>().Add(product);
            dbContext.SaveChanges();
            return Ok(product);
        }

        
        [HttpGet]
        [Route("GetProducts")]
         [Authorize(Roles = "admin")]
       // [AllowAnonymous]
        public ActionResult<IEnumerable<Product>> GetAllProducts()
        {
            var products = dbContext.Set<Product>().ToList();
         
            return Ok(products);
        }
        //done
        [HttpGet]
        [Route("getById")]
        public ActionResult<Product> getById(int id)
        {
            var product = dbContext.Set<Product>().Find(id);
            return Ok(product);
        }
        #region MyRegion

        /*
        [HttpGet]
        [Route("GetUsersWithLikedProducts")]
        public ActionResult<IEnumerable<User>> GetUsersWithLikedProducts()
        {
            // Using a LINQ query to join Users and Products
            var usersWithLikedProducts = dbContext.Users
                .Include(u => u.Products) // Eagerly loading related Products
                .Where(u => u.Products.Any(p => p.isLikedProduct))
                .ToList();

            return Ok(usersWithLikedProducts);
        }*/

        /*
        [HttpGet]
        [Route("GetUsersWiiithLikedProducts")]
        public IActionResult GetUsersWiiithLikedProducts()
        {
            // Query products where IsLikedProduct is true
            var likedProducts = dbContext.Products
                .Where(p => p.isLikedProduct)
                .Include(p => p.User) // Ensure users are included
                .Select(p => new
                {
                    UserId = p.User.Id,
                    UserName = p.User.Name,
                    LikedProductName = p.Name
                })
                .ToList();

            return Ok(likedProducts);
        }*/ 
        #endregion

        [HttpGet]
        [Route("GetUsersWithLikedProducts")]
       
        [Authorize(Roles = "admin")]

        public ActionResult<IEnumerable<User>> GetUsersWithLikedProducts()
        {
          
             var usersWithLikedProducts = dbContext.Products
                .Where(p => p.isLikedProduct)
                .Select(p => p.User)
                .Distinct()
                .ToList();

            return Ok(usersWithLikedProducts);
        }

        [HttpPut]
        [Route("EditProduct/{id}")]
        public ActionResult<Product> EditProduct(int id , [FromBody] Product updatedProduct)
        {
            var existingProduct = dbContext.Set<Product>().Find(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = updatedProduct.Name;
            
           
            dbContext.SaveChanges();
            return Ok(existingProduct);
        }

        [HttpDelete]
        [Route("DeleteProduct/{id}")]
        public ActionResult<Product> DeleteProduct(int id)
        {
            var product = dbContext.Set<Product>().Find(id);
            if (product == null)
            {
                return NotFound();
            }

            dbContext.Set<Product>().Remove(product);
            dbContext.SaveChanges();
            return Ok(product);
        }


        [HttpPost]
        [Route("auth")]
        [AllowAnonymous]
        public ActionResult<string> AuthenticateUser(AuthenticateRequest request)
        {
            var user = dbContext.Set<User>().FirstOrDefault(x => x.Name == request.UserName && x.Password == request.Password);
            if (user == null) return Unauthorized();
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescribtor = new SecurityTokenDescriptor
            {
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
               , SecurityAlgorithms.HmacSha256),

                Subject = new ClaimsIdentity(new Claim[]
             {
                new (ClaimTypes.NameIdentifier , request.UserName),
                new (ClaimTypes.NameIdentifier , request.Password),
               // new (ClaimTypes.NameIdentifier , user.Id.ToString()),
                //new (ClaimTypes.Name , user.Name),
                new (ClaimTypes.Role, "admin")
               
             })
            };

            var secuirityToken = tokenHandler.CreateToken(tokenDescribtor);
            var accessToken = tokenHandler.WriteToken(secuirityToken);
            return Ok(accessToken);
        }
        
    }

}


