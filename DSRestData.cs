using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Linq;
using System.Xml;

using DataSphere.WebServiceLib.WCFSerializer;
using System.Xml.Serialization;
using SS.CommonUtilities.StringUtil;
using SS.CommonUtilities.Collections;
using System.Text.RegularExpressions;

namespace DataSphere.WebServiceLib
{
    [Serializable]
    [XmlInclude(typeof(DSRestProperty))]
    [KnownType("GetDerivedTypes")]
    public class DSRestData : IXmlSerializable, IDSCollectionValue
    {
        static public Type DSRestDataType;
        static DSRestData()
        {
            DSRestData p = new DSRestData();
            DSRestDataType = p.GetType();
        }

        private readonly string TRANSACTION_TAG_KEY = "TRANSACTION_TAG";

        public DSRestData() { }

        public DSRestData(List<Attr> attrs, DSGenCollection<DSCollection<DSRestData>> objects)
        {
            m_attributes = attrs;
            m_objects = objects;
        }

        public DSRestDataDC ToDC()
        {
            DSRestDataDC j = new DSRestDataDC();
            j.Attributes = Attributes;
            j.Objects = ObjectsToDCClass();
            return j;
        }

        public DSGenCollectionDC<DSCollectionDC> ObjectsToDCClass()
        {
            DSGenCollectionDC<DSCollectionDC> jo = new DSGenCollectionDC<DSCollectionDC>();
            foreach (string key in Objects.Keys)
            {
                DSCollection<DSRestData> vals = Objects[key];
                DSCollectionDC jvals = new DSCollectionDC();
                foreach (string kv in vals.Keys)
                    jvals.Add(kv, vals[kv].ToDC());
                jo.Add(key, jvals);
            }
            return jo;
        }

        public T MakeSubTypeSpecific<T>() where T : DSRestData, new()
        {
            if (this is T)
                return this as T;
            DSRestData p = new T() as DSRestData;
            p.Attributes = Attributes;
            p.Objects = Objects;
            return p as T;
        }

        public DSRestData MakeDSRestData()
        {
            if (this.GetType() == DSRestDataType)
                return this;
            DSRestData p = new DSRestData();
            p.Attributes = Attributes;
            p.Objects = Objects;
            return p;
        }

        #region simple Attributes

        private List<Attr> m_attributes;
        public List<Attr> Attributes
        {
            get
            {
                if (m_attributes == null)
                    m_attributes = new List<Attr>();
                return m_attributes;
            }
            set
            {
                m_attributes = value;
            }
        }

        #region Keys

        public List<string> Keys
        {
            get
            {
                return Attributes.Keys();
            }
        }

        public List<string> KeysNoTmp
        {
            get
            {
                return Attributes.KeysNoTmp();
            }
        }

        #endregion

        #region Get

        public List<string> Get(params string[] path)
        {
            if (path != null && path.Length == 1 && path[0].Contains(":"))
            {
                string[] objNamePath = path[0].Split(':');
                if (objNamePath.Length == 3) // if not let it be handled by return Attributes.Get(path);
                    return GetObject(objNamePath[0], objNamePath[1], true).Get(objNamePath[2]);
            }
            return Attributes.Get(path);
        }

        public List<Attr> GetAttr(params string[] path)
        {
            return Attributes.GetAttr(path);
        }

        public List<IAttrExt> GetAttrExt(params string[] path)
        {
            return Attributes.GetAttrExt(path);
        }

        public IAttrExt Get1AttrExt(params string[] path)
        {
            return Attributes.Get1AttrExt(path);
        }

        public List<string> Get_SFLT(string name, string sflt)
        {
            return Attributes.Get_SFLT(name, sflt);
        }

        public List<string> Get_NOTSFLT(string name, string sflt)
        {
            return Attributes.Get_NOTSFLT(name, sflt);
        }

        public string Get1(params string[] path)
        {
            if (path != null && path.Length == 1 && path[0].Contains(":"))
            {
                string[] objNamePath = path[0].Split(':');
                if (objNamePath.Length == 3) // if not let it be handled by return Attributes.Get1(path);
                    return GetObject(objNamePath[0], objNamePath[1], true).Get1(objNamePath[2]);
            }
            return Attributes.Get1(path);
        }

        public string Get1_SFLT(string name, string sflt)
        {
            return Attributes.Get1_SFLT(name, sflt);
        }

        public string Get1_SFLT(string name, string sflt, bool bCleanupString)
        {
            return Attributes.Get1_SFLT(name, sflt, bCleanupString);
        }

        public string Get1_NOTSFLT(string name, string sflt)
        {
            return Attributes.Get1_NOTSFLT(name, sflt);
        }

        public string Get1_NOTSFLT(string name, string sflt, bool bCleanupString)
        {
            return Attributes.Get1_NOTSFLT(name, sflt, bCleanupString);
        }

        public DateTime? Get1DateTime(params string[] list)
        {
            return Attributes.Get1DateTime(list);
        }

        public DateTime Get1DateTime(DateTime defaultValue, params string[] list)
        {
            return Attributes.Get1DateTime(defaultValue, list);
        }

        public DateTimeOffset? Get1DateTimeOffset(params string[] list)
        {
            return Attributes.Get1DateTimeOffset(list);
        }

        public DateTimeOffset Get1DateTimeOffset(DateTimeOffset defaultValue, params string[] list)
        {
            return Attributes.Get1DateTimeOffset(defaultValue, list);
        }

        public void Set1(string name, decimal? value)
        {
            Set1(name, (value == null) ? null : value.ToString());
        }

        public int? Get1Int(params string[] list)
        {
            return Attributes.Get1Int(list);
        }

        public int Get1Int(int defaultValue, params string[] list)
        {
            return Attributes.Get1Int(defaultValue, list);
        }

        public bool Get1Bool(bool defaultVal, params string[] list)
        {
            return Attributes.Get1Bool(defaultVal, list);
        }

        public bool? Get1Bool(params string[] list)
        {
            return Attributes.Get1Bool(list);
        }

        public ulong Get1ULong(ulong defaultValue, params string[] list)
        {
            return Attributes.Get1ULong(defaultValue, list);
        }

        public ulong? Get1ULong(params string[] list)
        {
            return Attributes.Get1ULong(list);
        }

        public double? Get1Double(params string[] list)
        {
            return Attributes.Get1Double(list);
        }

        public double Get1Double(double defaultValue, params string[] list)
        {
            return Attributes.Get1Double(defaultValue, list);
        }

        public decimal? Get1Decimal(params string[] list)
        {
            return Attributes.Get1Decimal(list);
        }

        public decimal Get1Decimal(decimal defaultValue, params string[] list)
        {
            return Attributes.Get1Decimal(defaultValue, list);
        }

        public Guid? Get1Guid(params string[] list)
        {
            return Attributes.Get1Guid(list);
        }

        public Guid Get1Guid(Guid defaultValue, params string[] list)
        {
            return Attributes.Get1Guid(defaultValue, list);
        }

        public DSCPpid Get1DSCPpid(params string[] list)
        {
            return Attributes.Get1DSCPpid(list);
        }

        public List<DSCPpid> GetDSCPpids(params string[] list)
        {
            return Attributes.GetDSCPpids(list);
        }

        #endregion

        #region ExtendedObjects
        public List<DSRestData> GetExtensionObjects(params string[] path)
        {
            List<DSRestData> ret = new List<DSRestData>();

            if (!this.Objects.ContainsKey("EXT") || this.Objects["EXT"].Count < 1)
                return ret;

            List<DSRestData> coll = this.GetObjects("EXT").Values.ToList();
            for (int i = 0; i < path.Length; i++)
            {
                string name = path[i];
                foreach (string key in this.Objects["EXT"].Keys)
                {
                    if (name == "*" || key.StartsWith(name, StringComparison.OrdinalIgnoreCase))
                    {
                        if (!ret.Contains(this.Objects["EXT"][key]))
                            ret.Add(this.Objects["EXT"][key]);
                    }                        

                }
            }

            return ret;

        }
        #endregion

        #region Set

        public void Set1(string name, string value, DateTime? expire = null)
        {
            Attributes.Set1(name, value, expire);
        }
        
        public void Set(string name, string value, bool append, DateTime? expire = null, bool appendOnlyNew = true, bool setOnlyNewKey = false)
        {
            Attributes.Set(name, value, append, expire, appendOnlyNew, setOnlyNewKey);
        }

        public void Set(string name, List<Attr> attrs, bool append)
        {
            Attributes.Set(name, attrs, append);
        }

        public void Set1(string name, DateTime? value)
        {
            Attributes.Set1(name, value);
        }

        public void Set1(string name, DateTimeOffset? value)
        {
            Attributes.Set1(name, value);
        }

        public void Set1(string name, int? value)
        {
            Attributes.Set1(name, value);
        }

        public void Set1(string name, double? value)
        {
            Attributes.Set1(name, value);
        }

        public void Set1(string name, bool value)
        {
            Attributes.Set1(name, value);
        }

        public void Set1(string name, ulong? value)
        {
            Attributes.Set1(name, value);
        }

        public void Set1(string name, Guid? value)
        {
            Attributes.Set1(name, value);
        }

        public void Set1(string name, IDSCPpid reference)
        {
            Attributes.Set1(name, reference);
        }

        public void SetTransactionTag(string tag)
        {
            Attributes.Set1(TRANSACTION_TAG_KEY, tag);
        }
        #endregion

        #region Remove

        public void RemoveForKey(string key, string value = null)
        {
            Attributes.RemoveForKey(key, value);
        }

        public void RemoveForKeySFLT(string key, string sflt)
        {
            Attributes.RemoveForKeySFLT(key, sflt);
        }

        #endregion

        #region Exists

        public bool Exists(string name)
        {
            return Attributes.Exists(name);
        }

        #endregion

        #region TMP Attributes

        /// <summary>
        /// !!! can return Tmp Attributes from all level => code like:
        /// List<Attr> attrs = o.TmpAttributes(true);
        /// o.RemoveAllTmpAttributes();
        /// o.Attributes.AddRange(attrs);
        /// will promote deep attributes and is probably not what you want
        /// </summary>
        public List<Attr> TmpAttributes(bool includeExpired = false, bool includeNested = true)
        {
            List<Attr> attrs = Attributes.TmpAttributes(includeExpired, includeNested);
            if (includeNested)
            {
                foreach (string n in GetObjectNames())
                    foreach (DSRestData o in GetObjects(n).Values)
                        attrs.AddRange(o.TmpAttributes(includeExpired, includeNested));
            }
            return attrs;
        }

        public List<IAttrExt> GetTmpAttrExt(params string[] path)
        {
            return Attributes.GetTmpAttrExt(path);
        }

        public bool HasTmpAttributes(bool includeExpired = false)
        {
            if (Attributes.HasTmpAttributes())
                return true;
            foreach (string n in GetObjectNames())
                foreach (DSRestData o in GetObjects(n).Values)
                    if (o.HasTmpAttributes())
                        return true;
            return false;
        }

        /// <summary>
        /// This is copy not clone
        /// </summary>
        public void CopyTmpAttributes(DSRestData src, bool includeExpired = false)
        {
            Attributes.CopyTmpAttributes(src.Attributes, includeExpired);
            foreach (string n in src.GetObjectNames())
                foreach (KeyValuePair<string, DSRestData> ko in src.GetObjects(n))
                {
                    DSRestData o = GetObject(n, ko.Key, false);
                    if (o != null)
                        o.CopyTmpAttributes(ko.Value, includeExpired);
                }
        }

        public void RemoveAllTmpAttributes()
        {
            Attributes.RemoveAllTmpAttributes();
            foreach (string n in GetObjectNames())
                foreach (DSRestData o in GetObjects(n).Values)
                    o.RemoveAllTmpAttributes();
        }

        public void RemoveAllExpiredTmpAttributes()
        {
            Attributes.RemoveAllExpiredTmpAttributes();
            foreach (string n in GetObjectNames())
                foreach (DSRestData o in GetObjects(n).Values)
                    o.RemoveAllExpiredTmpAttributes();
        }



        #endregion

        #region Other


        //we only support int plus for now
        public void DeepAttributePlus(string value, string path)
        {
            Attributes.DeepAttributePlus(value, path);
        }

        public bool EqualAttribute(DSRestData other, params string[] path)
        {
            return Attributes.EqualAttribute(other.Attributes, path);
        }

        public void MergeAttribute(DSRestData other, params string[] path)
        {
            Attributes.MergeAttribute(other.Attributes, path);
        }

        public void CopyAttribute(DSRestData other, string key)
        {
            Attributes.CopyAttribute(other.Attributes, key);
        }

        #endregion


        #endregion

        #region Object attributes

        private DSGenCollection<DSCollection<DSRestData>> m_objects;
        public DSGenCollection<DSCollection<DSRestData>> Objects
        {
            get
            {
                if (m_objects == null)
                    m_objects = new DSGenCollection<DSCollection<DSRestData>>();
                return m_objects;
            }
            set
            {
                m_objects = value;
            }
        }

        public DSCollection<DSRestData> SetObjects(string name, DSCollection<DSRestData> value = null)
        {
            if (value == null)
            {
                if (Objects.ContainsKey(name.Trim()))
                    Objects.Remove(name.Trim());
                return null;
            }
            else
            {
                return Objects[name] = value.MakeBaseType();
            }
        }

        public void SetObjects(string name, DSCollection<DSRestProperty> value)
        {
            if (value == null)
            {
                if (Objects.ContainsKey(name.Trim()))
                    Objects.Remove(name.Trim());
            }
            else
            {
                Objects[name] = value.MakeBaseType();
            }
        }

        public void AddObject(string name, string key, DSRestData value)
        {
            DSCollection<DSRestData> objects = GetObjects(name);
            if (objects == null)
            {
                objects = new DSCollection<DSRestData>();
                objects = SetObjects(name, objects);
                objects = GetObjects(name);
            }


            if (value == null)
            {
                RemoveObject(name, key);
            }
            else
            {
                if (value is DSRestProperty)
                    objects[key] = value.MakeSubTypeSpecific<DSRestProperty>();
                else
                    objects[key] = value.MakeDSRestData();
            }
        }

        public void RemoveObject(string name, string key)
        {
            DSCollection<DSRestData> objects = GetObjects(name);
            if (objects != null && objects.ContainsKey(key.Trim()))
                objects.Remove(key.Trim());
        }

        public DSCollection<DSRestData> GetObjects(string name, bool emptyListOnNotFound = false)
        {
            DSCollection<DSRestData> ret = null;
            if (!Objects.TryGetValue(name.Trim(), out ret))
                return emptyListOnNotFound ? new DSCollection<DSRestData>() : null;
            return ret;
        }

        public DSCollection<DSRestData> GetObjects(List<string> names, bool emptyListOnNotFound = false)
        {
            DSCollection<DSRestData> ret = null;

            foreach (string name in names)
            {
                DSCollection<DSRestData> objs = GetObjects(name, emptyListOnNotFound);

                if (objs != null)
                {
                    ret = ret == null ? objs : ret.MergeLeft(objs);
                }
            }

            return ret;
        }


        public List<DSRestData> AllObjects()
        {
            return Objects.Aggregate(new List<DSRestData>(), (all, lo) => { all.AddRange(lo.Value.Values); return all; });
        }

        public DSRestData GetObject(string name, string key, bool createIfDoesntExist = false)
        {
            DSRestData o = null;
            if (Objects.ContainsKey(name))
                Objects[name].TryGetValue(key, out o);
            if (o == null && createIfDoesntExist == true)
            {
                o = new DSRestData();
                AddObject(name, key, o);
            }
            return o;
        }

        public T GetObject<T>(string name, string key, bool createIfDoesntExist = false) where T : DSRestData, new()
        {
            DSRestData d = GetObject(name, key, createIfDoesntExist);
            if (d == null)
                return null;
            return d.MakeSubTypeSpecific<T>();
        }

        public IEnumerable<string> GetObjectNames()
        {
            return Objects.Keys;
        }

        /// <summary>
        /// Remove all the objects for name
        /// </summary>
        /// <param name="name">if name == null remove all objects</param>
        public void RemoveObjects(string name)
        {
            if (name == null)
                Objects.Clear();
            else if (Objects.ContainsKey(name))
            {
                Objects.Remove(name);
                //Objects[name].Clear();
            }
        }

        #endregion

        #region Compare

        public class DSRestDataToString : DSRestData, IXmlSerializable
        {
            public class SerializableKVP<K, V>
            {
                public SerializableKVP(K k, V v) { Key = k; Value = v; }
                public SerializableKVP() { }
                public K Key { get; set; }
                public V Value { get; set; }
            }
            public List<SerializableKVP<string, List<SerializableKVP<string, string>>>> m_objectsForToString;

            #region IXmlSerializable Members

            System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
            {
                return null;
            }

            void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
            {
                throw new NotImplementedException();
            }

            void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
            {
                base.WriteXml(writer);
                if ((m_objectsForToString != null) && (m_objectsForToString.Count > 0))
                {
                    System.Xml.Serialization.XmlSerializer objectsSerializer = new System.Xml.Serialization.XmlSerializer(typeof(List<SerializableKVP<string, List<SerializableKVP<string, string>>>>));
                    objectsSerializer.Serialize(writer, m_objectsForToString);
                }
            }

            #endregion
        }

        public override string ToString()
        {
            return DataContractHelper.ObjectToXML(this);
        }

        public string ToString(bool isJson)
        {
            if (isJson)
                return this.ToJSon();
            else
                return this.ToString();
        }

        public string ToJSon()
        {
            return JsonHelper.ObjectToJson(this.ToDC());
        }


        public string ToComparableString()
        {
            Attributes.RemoveAll(a => ( (a as IAttrExt) == null && string.IsNullOrEmpty(a.Value)));
            DSRestDataToString o = new DSRestDataToString();
            o.Attributes = Attributes;
            o.Attributes.DeepSort();

            if (Objects.Count > 0)
            {
                o.m_objectsForToString = new List<DSRestDataToString.SerializableKVP<string, List<DSRestDataToString.SerializableKVP<string, string>>>>();
                foreach (string key in Objects.Keys)
                {
                    List<DSRestDataToString.SerializableKVP<string, string>> values = new List<DSRestDataToString.SerializableKVP<string, string>>();
                    foreach (string vkey in Objects[key].Keys)
                        values.Add(new DSRestDataToString.SerializableKVP<string, string>(vkey, ((DSRestData)Objects[key][vkey]).ToComparableString()));
                    values.Sort((a, b) => { return a.Key.CompareTo(b.Key); });
                    o.m_objectsForToString.Add(new DSRestDataToString.SerializableKVP<string, List<DSRestDataToString.SerializableKVP<string, string>>>(key, values));
                }
                o.m_objectsForToString.Sort((a, b) => { return a.Key.CompareTo(b.Key); });
            }
            return ((DSRestData)o).ToString();
        }


        public void DeepSort()
        {
            Attributes.DeepSort();

            foreach(DSCollection<DSRestData> objList in Objects.Values)
            {
                foreach(DSRestData obj in objList.Values)
                {
                    obj.DeepSort();
                }
            }
        }

        #endregion

        #region operate overloading

        public static bool operator ==(DSRestData item1, DSRestData item2)
        {
            if ((object)item1 == null)
                return (object)item2 == null;

            return item1.Equals(item2);
        }

        public static bool operator !=(DSRestData item1, DSRestData item2)
        {
            if ((object)item1 == null)
                return (object)item2 != null;

            return !item1.Equals(item2);
        }

        sealed public override bool Equals(object obj)
        {
            DSRestData restObj = null;
            if (obj != null && obj is DSRestData)
            {
                restObj = (DSRestData)obj;
            }
            else
                return false;

            return restObj.ToComparableString() == this.ToComparableString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        private List<Attr> ReadAttributes(System.Xml.XmlReader reader)
        {
            string eleName = reader.Name;
            if (eleName != "Attributes" && eleName != "AttrExt" && eleName != "TmpAttrExt")
                throw new Exception("ReadAttributes called on " + reader.Name);
            List<Attr> attrs = new List<Attr>();

            if (reader.IsEmptyElement)
            {
                reader.ReadStartElement(eleName);
                return attrs;
            }

            reader.ReadStartElement(eleName);

            while (reader.Name == "Attr" || reader.NodeType != XmlNodeType.EndElement)
            {
                if (reader.NodeType == XmlNodeType.EndElement) reader.ReadEndElement();

                if (reader.NodeType == XmlNodeType.EndElement) break;

                string name = reader.Name;
                string key = reader.GetAttribute("Key");
                string val = reader.GetAttribute("Value");
                Attr attr = null;
                
                switch (name)
                {
                    case "Attr":
                        attr = new Attr(key, val);
                        break;
                    case "AttrExt":
                        attr = new AttrExt(key, val);
                        break;
                    case "TmpAttr":
                        attr = new TmpAttr(key, val, Convert.ToDateTime(reader.GetAttribute("Expire")));
                        break;
                    case "TmpAttrExt":
                        attr = new TmpAttrExt(key, val, Convert.ToDateTime(reader.GetAttribute("Expire")));
                        break;
                    default:
                        throw new Exception();
                }

                IAttrExt attrExt = attr as IAttrExt;
                if (attrExt != null)
                {
                    attrExt.Extension = ReadAttributes(reader);
                }

                attrs.Add(attr);

                //next element is read already for ext type, no need to read here
                if (attrExt == null)
                {
                    reader.ReadStartElement();
                }
            }
            reader.ReadEndElement();
            return attrs;
        }

        private void WriteAttributes(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("Attributes");
            foreach (Attr attr in Attributes)
            {
                WriteAttr(writer, attr);
            }
            writer.WriteEndElement();
        }

        //recursively write attributes to support attrext
        private void WriteAttr(System.Xml.XmlWriter writer, Attr attr)
        {
            writer.WriteStartElement(attr.GetType().Name);

            writer.WriteAttributeString("Key", attr.Key);
            if(attr != null && attr.Value != null)
                writer.WriteAttributeString("Value", Regex.Replace(attr.Value, "[\u0000-\u0008\u000B-\u000C\u000E-\u001F]", ""));

            TmpAttr tmpAttr = attr as TmpAttr;
            if (tmpAttr != null)
            {
                writer.WriteAttributeString("Expire", tmpAttr.Expire.ToString());
            }

            IAttrExt attrExt = attr as IAttrExt;
            if (attrExt != null) 
            {
                foreach(Attr subAttr in attrExt.Extension)
                {
                    WriteAttr(writer, subAttr);
                }
            }

            writer.WriteEndElement();
        }

        public void ReadXml(System.Xml.XmlReader origReader)
        {
            System.Xml.XmlReader reader;
            if (origReader.Settings != null && origReader.Settings.IgnoreWhitespace)
            {
                reader = origReader;
            }
            else
            {
                XmlReaderSettings readsettings = new XmlReaderSettings();
                readsettings.IgnoreWhitespace = true;
                readsettings.IgnoreComments = true;

                reader = XmlReader.Create(origReader.ReadSubtree(), readsettings);
            }

            bool wasEmpty = reader.IsEmptyElement;
            reader.ReadStartElement();

            if (wasEmpty)
                return;

            if (reader.Name == "Attributes")
                Attributes = ReadAttributes(reader);
            if (reader.Name == "Objects")
            {
                System.Xml.Serialization.XmlSerializer objectSerializer = new System.Xml.Serialization.XmlSerializer(typeof(DSGenCollection<DSCollection<DSRestData>>));
                Objects = (DSGenCollection<DSCollection<DSRestData>>)objectSerializer.Deserialize(reader);
            }
            if (reader.NodeType == System.Xml.XmlNodeType.EndElement)
                reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            if (KeyName != null)
                writer.WriteAttributeString("Key", KeyName);
            if ((Attributes != null) && (Attributes.Count > 0))
                WriteAttributes(writer);
            if ((Objects != null) && (Objects.Count > 0))
            {
                System.Xml.Serialization.XmlSerializer objectSerializer = new System.Xml.Serialization.XmlSerializer(typeof(DSGenCollection<DSCollection<DSRestData>>));
                objectSerializer.Serialize(writer, Objects);
            }
        }

        #endregion

        #region IDSCollectionValue Members

        private string m_keyName;
        public string KeyName
        {
            get
            {
                return m_keyName;
            }
            set
            {
                m_keyName = value;
            }
        }

        #endregion

        public object Clone()
        {
            DSRestData c = new DSRestData();
            c.Attributes.AddRange(Attributes.Select(a => a.Clone()));
            c.Objects = (DSGenCollection<DSCollection<DSRestData>>)Objects.Clone();
            return c;
        }

        public T Clone<T>() where T : DSRestData, new()
        {
            DSRestData c = (DSRestData)Clone();
            DSRestData p = new DSRestData();
            p.Attributes = c.Attributes;
            p.Objects = c.Objects;
            return p.MakeSubTypeSpecific<T>();
        }

        #region for known subtypes
        /// <summary>
        /// this is for DataContractor serialization, any subtype can added to knowntype even in a separate assembly so rest webservice can return subtype of DSRestProperty
        /// example code in DSLib.DSStore.RestObjects.DSTaxonomy static constructor: DSRestData.DerivedTypes.Add(typeof(DSTaxonomy))
        /// </summary>
        public static List<Type> DerivedTypes = new List<Type>();

        private static IEnumerable<Type> GetDerivedTypes()
        {
            return DerivedTypes;
        }

        #endregion
    }

    [DataContract(Namespace = "")]
    sealed public class DSRestDataDC
    {
        public DSRestData ToNormal()
        {
            return new DSRestData(Attributes, ObjectsToNormal());
        }

        [IgnoreDataMember]
        private List<Attr> m_attributes;
        [DataMember]
        public List<Attr> Attributes
        {
            get
            {
                if (m_attributes == null)
                    m_attributes = new List<Attr>();
                return m_attributes;
            }
            set
            {
                m_attributes = value;
            }
        }

        [IgnoreDataMember]
        private DSGenCollectionDC<DSCollectionDC> m_objects;
        [DataMember]
        public DSGenCollectionDC<DSCollectionDC> Objects
        {
            get
            {
                if (m_objects == null)
                    m_objects = new DSGenCollectionDC<DSCollectionDC>();
                return m_objects;
            }
            set
            {
                m_objects = value;
            }
        }

        public DSGenCollection<DSCollection<DSRestData>> ObjectsToNormal()
        {
            DSGenCollection<DSCollection<DSRestData>> no = new DSGenCollection<DSCollection<DSRestData>>();
            foreach (string key in Objects.Keys)
                no.Add(key, Objects[key].ToNormal());
            return no;
        }
    }
}