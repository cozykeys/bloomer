﻿namespace KbUtil.Lib.SvgGeneration.Internal
{
    using System;
    using System.IO;
    using System.Xml;
    using KbUtil.Lib.Models.Keyboard;

    internal class GroupWriter : IElementWriter<Group>
    {
        public SvgGenerationOptions GenerationOptions { get; set; }

        public void Write(XmlWriter writer, Group group)
        {
            writer.WriteStartElement("g");

            var elementWriter = new ElementWriter { GenerationOptions = GenerationOptions };

            elementWriter.WriteAttributes(writer, group);
            WriteAttributes(writer, group);

            elementWriter.WriteSubElements(writer, group);
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
                        var keyWriter = new KeyWriter { GenerationOptions = GenerationOptions };
                        keyWriter.Write(writer, (Key)key);
                        break;
                    case var spacer when child is Spacer:
                        var spacerWriter = new SpacerWriter { GenerationOptions = GenerationOptions };
                        spacerWriter.Write(writer, (Spacer)spacer);
                        break;
                    case var stack when child is Stack:
                        var stackWriter = new StackWriter { GenerationOptions = GenerationOptions };
                        stackWriter.Write(writer, (Stack)stack);
                        break;
                    case var @case when child is Case:
                        var caseWriter = new CaseWriter { GenerationOptions = GenerationOptions };
                        caseWriter.Write(writer, (Case)@case);
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