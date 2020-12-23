using System;

namespace Common.model
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Isbn { get; set; }
        public string Author { get; set; }
        public DateTime Published { get; set; }
        public int Pages { get; set; }
        public string ?Language { get; set; }
        public Genre ?Genre { get; set; }


        protected bool Equals(Book other)
        {
            return Isbn == other.Isbn;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Book) obj);
        }

        public override int GetHashCode()
        {
            return (Isbn != null ? Isbn.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Title)}: {Title}, {nameof(Isbn)}: {Isbn}, {nameof(Author)}: {Author}, {nameof(Published)}: {Published}, {nameof(Pages)}: {Pages}, {nameof(Language)}: {Language}, {nameof(Genre)}: {Genre}";
        }
    }
}