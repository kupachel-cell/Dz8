

namespace Dz7tum.classes
{
    public class Song
    {
        private string name;  
        private string author; 
        private Song prev;    

       public Song()
        {
            Console.WriteLine("Введите название песни");
            name = Console.ReadLine();
            Console.WriteLine("Введите автора песни");
            author = Console.ReadLine();

            prev = null;
        }
        public Song(string name, string author) : this(name, author, null)
        {
        }
        public Song(string name, string author, Song prev)
        {
            this.name = name;
            this.author = author;
            this.prev = prev;
        }
        public string Title()
        {
            return $"{name} - {author}";
        }

        public void PrintInfo()
        {
            Console.WriteLine(Title());
        }
    }
}

