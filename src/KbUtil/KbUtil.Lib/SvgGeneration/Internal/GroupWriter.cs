namespace KbUtil.Lib.SvgGeneration.Internal
{
    using System;
    using System.IO;
    using System.Xml;
    using KbUtil.Lib.Models.Keyboard;

    internal class GroupWriter : IElementWriter<Group>
    {
        private GroupWriter()
        {
        }

        public static GroupWriter Instance { get; } = new GroupWriter();

        public void Write(XmlWriter writer, Group group)
        {
            writer.WriteStartElement("g");

            // Attributes
            ElementWriter.Instance.WriteAttributes(writer, group);
            WriteAttributes(writer, group);

            // Elements
            ElementWriter.Instance.WriteSubElements(writer, group);
            WriteSubElements(writer, group);

            writer.WriteEndElement();
        }

        public void WriteAttributes(XmlWriter writer, Group group)
        {
        }

        public void WriteSubElements(XmlWriter writer, Group group)
        {
            foreach (Element child in group.Children)
            {
                switch (child)
                {
                    case var _ when child is Keyboard:
                        throw new InvalidDataException("Keyboard is not a valid child type.");
                    case var key when child is Key:
                        KeyWriter.Instance.Write(writer, (Key)key);
                        break;
                    case var stack when child is Stack:
                        StackWriter.Instance.Write(writer, (Stack)stack);
                        break;
                    case var subGroup when child is Group:
                        Write(writer, (Group)subGroup);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
        }
    }
}
