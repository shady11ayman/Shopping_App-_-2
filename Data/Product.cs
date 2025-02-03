namespace Shopping_App___2.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? productImgUrl { get; set; }
        public bool isLikedProduct { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

    }
}
