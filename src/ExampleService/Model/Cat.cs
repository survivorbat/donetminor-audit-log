namespace ExampleService.Model
{
    public class Cat : Animal
    {
        public string FurColor { get; set; }
        public string FavouriteCatFood { get; set; }

        public Cat(string name) : base(name)
        {
        }
    }
}
