using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using Interfaces.Enum;

namespace Interfaces.FileHandler
{
    public class XMLFile
    {
        private XmlTextWriter _writer;

        public XMLFile(string APath)
        {
            using (var xmlFile = File.Create(APath))
            {
                xmlFile.Close();
            }

                _writer = new XmlTextWriter(APath, System.Text.Encoding.UTF8);
        }

        public void beginDocument()
        {
            _writer.WriteStartDocument(true);
            _writer.Formatting = Formatting.Indented;
            _writer.Indentation = 2;
            _writer.WriteStartElement("Filme&Serien");
        }

        public void endDocument()
        {
            _writer.WriteEndElement();
            _writer.WriteEndDocument();
            _writer.Close();
        }

        public void writeMovieToXML(int id, string name, string desc, string genres, string release, double rating)
        {
            _writer.WriteStartElement("Film");

            _writer.WriteStartElement("ID");
            _writer.WriteString(id.ToString());
            _writer.WriteEndElement();

            _writer.WriteStartElement("Film-Name");
            _writer.WriteString(name.Trim());
            _writer.WriteEndElement();

            _writer.WriteStartElement("Beschreibung");
            _writer.WriteString(desc.Trim());
            _writer.WriteEndElement();

            _writer.WriteStartElement("Genres");
            _writer.WriteString(genres);
            _writer.WriteEndElement();

            _writer.WriteStartElement("Release");
            _writer.WriteString(release);
            _writer.WriteEndElement();

            _writer.WriteStartElement("Rating");
            _writer.WriteString(rating.ToString());
            _writer.WriteEndElement();

            _writer.WriteStartElement("Staffeln");
            _writer.WriteString("0");
            _writer.WriteEndElement();

            _writer.WriteStartElement("Typ");
            _writer.WriteString("0");
            _writer.WriteEndElement();

            _writer.WriteEndElement();
        }

        public ObservableCollection<MediaFields> readXMLFile(string filepath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);
            ObservableCollection<MediaFields> videoProperty = new ObservableCollection<MediaFields>();
            /*
            foreach (XmlNode video in doc.SelectNodes("/Filme&Serien/Film"))
            {
                VideoProperties vp = new VideoProperties();
                vp.Videoname = video["Videoname"].InnerText;
                vp.Beschreibung = video["Beschreibung"].InnerText;
                vp.Kategorie = video["Kategorie"].InnerText;
                vp.Clips = video["Clips"].InnerText;
                vp.UploadDatum = video["UploadDatum"].InnerText;

                videoProperty.Add(vp);
            }*/
            return videoProperty;
        }

        public MediaFields appendMoviefromXML(string filepath, int index)
        {
            int count = -1;
            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);
            MediaFields vp = new MediaFields();
            /*
            foreach (XmlNode video in doc.SelectNodes("/Filme&Serien/Film"))
            {
                count++;

                if (count == index)
                {
                    vp.Videoname = video["Videoname"].InnerText;
                    vp.Beschreibung = video["Beschreibung"].InnerText;
                    vp.Kategorie = video["Kategorie"].InnerText;
                    vp.Clips = video["Clips"].InnerText;
                    vp.UploadDatum = video["UploadDatum"].InnerText;
                }
            }*/

            return vp;
        //}
    }
}
}
