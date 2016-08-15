using EKonyvtarUW.Services;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EKonyvtarUW.Models
{
    [Table("VIEW_BOOKS")]
    public class Book : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private static string mekThumbnailUrl = "http://mek.oszk.hu/{0}/borito.jpg";
        private static string mekUrl = "http://mek.oszk.hu/{0}";

        [Column("_id")]
        public string DbId { get; set; }

        [Column("url")]
        public string UrlId { get; set; }

        private string _url = null;
        public string Url
        {
            get
            {
                return _url ?? String.Format(mekUrl, UrlId);
            }
            set { _url = value; }
        }

        private Uri _ThumbnailUrl = null;
        public Uri ThumbnailUrl
        {
            get
            {
                if (_ThumbnailUrl != null)
                    return _ThumbnailUrl;

                return new Uri(String.Format(mekThumbnailUrl, UrlId));
            }
            set
            {
                _ThumbnailUrl = value;
            }
        }

        private string _Title = String.Empty;
        [Column("title")]
        public string Title
        {
            get { return _Title; }
            set
            {
                _Title = value;
                NotifyPropertyChanged("Title");
            }
        }

        [Column("creator_name")]
        public string Creators { get; set; }

        [Column("subtitle")]
        public string SubTitle { get; set; }

        [Column("year")]
        public string Year { get; set; }

        private string _Contents;
        public string Contents
        {
            get { return _Contents; }
            set
            {
                _Contents = value;
                NotifyPropertyChanged("Contents");
                NotifyPropertyChanged("HasContent");
            }
        }

        private string _Summary;
        public string Summary
        {
            get { return _Summary; }
            set
            {
                _Summary = value;
                NotifyPropertyChanged("Summary");
                NotifyPropertyChanged("HasContent");
            }
        }


        private string _Labels;
        public string Labels { get { return _Labels; } set { _Labels = value; NotifyPropertyChanged("Labels"); } }

        public string Recommendation { get; set; }

        public bool HasContent
        {
            get
            {
                return (!string.IsNullOrWhiteSpace(Summary) || (!string.IsNullOrWhiteSpace(Contents) || !string.IsNullOrWhiteSpace(Recommendation)));
            }
        }

        private List<string> _Media;
        public List<string> Media
        {
            get { return _Media; }
            set
            {
                _Media = value;
                NotifyPropertyChanged("Media");
                NotifyPropertyChanged("PreferedMedia");
                NotifyPropertyChanged("MediaDictionary");
            }
        }

        //TODO: switch this to human readable
        public IEnumerable<KeyValuePair<string, string>> MediaDictionary
        {
            get
            {

                try
                {
                    var results = Media.Select(p => new KeyValuePair<string, string>(p.Split('.').LastOrDefault().ToUpper(), p));
                    return results;
                }
                catch { }
                return null;
            }
        }

        private string _preferedMedia = null;
        public string PreferedMedia
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_preferedMedia))
                    return Media?.FirstOrDefault() ?? "";

                return _preferedMedia;
            }
            set { _preferedMedia = value; }
        }
        public string ContentUri
        {
            get
            {
                try
                {

                }
                catch { }
                return null;
            }
        }

        public bool IsFavorite
        {
            get
            {
                return FavoriteService.IsBookFavorited(this);
            }
        }

        public void ToggleFavorite()
        {
            if (IsFavorite)
                FavoriteService.RemoveBook(this);
            else
                FavoriteService.AddBook(this);

            NotifyPropertyChanged("IsFavorite");
        }

        public void CopyFrom(Book book)
        {
            try
            {
                this.Title = book.Title;
                this.Creators = book.Creators;
                this.Media = book.Media;

                //Textual content
                this.Contents = book.Contents;
                this.Summary = book.Summary;
                this.Labels = book.Labels;
                this.Recommendation = this.Recommendation ?? book.Recommendation;
            }
            catch (Exception ex)
            {
                //TODO: Resolve offline page issues..
            }
        }

        public string ShareInfo
        {
            get
            {
                return String.Format("{0}\n{1}\n\n", Title, Url);
            }
        }
    }
}
