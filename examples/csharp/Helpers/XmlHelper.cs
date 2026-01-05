namespace W12CSheets.Client.Helpers;

/// <summary>
/// XML helper utilities
/// </summary>
public static class XmlHelper
{
    /// <summary>
    /// Serialize object to XML string
    /// </summary>
    public static string Serialize<T>(T obj)
    {
        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        using var writer = new System.IO.StringWriter();
        serializer.Serialize(writer, obj);
        return writer.ToString();
    }

    /// <summary>
    /// Deserialize XML string to object
    /// </summary>
    public static T? Deserialize<T>(string xml)
    {
        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        using var reader = new System.IO.StringReader(xml);
        return (T?)serializer.Deserialize(reader);
    }

    /// <summary>
    /// Pretty print XML
    /// </summary>
    public static string PrettyPrint(string xml)
    {
        var doc = new System.Xml.XmlDocument();
        doc.LoadXml(xml);
        
        using var writer = new System.IO.StringWriter();
        using var xmlWriter = new System.Xml.XmlTextWriter(writer)
        {
            Formatting = System.Xml.Formatting.Indented,
            Indentation = 2
        };
        
        doc.WriteContentTo(xmlWriter);
        return writer.ToString();
    }

    /// <summary>
    /// Validate XML against XSD schema
    /// </summary>
    public static bool IsValid(string xml, string xsd)
    {
        try
        {
            var schemas = new System.Xml.Schema.XmlSchemaSet();
            schemas.Add("", System.Xml.XmlReader.Create(new System.IO.StringReader(xsd)));
            
            var doc = new System.Xml.XmlDocument();
            doc.LoadXml(xml);
            doc.Schemas = schemas;
            
            bool isValid = true;
            doc.Validate((sender, e) => { isValid = false; });
            
            return isValid;
        }
        catch
        {
            return false;
        }
    }
}
