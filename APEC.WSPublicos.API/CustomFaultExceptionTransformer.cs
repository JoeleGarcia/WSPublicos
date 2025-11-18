using SoapCore.Extensibility;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Xml;
using System.Xml.Linq;

namespace APEC.WSPublicos.API
{
    public class CustomFaultExceptionTransformer : IFaultExceptionTransformer
    {
        public Message ProvideFault(
            Exception exception,
            MessageVersion messageVersion,
            Message requestMessage,
            XmlNamespaceManager xmlNamespaceManager)
        {
            string faultMessage = exception is FaultException faultException
                ? faultException.Message
                : $"Error interno: {exception.Message}";

            // Crear un cuerpo SOAP Fault personalizado
            var faultXml = CreateFaultXml(faultMessage);

            // Crear el mensaje SOAP con el cuerpo de falla
            return CreateMessageWithFault(messageVersion, faultXml);
        }

        private XElement CreateFaultXml(string faultString)
        {
            return new XElement(XName.Get("Fault", "http://schemas.xmlsoap.org/soap/envelope/"),
                new XElement("faultcode", "Server"),
                new XElement("faultstring", faultString)
            );
        }

        private Message CreateMessageWithFault(MessageVersion version, XElement faultXml)
        {
            // Crear un BodyWriter personalizado para el XML de falla
            var bodyWriter = new FaultBodyWriter(faultXml);
            return Message.CreateMessage(version, "", bodyWriter);
        }

        private class FaultBodyWriter : BodyWriter
        {
            private readonly XElement _faultXml;

            public FaultBodyWriter(XElement faultXml) : base(true)
            {
                _faultXml = faultXml;
            }

            protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
            {
                _faultXml.WriteTo(writer);
            }
        }
    }
}
