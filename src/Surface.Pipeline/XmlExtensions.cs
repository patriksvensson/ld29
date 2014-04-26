using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Surface.Pipeline
{
    internal static class XmlExtensions
    {
        public static XElement GetDocumentRoot(this XDocument document, string name, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            if (document.Root != null)
            {
                var rootName = document.Root.Name.LocalName;
                if (rootName.Equals(name, comparison))
                {
                    return document.Root;
                }
            }
            return null;
        }

        public static bool HasElement(this XElement element, string name, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            return GetElement(element, name, comparison) != null;
        }

        public static XElement GetElement(this XElement element, string name, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            return element.Elements().FirstOrDefault(e => e.Name.LocalName.Equals(name, comparison));
        }

        public static IEnumerable<XElement> GetElements(this XElement element, string name, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            return element.Elements().Where(e => e.Name.LocalName.Equals(name, comparison));
        }

        public static bool HasAttribute(this XElement element, string name)
        {
            if (element != null)
            {
                XAttribute attribute = element.Attribute(name);
                return attribute != null;
            }
            return false;
        }

        public static T ReadAttribute<T>(this XElement element, string name, T defaultValue)
        {
            if (element != null)
            {
                XAttribute attribute = element.Attribute(name);
                if (attribute != null)
                {
                    return attribute.Value.Convert(defaultValue);
                }
            }
            return defaultValue;
        }
    }
}