using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Movie_Cleanup.Modules.MediaFile;

namespace Movie_Cleanup.Modules
{
    [XmlRoot("MusicLibrary")]
    public class MediaLibraryManager : MediaLibrary//Movie_Cleanup.Modules.MediaFile.Song
    {
        internal MediaLibrary Load(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MediaLibrary));
            TextReader tr = new StreamReader(fileName);
            //var b = (Artist)serializer.Deserialize(tr);
            tr.Close();

            MediaLibrary myObject;
// Construct an instance of the XmlSerializer with the type
// of object that is being deserialized.
XmlSerializer mySerializer =
new XmlSerializer(typeof(MediaLibrary));
// To read the file, create a FileStream.
FileStream myFileStream = 
new FileStream(fileName, FileMode.Open);
// Call the Deserialize method and cast to the object type.
myObject = (MediaLibrary)mySerializer.Deserialize(myFileStream);
myFileStream.Close();
return myObject;
            //return new FileInfo(fileName).XmlDeserialize<ObservableCollection<MusicLibrary>>();
        }
        public void Save(string fileName)
        {
            this.XmlSerialize(fileName);
        }

        //public object Deserialize(string fileName)
        //{
        //    XmlDocument xmlDoc = new XmlDocument();
        //    //xmlDoc.LoadXml(xmlText);
        //    xmlDoc.Load(fileName);
        //    Type objectType =
        //      Type.GetType(xmlDoc.DocumentElement.Attributes["ObjectType"].Value);
        //    var childs = GetChild(xmlDoc.DocumentElement);
        //    if (childs[0].Name == "Array")//Array found
        //        return DeserializeIEnumerable(childs[0], objectType);
        //    else
        //    {
        //        object obj = Utility.CreateInstance(objectType);
        //        DeserializeProperty(xmlDoc.DocumentElement, obj);
        //        return obj;
        //    }
        //}
    }
}
