using System;

namespace lab1v12
{
    class Song
    {
        // Поля
        private string title;
        private string artist;

        public double Duration { get; set; }

        // Конструктор
        public Song(string title, string artist, double duration)
        {
            this.title = title;
            this.artist = artist;
            this.Duration = duration;
        }

        // Деструктор
        ~Song()
        {
            Console.WriteLine($"Об'єкт пісні \"{title}\" видалено.");
        }

        // Метод
        public void Play()
        {
            Console.WriteLine($"Грає пісня: {title} від {artist}, тривалість {Duration} хв.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Song song1 = new Song("Song 1", "Artist 1", 3.5);
            Song song2 = new Song("Song 2", "Artist 2", 4.1);
            Song song3 = new Song("Song 3", "Artist 3", 5.9);

            song1.Play();
            song2.Play();
            song3.Play();

            song1 = null!;
            song2 = null!;


        }
    }
}