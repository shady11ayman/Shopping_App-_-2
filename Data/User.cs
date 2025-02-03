namespace Shopping_App___2.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
