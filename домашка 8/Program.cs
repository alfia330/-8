//домашка 9.1
using System;

class Song
{
    private string name;
    private string author;
    private Song prev;

    public Song(string name, string author)
    {
        this.name = name;
        this.author = author;
        this.prev = null;
    }

    public Song(string name, string author, Song prev)
    {
        this.name = name;
        this.author = author;
        this.prev = prev;
    }

    public Song() : this("unknown", "unknown") { }

    public override string ToString() => $"{name} — {author}";
}

class Program
{
    static void Main()
    {
        Song song1 = new Song("Blinding Lights", "The Weeknd");
        Song song2 = new Song("Save Your Tears", "The Weeknd", song1);
        Song mySong = new Song();

        Console.WriteLine(song1);
        Console.WriteLine(song2);
        Console.WriteLine(mySong);
    }
}