namespace KbUtil.Lib.Deserialization.Internal
{
    using KbUtil.Lib.Models.Attributes;
    using KbUtil.Lib.Models.Keyboard;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;

    internal class GroupDeserializer : IDeserializer<Group>
    {
        public static GroupDeserializer Default { get; set; } = new GroupDeserializer();

        public void Deserialize(XElement groupElement, Group group)
        {
            ElementDeserializer.Default.Deserialize(groupElement, group);

            DeserializeChildren(groupElement, group);
        }

        private static void DeserializeChildren(XElement groupElement, Group group)
        {
            if (XmlUtilities.TryGetSubElement(groupElement, "Children", out XElement childrenElement))
            {
                IEnumerable<XElement> childElements = childrenElement
                    .Nodes()
                    .Where(node =>
                        node.NodeType == System.Xml.XmlNodeType.Element)
                    .Select(node => (XElement)node);

                List<Element> children = childElements
                    .Select(childElement => DeserializeChild(group, childElement))
                    .ToList();

                group.Children = children;
            }
        }

        private static Element DeserializeChild(Element parent, XElement childElement)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();

            string childElementName = childElement.Name.ToString();
            Type childType = types.Single(type => type.Name == childElementName);

            Func<Type, bool> isTypeValidGroupChild = (type) => type.CustomAttributes
                .Any(attr => attr.AttributeType == typeof(GroupChildAttribute));

            if (childType == null || !isTypeValidGroupChild(childType))
            {
                var attrs = childType.CustomAttributes;
                throw new NotSupportedException();
            }

            Element child = (Element) Activator.CreateInstance(childType);
            child.Parent = parent;

            Type deserializerType = types
                .Single(type => type.Name == $"{childType.Name}Deserializer");

            object deserializer = Activator.CreateInstance(deserializerType);
            MethodInfo method = deserializerType.GetMethod("Deserialize");
            method.Invoke(deserializer, new object[] { childElement, child });

            return child;
        }
    }
}
