namespace ExampleService.Model
{
    public class Elephant : Animal
    {
        public string SkinColor { get; set; }

        public Elephant(string name) : base(name)
        {
        }
    }
}
