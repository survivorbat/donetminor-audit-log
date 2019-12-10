using ExampleService.Model;

namespace ExampleService
{
    internal static partial class SeedData
    {
        public static readonly Person[] Persons = {
            new Person {FirstName = "Jan", LastName = "Hendrik", Age = 23},
            new Person {FirstName = "Peter", LastName = "Vickan", Age = 34},
            new Person {FirstName = "Bernt", LastName = "Lennon", Age = 44},
        };

        public static readonly Animal[] Animals = {
            new Cat("Jeffrey") {FavouriteCatFood = "Gourmet", FurColor = "Gray"},
            new Cat("Aspra") {FavouriteCatFood = "Pura", FurColor = "White"},
            new Cat("Luna") {FavouriteCatFood = "Whiskas", FurColor = "White"},
            new Elephant("John") { SkinColor = "Gray"},
            new Elephant("Patrick") { SkinColor = "Brown"},
        };
    }
}
