using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Markup;

namespace Movie_Cleanup.Modules.Shell
{
    //class PropertyKey
    //{
    //    readonly TypeKey typeKey;
    //    readonly string type;
    //    readonly string name;
    //    readonly MethodAttributes getterMethodAttributes;

    //    public PropertyKey(TypeKey typeKey, PropertyDefinition prop)
    //    {
    //        this.typeKey = typeKey;
    //        this.type = prop.PropertyType.FullName;
    //        this.name = prop.Name;
    //        this.getterMethodAttributes = prop.GetMethod != null ? prop.GetMethod.Attributes : 0;
    //    }

    //    public TypeKey TypeKey
    //    {
    //        get { return typeKey; }
    //    }

    //    public string Type
    //    {
    //        get { return type; }
    //    }

    //    public string Name
    //    {
    //        get { return name; }
    //    }

    //    public MethodAttributes GetterMethodAttributes
    //    {
    //        get { return getterMethodAttributes; }
    //    }

    //    public virtual bool Matches(MemberReference member)
    //    {
    //        PropertyReference propRef = member as PropertyReference;
    //        if (propRef != null)
    //        {
    //            if (typeKey.Matches(propRef.DeclaringType))
    //                return type == propRef.PropertyType.FullName && name == propRef.Name;
    //        }

    //        return false;
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        PropertyKey key = obj as PropertyKey;
    //        if (key == null)
    //            return false;

    //        return this == key;
    //    }

    //    public static bool operator ==(PropertyKey a, PropertyKey b)
    //    {
    //        if ((object)a == null)
    //            return (object)b == null;
    //        else if ((object)b == null)
    //            return false;
    //        else
    //            return a.typeKey == b.typeKey && a.type == b.type && a.name == b.name;
    //    }

    //    public static bool operator !=(PropertyKey a, PropertyKey b)
    //    {
    //        if ((object)a == null)
    //            return (object)b != null;
    //        else if ((object)b == null)
    //            return true;
    //        else
    //            return a.typeKey != b.typeKey || a.type != b.type || a.name != b.name;
    //    }

    //    public override int GetHashCode()
    //    {
    //        return typeKey.GetHashCode() ^ type.GetHashCode() ^ name.GetHashCode();
    //    }

    //    public override string ToString()
    //    {
    //        return String.Format("[{0}]{1} {2}::{3}", typeKey.Scope, type, typeKey.Fullname, name);
    //    }
    //}
}
