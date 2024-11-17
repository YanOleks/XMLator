using System;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace XMLator.Transformers
{
    internal class XmlToHtmlTransformer
    {
        private readonly string _xslFilePath;

        public XmlToHtmlTransformer(string xslFilePath)
        {
            _xslFilePath = xslFilePath;
        }

        public void Transform(Stream xmlStream, string outputHtmlFilePath)
        {
            if (string.IsNullOrWhiteSpace(_xslFilePath) || !File.Exists(_xslFilePath))
                throw new FileNotFoundException("XSL файл не знайдено", _xslFilePath);

            try
            {
                XslCompiledTransform xslTransform = new();
                xslTransform.Load(_xslFilePath);

                using XmlReader xmlReader = XmlReader.Create(xmlStream);
                using XmlWriter writer = XmlWriter.Create(outputHtmlFilePath, new XmlWriterSettings { Indent = true });

                xslTransform.Transform(xmlReader, writer);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Помилка трансформації XML в HTML", ex);
            }
        }
    }
}
