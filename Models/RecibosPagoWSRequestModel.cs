using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Telerik.SvgIcons;

namespace GuanajuatoAdminUsuarios.Models
{

    //[XmlRoot("soapenv:Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")] // Serialized with root element name "GetUserProfileResponse" in namespace "http://schemas.abudhabi.ae/sso/2010/11".
    [XmlRoot(ElementName = "reversaDePago", Namespace = "http://modificacion.recibos.sittegws/")]
    public class RecibosPagoWSRequestModel
    {
        public string UsuarioLog { get; set; }
        public string PasswordLog { get; set; }
        public string ReciboControlInterno { get; set; }
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
        public string FechaReversa { get; set; }
    }

    [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Body
    {
        [XmlElement(ElementName = "reversaDePago", Namespace = "http://modificacion.recibos.sittegws/")]
        public RecibosPagoWSRequestModel ReversaDePago { get; set; }
    }

    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope
    {
        [XmlElement(ElementName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public string Header { get; set; }
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Body Body { get; set; }
        [XmlAttribute(AttributeName = "soapenv", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Soapenv { get; set; }
        [XmlAttribute(AttributeName = "mod", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Mod { get; set; }
    }
}
