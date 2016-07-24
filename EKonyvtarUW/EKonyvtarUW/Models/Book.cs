﻿using EKonyvtarUW.Services;
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

        private static string mekUrl = "http://mek.oszk.hu/{0}/borito.jpg";

        [Column("_id")]
        public string DbId { get; set; }

        [Column("url")]
        public string UrlId { get; set; }
        public string Url { get; set; }

        private Uri _ThumbnailUrl = null;
        public Uri ThumbnailUrl
        {
            get
            {
                if (_ThumbnailUrl != null)
                    return _ThumbnailUrl;

                return new Uri(String.Format(mekUrl, UrlId));
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
            }
        }

        private string _Summary;
        public string Summary { get { return _Summary; } set { _Summary = value; NotifyPropertyChanged("Summary"); } }


        private string _Abbreviation;
        public string Abbreviation { get { return _Abbreviation; } set { _Abbreviation = value; NotifyPropertyChanged("Abbreviation"); } }

        public string Recommendation { get; set; }

        private List<string> _Media;
        public List<string> Media
        {
            get { return _Media; }
            set
            {
                _Media = value;
                NotifyPropertyChanged("Media");
                NotifyPropertyChanged("ContentUri");
                NotifyPropertyChanged("MediaDictionary");
            }
        }

        public IEnumerable<KeyValuePair<string,string>> MediaDictionary
        {
            get
            {
                
                try
                {
                    var results = Media.Select(p => new KeyValuePair<string, string>(p.Split('.').LastOrDefault().ToUpper(),p));
                    return results;
                }
                catch { }
                return null;
            }
        }
        public string ContentUri
        {
            get
            {
                String content = Media.FirstOrDefault() ?? "";

                try
                {
                    if (Regex.IsMatch(content, "\\.pdf$"))
                        return String.Format("https://docs.google.com/gview?url={0}&embedded=true", content);

                    //
                    if (Regex.IsMatch(content, "\\.doc[x]?$"))
                        return String.Format("http://view.officeapps.live.com/op/view.aspx?src={0}", content);

                    return content;
                }
                catch { }
                return null;
            }
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
                this.Abbreviation = book.Abbreviation;
                this.Recommendation = this.Recommendation ?? book.Recommendation;
            } catch (Exception ex)
            {
                //Resolve offline page issues..
            }
        }
    }
}
