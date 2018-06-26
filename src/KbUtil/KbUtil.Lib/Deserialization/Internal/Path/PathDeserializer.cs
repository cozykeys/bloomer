namespace KbUtil.Lib.Deserialization.Path
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Linq;
    using Models.Path;
    using KbUtil.Lib.Deserialization.Internal;

    public class PathDeserializer : IDeserializer<Path>
    {
        private static IDictionary<string, object> _componentDeserializerMap;
        private static IDictionary<string, Type> _componentTypeMap;

        static PathDeserializer()
        {
            InitializeComponentDeserializerMap();
            InitializeComponentTypeMap();
        }

        public void Deserialize(XElement pathElement, Path path)
        {
            var componentsElement = (XElement) pathElement
                .Nodes()
                .Single(node =>
                    node.NodeType == XmlNodeType.Element
                    && ((XElement) node).Name == "Components");

            path.Components = componentsElement
                .Nodes()
                .Where(node => node.NodeType == XmlNodeType.Element)
                .Select(componentElement => DeserializeComponent((XElement)componentElement));
        }

        private static void InitializeComponentDeserializerMap()
        {
            Func<Type, bool> isDeserializer = type => type
                .GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDeserializer<>));

            IEnumerable<Type> deserializerTypes = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(isDeserializer);

            _componentDeserializerMap = deserializerTypes
                .ToDictionary(type => type.Name, Activator.CreateInstance);
        }

        private static void InitializeComponentTypeMap()
        {
            IEnumerable<Type> pathComponentTypes = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(type => type.GetInterfaces().Contains(typeof(IPathComponent)));

            _componentTypeMap = pathComponentTypes.ToDictionary(type => type.Name);
        }

        private static IPathComponent DeserializeComponent(XElement componentElement)
        {
            string elementName = componentElement.Name.ToString();

            Type elementType = _componentTypeMap[elementName];
            var component = (IPathComponent) Activator.CreateInstance(elementType);

            object deserializer = _componentDeserializerMap[$"{elementName}Deserializer"];
            MethodInfo deserializeMethod = deserializer.GetType().GetMethod("Deserialize");

            if (deserializeMethod == null)
            {
                throw new Exception("TODO");
            }

            var methodParameters = new object[] { componentElement, component };
            deserializeMethod.Invoke(deserializer, methodParameters);

            return component;
        }
    }
}
